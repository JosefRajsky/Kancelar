
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Aktivita_Api.Repositories
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
            var item = db.Aktivity.FirstOrDefault(u => u.AktivitaId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>
            {
                MessageType.AktivitaCreated,
                MessageType.AktivitaUpdated,
                MessageType.AktivitaRemoved
            };
            await _handler.RequestReplay("Aktivitalate.ex", entityId, msgTypes);           
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
                        var create = JsonConvert.DeserializeObject<EventAktivitaCreated>(msg.Event);
                        var forCreate = db.Aktivity.FirstOrDefault(u => u.AktivitaId == create.AktivitaId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Aktivity.Add(forCreate);
                            db.SaveChanges();
                        }
                        
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventAktivitaDeleted>(msg.Event);
                        var forRemove = db.Aktivity.FirstOrDefault(u => u.AktivitaId == remove.AktivitaId);
                        if (forRemove != null) db.Aktivity.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventAktivitaUpdated>(msg.Event);
                        var forUpdate = db.Aktivity.FirstOrDefault(u => u.AktivitaId == update.AktivitaId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update,forUpdate);
                            db.Aktivity.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Aktivita Create(EventAktivitaCreated evt)
        {
            var model = new Aktivita()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                AktivitaId = evt.AktivitaId,
                Value1 = evt.AktivitaValue1,
                Value2 = evt.AktivitaValue2
            };
            return model;
        }
        private Aktivita Modify(EventAktivitaUpdated evt, Aktivita item)
        {           
            item.EventGuid = evt.EventId;
            item.Value1 = evt.AktivitaValue1;
            item.Value2 = evt.AktivitaValue2;
            return item;
        }

        public async Task<Aktivita> Get(Guid id) => await Task.Run(() => db.Aktivity.FirstOrDefault(b => b.AktivitaId == id));
        public async Task<List<Aktivita>> GetList() => await db.Aktivity.ToListAsync();
        public async Task Add(CommandAktivitaCreate cmd)
        {
            var ev = new EventAktivitaCreated()
            {
                EventId = Guid.NewGuid(),                           
                Generation = 0,
                AktivitaId = Guid.NewGuid(),
            };              
                var item = Create(ev);
                db.Aktivity.Add(item);
                await db.SaveChangesAsync();                
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.AktivitaId);
            
        }
        public async Task Update(CommandAktivitaUpdate cmd)
        {
            var item = db.Aktivity.FirstOrDefault(u => u.AktivitaId == cmd.AktivitaId);                   
            if (item != null) {
                var ev = new EventAktivitaUpdated()
                {
                    EventId = Guid.NewGuid(),
                    AktivitaValue1 = cmd.AktivitaValue1,
                    AktivitaValue2 = cmd.AktivitaValue2,

                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.AktivitaUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.AktivitaId);
                db.Aktivity.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandAktivitaRemove cmd)
        {
            var remove = db.Aktivity.FirstOrDefault(u => u.AktivitaId == cmd.AktivitaId);
            if (remove != null) {
                
                var ev = new EventAktivitaDeleted()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    AktivitaId = cmd.AktivitaId,
                };
                db.Aktivity.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.AktivitaRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.AktivitaId);
                await db.SaveChangesAsync();
            }

        }


    }

}








