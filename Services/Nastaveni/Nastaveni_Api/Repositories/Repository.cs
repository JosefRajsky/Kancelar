
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nastaveni_Api.Repositories
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
            var item = db.Nastaveni.FirstOrDefault(u => u.PravidloId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>
            {
                MessageType.NastaveniCreated,
                MessageType.NastaveniUpdated,
                MessageType.NastaveniRemoved
            };
            await _handler.RequestReplay("nastaveni.ex", entityId, msgTypes);           
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
                        var create = JsonConvert.DeserializeObject<EventNastaveniCreated>(msg.Event);
                        var forCreate = db.Nastaveni.FirstOrDefault(u => u.PravidloId == create.NastaveniId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Nastaveni.Add(forCreate);
                            db.SaveChanges();
                        }
                        
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventNastaveniDeleted>(msg.Event);
                        var forRemove = db.Nastaveni.FirstOrDefault(u => u.PravidloId == remove.NastaveniId);
                        if (forRemove != null) db.Nastaveni.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventNastaveniUpdated>(msg.Event);
                        var forUpdate = db.Nastaveni.FirstOrDefault(u => u.PravidloId == update.NastaveniId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update,forUpdate);
                            db.Nastaveni.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Pravidlo Create(EventNastaveniCreated evt)
        {
            var model = new Pravidlo()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                PravidloId = evt.NastaveniId,
                Value1 = evt.NastaveniValue1,
                Value2 = evt.NastaveniValue2
            };
            return model;
        }
        private Pravidlo Modify(EventNastaveniUpdated evt, Pravidlo item)
        {           
            item.EventGuid = evt.EventId;
            item.Value1 = evt.NastaveniValue1;
            item.Value2 = evt.NastaveniValue2;
            return item;
        }
        public async Task<Pravidlo> Get(Guid id) => await Task.Run(() => db.Nastaveni.FirstOrDefault(b => b.PravidloId == id));
        public async Task<List<Pravidlo>> GetList() => await db.Nastaveni.ToListAsync();
        public async Task Add(CommandNastaveniCreate cmd)
        {
            var ev = new EventNastaveniCreated()
            {
                EventId = Guid.NewGuid(),                           
                Generation = 0,
                NastaveniId = Guid.NewGuid(),
            };              
                var item = Create(ev);
                db.Nastaveni.Add(item);
                await db.SaveChangesAsync();                
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.PravidloId);
            
        }
        public async Task Update(CommandNastaveniUpdate cmd)
        {
            var item = db.Nastaveni.FirstOrDefault(u => u.PravidloId == cmd.NastaveniId);                   
            if (item != null) {
                var ev = new EventNastaveniUpdated()
                {
                    EventId = Guid.NewGuid(),
                    NastaveniValue1 = cmd.NastaveniValue1,
                    NastaveniValue2 = cmd.NastaveniValue2,

                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.NastaveniUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.NastaveniId);
                db.Nastaveni.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandNastaveniRemove cmd)
        {
            var remove = db.Nastaveni.FirstOrDefault(u => u.PravidloId == cmd.NastaveniId);
            if (remove != null) {
                
                var ev = new EventNastaveniDeleted()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    NastaveniId = cmd.NastaveniId,
                };
                db.Nastaveni.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.NastaveniRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.PravidloId);
                await db.SaveChangesAsync();
            }

        }


    }

}








