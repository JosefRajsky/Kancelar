
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Ukol_Api.Repositories
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
            var item = db.Ukoly.FirstOrDefault(u => u.UkolId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.UkolCreated);
            msgTypes.Add(MessageType.UkolUpdated);
            msgTypes.Add(MessageType.UkolRemoved);
            await _handler.RequestReplay("Ukollate.ex", entityId, msgTypes);           
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
                        var create = JsonConvert.DeserializeObject<EventUkolCreated>(msg.Event);
                        var forCreate = db.Ukoly.FirstOrDefault(u => u.UkolId == create.UkolId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Ukoly.Add(forCreate);
                            db.SaveChanges();
                        }
                        
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventUkolDeleted>(msg.Event);
                        var forRemove = db.Ukoly.FirstOrDefault(u => u.UkolId == remove.UkolId);
                        if (forRemove != null) db.Ukoly.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventUkolUpdated>(msg.Event);
                        var forUpdate = db.Ukoly.FirstOrDefault(u => u.UkolId == update.UkolId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update,forUpdate);
                            db.Ukoly.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Ukol Create(EventUkolCreated evt)
        {
            var model = new Ukol()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                UkolId = evt.UkolId,
                Value1 = evt.UkolValue1,
                Value2 = evt.UkolValue2
            };
            return model;
        }
        private Ukol Modify(EventUkolUpdated evt, Ukol item)
        {           
            item.EventGuid = evt.EventId;
            item.Value1 = evt.UkolValue1;
            item.Value2 = evt.UkolValue2;
            return item;
        }

        public async Task<Ukol> Get(Guid id) => await Task.Run(() => db.Ukoly.FirstOrDefault(b => b.UkolId == id));
        public async Task<List<Ukol>> GetList() => await db.Ukoly.ToListAsync();
        public async Task Add(CommandUkolCreate cmd)
        {
            var ev = new EventUkolCreated()
            {
                EventId = Guid.NewGuid(),                           
                Generation = 0,
                UkolId = Guid.NewGuid(),
            };              
                var item = Create(ev);
                db.Ukoly.Add(item);
                await db.SaveChangesAsync();                
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.UkolId);
            
        }
        public async Task Update(CommandUkolUpdate cmd)
        {
            var item = db.Ukoly.FirstOrDefault(u => u.UkolId == cmd.UkolId);                   
            if (item != null) {
                var ev = new EventUkolUpdated()
                {
                    EventId = Guid.NewGuid(),
                    UkolValue1 = cmd.UkolValue1,
                    UkolValue2 = cmd.UkolValue2,

                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.UkolUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.UkolId);
                db.Ukoly.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandUkolRemove cmd)
        {
            var remove = db.Ukoly.FirstOrDefault(u => u.UkolId == cmd.UkolId);
            if (remove != null) {
                
                var ev = new EventUkolDeleted()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    UkolId = cmd.UkolId,
                };
                db.Ukoly.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.UkolRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.UkolId);
                await db.SaveChangesAsync();
            }

        }


    }

}








