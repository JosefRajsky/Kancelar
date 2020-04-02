
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Template.Repositories
{
    public class Repository : IRepository
    {
        private readonly TemplateDbContext db;
        private Publisher _publisher;
        private MessageHandler _handler;
        public Repository(TemplateDbContext dbContext, Publisher publisher)
        {
            db = dbContext;
            _publisher = publisher;
            _handler = new MessageHandler(publisher);

        }
        public async Task LastEventCheck(Guid eventId, Guid entityId)
        {
            var item = db.Temps.FirstOrDefault(u => u.TempId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.TempCreated);
            msgTypes.Add(MessageType.UzivatelUpdated);
            msgTypes.Add(MessageType.UzivatelRemoved);
            await _handler.RequestReplay("uzivatel.ex", entityId, msgTypes);           
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
                        var create = JsonConvert.DeserializeObject<EventTempCreated>(msg.Event);
                        var forCreate = db.Temps.FirstOrDefault(u => u.TempId == create.TempId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Temps.Add(forCreate);
                            db.SaveChanges();
        
                        }
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventTempDeleted>(msg.Event);
                        var forRemove = db.Temps.FirstOrDefault(u => u.TempId == remove.TempId);
                        if (forRemove != null) db.Temps.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventTempUpdated>(msg.Event);
                        var forUpdate = db.Temps.FirstOrDefault(u => u.TempId == update.TempId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update,forUpdate);
                            db.Temps.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Temp Create(EventTempCreated evt)
        {
            var model = new Temp()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                TempId = evt.TempId,
                TempValue1 = evt.TempValue1,
                TempValue2 = evt.TempValue2
            };
            return model;
        }
        private Temp Modify(EventTempUpdated evt, Temp item)
        {           
            item.EventGuid = evt.EventId;
            item.TempValue1 = evt.TempValue1;
            item.TempValue2 = evt.TempValue2;
            return item;
        }

        public async Task<Temp> Get(Guid id) => await Task.Run(() => db.Temps.FirstOrDefault(b => b.TempId == id));
        public async Task<List<Temp>> GetList() => await db.Temps.ToListAsync();
        public async Task Add(CommandTempCreate cmd)
        {
            var ev = new EventTempCreated()
            {
                EventId = Guid.NewGuid(),                           
                Generation = 0,
                TempId = Guid.NewGuid(),
            };              
                var item = Create(ev);
                db.Temps.Add(item);
                await db.SaveChangesAsync();                
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.TempId);
            
        }
        public async Task Update(CommandTempUpdate cmd)
        {
            var item = db.Temps.FirstOrDefault(u => u.TempId == cmd.TempId);                   
            if (item != null) {
                var ev = new EventTempUpdated()
                {
                    EventId = Guid.NewGuid(),
                    TempValue1 = cmd.TempValue1,
                    TempValue2 = cmd.TempValue2,

                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.TempUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.TempId);
                db.Temps.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandTempRemove cmd)
        {
            var remove = db.Temps.FirstOrDefault(u => u.TempId == cmd.TempId);
            if (remove != null) {
                
                var ev = new EventTempDeleted()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    TempId = cmd.TempId,
                };
                db.Temps.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.TempRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.TempId);
                await db.SaveChangesAsync();
            }

        }


    }

}








