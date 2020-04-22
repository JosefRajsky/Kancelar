
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Vykaz_Api.Repositories
{
    public class Repository : IRepository
    {
        private readonly ServiceDbContext db;
        private readonly MessageHandler _handler;
        public Repository(ServiceDbContext dbContext, Publisher publisher)
        {
            db = dbContext;          
            _handler = new MessageHandler(publisher);

        }
        public async Task LastEventCheck(Guid eventId, Guid entityId)
        {
            var item = db.Vykazy.FirstOrDefault(u => u.VykazId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>
            {
                MessageType.VykazCreated,
                MessageType.VykazUpdated,
                MessageType.VykazRemoved
            };
            await _handler.RequestReplay("vykaz.ex", entityId, msgTypes);           
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
                        var create = JsonConvert.DeserializeObject<EventVykazCreated>(msg.Event);
                        var forCreate = db.Vykazy.FirstOrDefault(u => u.VykazId == create.VykazId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Vykazy.Add(forCreate);
                            db.SaveChanges();
                        }
                        
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventVykazDeleted>(msg.Event);
                        var forRemove = db.Vykazy.FirstOrDefault(u => u.VykazId == remove.VykazId);
                        if (forRemove != null) db.Vykazy.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventVykazUpdated>(msg.Event);
                        var forUpdate = db.Vykazy.FirstOrDefault(u => u.VykazId == update.VykazId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update,forUpdate);
                            db.Vykazy.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Vykaz Create(EventVykazCreated evt)
        {
            var model = new Vykaz()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                VykazId = evt.VykazId,
                Value1 = evt.VykazValue1,
                Value2 = evt.VykazValue2
            };
            return model;
        }
        private Vykaz Modify(EventVykazUpdated evt, Vykaz item)
        {           
            item.EventGuid = evt.EventId;
            item.Value1 = evt.VykazValue1;
            item.Value2 = evt.VykazValue2;
            return item;
        }

        public async Task<Vykaz> Get(Guid id) => await Task.Run(() => db.Vykazy.FirstOrDefault(b => b.VykazId == id));
        public async Task<List<Vykaz>> GetList() => await db.Vykazy.ToListAsync();
        public async Task Add(CommandVykazCreate cmd)
        {
            var ev = new EventVykazCreated()
            {
                EventId = Guid.NewGuid(),                           
                Generation = 0,
                VykazId = Guid.NewGuid(),
            };              
                var item = Create(ev);
                db.Vykazy.Add(item);
                await db.SaveChangesAsync();                
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.VykazId);
            
        }
        public async Task Update(CommandVykazUpdate cmd)
        {
            var item = db.Vykazy.FirstOrDefault(u => u.VykazId == cmd.VykazId);                   
            if (item != null) {
                var ev = new EventVykazUpdated()
                {
                    EventId = Guid.NewGuid(),
                    VykazValue1 = cmd.VykazValue1,
                    VykazValue2 = cmd.VykazValue2,

                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.VykazUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.VykazId);
                db.Vykazy.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandVykazRemove cmd)
        {
            var remove = db.Vykazy.FirstOrDefault(u => u.VykazId == cmd.VykazId);
            if (remove != null) {
                
                var ev = new EventVykazDeleted()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    VykazId = cmd.VykazId,
                };
                db.Vykazy.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.VykazRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.VykazId);
                await db.SaveChangesAsync();
            }

        }


    }

}








