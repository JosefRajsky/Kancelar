
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Mzdy_Api.Repositories
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
            var item = db.Mzdy.FirstOrDefault(u => u.MzdaId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.MzdyCreated);
            msgTypes.Add(MessageType.MzdyUpdated);
            msgTypes.Add(MessageType.MzdyRemoved);
            await _handler.RequestReplay("Mzdylate.ex", entityId, msgTypes);           
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
                        var create = JsonConvert.DeserializeObject<EventMzdyCreated>(msg.Event);
                        var forCreate = db.Mzdy.FirstOrDefault(u => u.MzdaId == create.MzdyId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Mzdy.Add(forCreate);
                            db.SaveChanges();
                        }
                        
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventMzdyDeleted>(msg.Event);
                        var forRemove = db.Mzdy.FirstOrDefault(u => u.MzdaId == remove.MzdyId);
                        if (forRemove != null) db.Mzdy.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventMzdyUpdated>(msg.Event);
                        var forUpdate = db.Mzdy.FirstOrDefault(u => u.MzdaId == update.MzdyId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update,forUpdate);
                            db.Mzdy.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Mzda Create(EventMzdyCreated evt)
        {
            var model = new Mzda()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                MzdaId = evt.MzdyId,
                Value1 = evt.MzdyValue1,
                Value2 = evt.MzdyValue2
            };
            return model;
        }
        private Mzda Modify(EventMzdyUpdated evt, Mzda item)
        {           
            item.EventGuid = evt.EventId;
            item.Value1 = evt.MzdyValue1;
            item.Value2 = evt.MzdyValue2;
            return item;
        }

        public async Task<Mzda> Get(Guid id) => await Task.Run(() => db.Mzdy.FirstOrDefault(b => b.MzdaId == id));
        public async Task<List<Mzda>> GetList() => await db.Mzdy.ToListAsync();
        public async Task Add(CommandMzdyCreate cmd)
        {
            var ev = new EventMzdyCreated()
            {
                EventId = Guid.NewGuid(),                           
                Generation = 0,
                MzdyId = Guid.NewGuid(),
            };              
                var item = Create(ev);
                db.Mzdy.Add(item);
                await db.SaveChangesAsync();                
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.MzdaId);
            
        }
        public async Task Update(CommandMzdyUpdate cmd)
        {
            var item = db.Mzdy.FirstOrDefault(u => u.MzdaId == cmd.MzdyId);                   
            if (item != null) {
                var ev = new EventMzdyUpdated()
                {
                    EventId = Guid.NewGuid(),
                    MzdyValue1 = cmd.MzdyValue1,
                    MzdyValue2 = cmd.MzdyValue2,

                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.MzdyUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.MzdyId);
                db.Mzdy.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandMzdyRemove cmd)
        {
            var remove = db.Mzdy.FirstOrDefault(u => u.MzdaId == cmd.MzdyId);
            if (remove != null) {
                
                var ev = new EventMzdyDeleted()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    MzdyId = cmd.MzdyId,
                };
                db.Mzdy.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.MzdyRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.MzdaId);
                await db.SaveChangesAsync();
            }

        }


    }

}








