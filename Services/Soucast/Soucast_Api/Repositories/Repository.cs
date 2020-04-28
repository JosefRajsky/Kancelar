
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Soucast_Api.Repositories
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
            var item = db.Soucasti.FirstOrDefault(u => u.SoucastId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.SoucastCreated);
            msgTypes.Add(MessageType.SoucastUpdated);
            msgTypes.Add(MessageType.SoucastRemoved);
            await _handler.RequestReplay("Soucastlate.ex", entityId, msgTypes);
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
                        var create = JsonConvert.DeserializeObject<EventSoucastCreated>(msg.Event);
                        var forCreate = db.Soucasti.FirstOrDefault(u => u.SoucastId == create.SoucastId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Soucasti.Add(forCreate);
                            db.SaveChanges();
                        }

                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventSoucastRemoved>(msg.Event);
                        var forRemove = db.Soucasti.FirstOrDefault(u => u.SoucastId == remove.SoucastId);
                        if (forRemove != null) db.Soucasti.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventSoucastUpdated>(msg.Event);
                        var forUpdate = db.Soucasti.FirstOrDefault(u => u.SoucastId == update.SoucastId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update, forUpdate);
                            db.Soucasti.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Soucast Create(EventSoucastCreated evt)
        {
            var model = new Soucast()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                SoucastId = evt.SoucastId,
                ImportedId = evt.ImportedId,
                ParentId = evt.ParentId,
                Nazev = evt.Nazev,
                Zkratka = evt.Zkratka
            };
            return model;
        }
        private Soucast Modify(EventSoucastUpdated evt, Soucast item)
        {
            item.EventGuid = evt.EventId;
            item.Generation = evt.Generation;
            item.EventGuid = evt.EventId;
            item.SoucastId = evt.SoucastId;
            item.ImportedId = evt.ImportedId;
            item.ParentId = evt.ParentId;
            item.Nazev = evt.Nazev;
            item.Zkratka = evt.Zkratka;
            return item;
        }

        public async Task<Soucast> Get(Guid id) => await Task.Run(() => db.Soucasti.FirstOrDefault(b => b.SoucastId == id));
        public async Task<List<Soucast>> GetList() => await db.Soucasti.ToListAsync();
        public async Task Add(CommandSoucastCreate cmd)
        {
            var ev = new EventSoucastCreated()
            {
                EventId = Guid.NewGuid(),
                Generation = 0,
                SoucastId = Guid.NewGuid(),
                ImportedId = cmd.ImportedId,
                ParentId = cmd.ParentId,
                Nazev = cmd.Nazev,
                Zkratka = cmd.Zkratka
            };
            var item = Create(ev);
            db.Soucasti.Add(item);
            await db.SaveChangesAsync();
            await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.SoucastId);

        }
        public async Task Update(CommandSoucastUpdate cmd)
        {
            var item = db.Soucasti.FirstOrDefault(u => u.SoucastId == cmd.SoucastId);
            if (item != null)
            {
                var ev = new EventSoucastUpdated()
                {
                    EventId = Guid.NewGuid(),
                    Zkratka = cmd.Zkratka,
                    Nazev = cmd.Nazev,
                    ParentId = cmd.ParentId,
                    ImportedId = cmd.ImportedId,
                    SoucastId = cmd.SoucastId

                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.SoucastUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.SoucastId);
                db.Soucasti.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandSoucastRemove cmd)
        {
            var remove = db.Soucasti.FirstOrDefault(u => u.SoucastId == cmd.SoucastId);
            if (remove != null)
            {

                var ev = new EventSoucastRemoved()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    SoucastId = cmd.SoucastId,
                };
                db.Soucasti.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.SoucastRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.SoucastId);
                await db.SaveChangesAsync();
            }

        }


    }

}








