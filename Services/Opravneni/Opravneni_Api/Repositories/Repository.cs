
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Opravneni_Api.Repositories
{
    public class Repository : IRepository
    {
        private readonly ServiceDbContext db;
        private MessageHandler _handler;
        public Repository(ServiceDbContext dbContext, Publisher publisher)
        {
            db = dbContext;          
            _handler = new MessageHandler(publisher);

        }
        public async Task LastEventCheck(Guid eventId, Guid entityId)
        {
            var item = db.Opravneni.FirstOrDefault(u => u.PravoId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.OpravneniCreated);
            msgTypes.Add(MessageType.OpravneniUpdated);
            msgTypes.Add(MessageType.OpravneniRemoved);
            await _handler.RequestReplay("Opravnenilate.ex", entityId, msgTypes);           
        }
        public async Task ReplayEvents(List<string> stream, Guid? entityId)
        {
            var messages = new List<Message>();
            foreach (var item in stream)
            {
                messages.Add(JsonConvert.DeserializeObject<Message>(item));
            }
            var replayOrderedStream = messages.OrderBy(d => d.Created);
            foreach (var msg in replayOrderedStream)
            {
                switch (msg.MessageType)
                {
                    case MessageType.UzivatelCreated:
                        var create = JsonConvert.DeserializeObject<EventOpravneniCreated>(msg.Event);
                        var forCreate = db.Opravneni.FirstOrDefault(u => u.PravoId == create.OpravneniId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Opravneni.Add(forCreate);
                            db.SaveChanges();
                        }
                        
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventOpravneniDeleted>(msg.Event);
                        var forRemove = db.Opravneni.FirstOrDefault(u => u.PravoId == remove.OpravneniId);
                        if (forRemove != null) db.Opravneni.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventOpravneniUpdated>(msg.Event);
                        var forUpdate = db.Opravneni.FirstOrDefault(u => u.PravoId == update.OpravneniId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update,forUpdate);
                            db.Opravneni.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Pravo Create(EventOpravneniCreated evt)
        {
            var model = new Pravo()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                PravoId = evt.OpravneniId,
                Value1 = evt.OpravneniValue1,
                Value2 = evt.OpravneniValue2
            };
            return model;
        }
        private Pravo Modify(EventOpravneniUpdated evt, Pravo item)
        {           
            item.EventGuid = evt.EventId;
            item.Value1 = evt.OpravneniValue1;
            item.Value2 = evt.OpravneniValue2;
            return item;
        }

        public async Task<Pravo> Get(Guid id) => await Task.Run(() => db.Opravneni.FirstOrDefault(b => b.PravoId == id));
        public async Task<List<Pravo>> GetList() => await db.Opravneni.ToListAsync();
        public async Task Add(CommandOpravneniCreate cmd)
        {
            var ev = new EventOpravneniCreated()
            {
                EventId = Guid.NewGuid(),                           
                Generation = 0,
                OpravneniId = Guid.NewGuid(),
            };              
                var item = Create(ev);
                db.Opravneni.Add(item);
                await db.SaveChangesAsync();                
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.PravoId);
            
        }
        public async Task Update(CommandOpravneniUpdate cmd)
        {
            var item = db.Opravneni.FirstOrDefault(u => u.PravoId == cmd.OpravneniId);                   
            if (item != null) {
                var ev = new EventOpravneniUpdated()
                {
                    EventId = Guid.NewGuid(),
                    OpravneniValue1 = cmd.OpravneniValue1,
                    OpravneniValue2 = cmd.OpravneniValue2,

                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.OpravneniUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.OpravneniId);
                db.Opravneni.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandOpravneniRemove cmd)
        {
            var remove = db.Opravneni.FirstOrDefault(u => u.PravoId == cmd.OpravneniId);
            if (remove != null) {
                
                var ev = new EventOpravneniDeleted()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    OpravneniId = cmd.OpravneniId,
                };
                db.Opravneni.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.OpravneniRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.PravoId);
                await db.SaveChangesAsync();
            }

        }


    }

}








