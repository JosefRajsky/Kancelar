
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Struktura_Api.Repositories
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
            var item = db.Struktury.FirstOrDefault(u => u.StrukturaId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.StrukturaCreated);
            msgTypes.Add(MessageType.StrukturaUpdated);
            msgTypes.Add(MessageType.StrukturaRemoved);
            await _handler.RequestReplay("Strukturalate.ex", entityId, msgTypes);           
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
                        var create = JsonConvert.DeserializeObject<EventStrukturaCreated>(msg.Event);
                        var forCreate = db.Struktury.FirstOrDefault(u => u.StrukturaId == create.StrukturaId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Struktury.Add(forCreate);
                            db.SaveChanges();
                        }
                        
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventStrukturaDeleted>(msg.Event);
                        var forRemove = db.Struktury.FirstOrDefault(u => u.StrukturaId == remove.StrukturaId);
                        if (forRemove != null) db.Struktury.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventStrukturaUpdated>(msg.Event);
                        var forUpdate = db.Struktury.FirstOrDefault(u => u.StrukturaId == update.StrukturaId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update,forUpdate);
                            db.Struktury.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Struktura Create(EventStrukturaCreated evt)
        {
            var model = new Struktura()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                StrukturaId = evt.StrukturaId,
                Value1 = evt.StrukturaValue1,
                Value2 = evt.StrukturaValue2
            };
            return model;
        }
        private Struktura Modify(EventStrukturaUpdated evt, Struktura item)
        {           
            item.EventGuid = evt.EventId;
            item.Value1 = evt.StrukturaValue1;
            item.Value2 = evt.StrukturaValue2;
            return item;
        }

        public async Task<Struktura> Get(Guid id) => await Task.Run(() => db.Struktury.FirstOrDefault(b => b.StrukturaId == id));
        public async Task<List<Struktura>> GetList() => await db.Struktury.ToListAsync();
        public async Task Add(CommandStrukturaCreate cmd)
        {
            var ev = new EventStrukturaCreated()
            {
                EventId = Guid.NewGuid(),                           
                Generation = 0,
                StrukturaId = Guid.NewGuid(),
            };              
                var item = Create(ev);
                db.Struktury.Add(item);
                await db.SaveChangesAsync();                
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.StrukturaId);
            
        }
        public async Task Update(CommandStrukturaUpdate cmd)
        {
            var item = db.Struktury.FirstOrDefault(u => u.StrukturaId == cmd.StrukturaId);                   
            if (item != null) {
                var ev = new EventStrukturaUpdated()
                {
                    EventId = Guid.NewGuid(),
                    StrukturaValue1 = cmd.StrukturaValue1,
                    StrukturaValue2 = cmd.StrukturaValue2,

                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.StrukturaUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.StrukturaId);
                db.Struktury.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandStrukturaRemove cmd)
        {
            var remove = db.Struktury.FirstOrDefault(u => u.StrukturaId == cmd.StrukturaId);
            if (remove != null) {
                
                var ev = new EventStrukturaDeleted()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    StrukturaId = cmd.StrukturaId,
                };
                db.Struktury.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.StrukturaRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.StrukturaId);
                await db.SaveChangesAsync();
            }

        }


    }

}








