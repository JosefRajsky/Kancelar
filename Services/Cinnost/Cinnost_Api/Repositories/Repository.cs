
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Cinnost_Api.Repositories
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
            var item = db.Cinnosti.FirstOrDefault(u => u.CinnostId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>
            {
                MessageType.CinnostCreated,
                MessageType.CinnostUpdated,
                MessageType.CinnostRemoved
            };
            await _handler.RequestReplay("Cinnostlate.ex", entityId, msgTypes);           
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
                        var create = JsonConvert.DeserializeObject<EventCinnostCreated>(msg.Event);
                        var forCreate = db.Cinnosti.FirstOrDefault(u => u.CinnostId == create.CinnostId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Cinnosti.Add(forCreate);
                            db.SaveChanges();
                        }
                        
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventCinnostDeleted>(msg.Event);
                        var forRemove = db.Cinnosti.FirstOrDefault(u => u.CinnostId == remove.CinnostId);
                        if (forRemove != null) db.Cinnosti.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventCinnostUpdated>(msg.Event);
                        var forUpdate = db.Cinnosti.FirstOrDefault(u => u.CinnostId == update.CinnostId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update,forUpdate);
                            db.Cinnosti.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Cinnost Create(EventCinnostCreated evt)
        {
            var model = new Cinnost()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                CinnostId = evt.CinnostId,
                Value1 = evt.CinnostValue1,
                Value2 = evt.CinnostValue2
            };
            return model;
        }
        private Cinnost Modify(EventCinnostUpdated evt, Cinnost item)
        {           
            item.EventGuid = evt.EventId;
            item.Value1 = evt.CinnostValue1;
            item.Value2 = evt.CinnostValue2;
            return item;
        }

        public async Task<Cinnost> Get(Guid id) => await Task.Run(() => db.Cinnosti.FirstOrDefault(b => b.CinnostId == id));
        public async Task<List<Cinnost>> GetList() => await db.Cinnosti.ToListAsync();
        public async Task Add(CommandCinnostCreate cmd)
        {
            var ev = new EventCinnostCreated()
            {
                EventId = Guid.NewGuid(),                           
                Generation = 0,
                CinnostId = Guid.NewGuid(),
            };              
                var item = Create(ev);
                db.Cinnosti.Add(item);
                await db.SaveChangesAsync();                
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.CinnostId);
            
        }
        public async Task Update(CommandCinnostUpdate cmd)
        {
            var item = db.Cinnosti.FirstOrDefault(u => u.CinnostId == cmd.CinnostId);                   
            if (item != null) {
                var ev = new EventCinnostUpdated()
                {
                    EventId = Guid.NewGuid(),
                    CinnostValue1 = cmd.CinnostValue1,
                    CinnostValue2 = cmd.CinnostValue2,

                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.CinnostUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.CinnostId);
                db.Cinnosti.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandCinnostRemove cmd)
        {
            var remove = db.Cinnosti.FirstOrDefault(u => u.CinnostId == cmd.CinnostId);
            if (remove != null) {
                
                var ev = new EventCinnostDeleted()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    CinnostId = cmd.CinnostId,
                };
                db.Cinnosti.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.CinnostRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.CinnostId);
                await db.SaveChangesAsync();
            }

        }


    }

}








