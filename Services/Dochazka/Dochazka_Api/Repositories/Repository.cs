
using CommandHandler;
using Dochazka_Api.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Serialization.NodeDeserializers;

namespace Dochazka_Api.Repositories
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
            var item = db.Dochazka.FirstOrDefault(u => u.DochazkaId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>
            {
                MessageType.DochazkaCreated,
                MessageType.DochazkaUpdated,
                MessageType.DochazkaDeleted
            };
            await _handler.RequestReplay("dochazka.ex", entityId, msgTypes);
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
                    case MessageType.DochazkaCreated:
                        var create = JsonConvert.DeserializeObject<EventDochazkaCreated>(msg.Event);
                        var forCreate = db.Dochazka.FirstOrDefault(u => u.DochazkaId == create.DochazkaId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Dochazka.Add(forCreate);
                            db.SaveChanges();
                        }

                        break;
                    case MessageType.DochazkaDeleted:
                        var remove = JsonConvert.DeserializeObject<EventDochazkaDeleted>(msg.Event);
                        var forRemove = db.Dochazka.FirstOrDefault(u => u.DochazkaId == remove.DochazkaId);
                        if (forRemove != null) db.Dochazka.Remove(forRemove);

                        break;
                    case MessageType.DochazkaUpdated:
                        var update = JsonConvert.DeserializeObject<EventDochazkaUpdated>(msg.Event);
                        var forUpdate = db.Dochazka.FirstOrDefault(u => u.DochazkaId == update.DochazkaId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update, forUpdate);
                            db.Dochazka.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Dochazka Create(EventDochazkaCreated evt)
        {
            var model = new Dochazka()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                DochazkaId = evt.DochazkaId,
                UzivatelId = evt.UzivatelId,
                Rok = evt.Datum.Year,
                Den = evt.Datum.Day,
                Mesic = evt.Datum.Month,
                DenTydne = Convert.ToInt32(evt.Datum.DayOfWeek),
                Tick = evt.Datum.Ticks,
                Prichod = evt.Prichod,
                CteckaId = evt.CteckaId,
                Datum = evt.Datum,
            };
            return model;
        }
        private Dochazka Modify(EventDochazkaUpdated evt, Dochazka item)
        {
            item.EventGuid = evt.EventId;
            item.UzivatelId = evt.UzivatelId;
            item.Rok = evt.Datum.Year;
            item.Den = evt.Datum.Day;
            item.Mesic = evt.Datum.Month;
            item.DenTydne = Convert.ToInt32(evt.Datum.DayOfWeek);
            item.Tick = evt.Datum.Ticks;
            item.Prichod = evt.Prichod;
            item.CteckaId = evt.CteckaId;
            item.Datum = evt.Datum;
            return item;
        }
        public async Task<Dochazka> Get(Guid id) => await Task.Run(() => db.Dochazka.FirstOrDefault(b => b.Id == id));
        public async Task<List<Dochazka>> GetList() => await db.Dochazka.ToListAsync();
      
        public async Task Add(CommandDochazkaCreate cmd)
        {
            var ev = new EventDochazkaCreated()
            {
                EventId = Guid.NewGuid(),
                Generation = 0,
                DochazkaId = Guid.NewGuid(),
                CteckaId = cmd.CteckaId,
                Datum = cmd.Datum,
                EventCreated = DateTime.Now,
                Prichod = cmd.Prichod,
                UzivatelId = cmd.UzivatelId
            };
            var item = Create(ev);
            db.Dochazka.Add(item);
            await db.SaveChangesAsync();
            await _handler.PublishEvent(ev, MessageType.DochazkaCreated, ev.EventId, null, ev.Generation, item.DochazkaId);
        }
        public async Task Update(CommandDochazkaUpdate cmd)
        {
            var item = db.Dochazka.FirstOrDefault(u => u.DochazkaId == cmd.DochazkaId);
            if (item != null)
            {
                var ev = new EventDochazkaUpdated()
                {
                    EventId = Guid.NewGuid(),
                    UzivatelId = cmd.UzivatelId,
                    Prichod = cmd.Prichod,
                    EventCreated = DateTime.Now,
                    Datum = cmd.Datum,
                    CteckaId = cmd.CteckaId,
                    DochazkaId = cmd.DochazkaId,
                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.DochazkaUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.DochazkaId);
                db.Dochazka.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandDochazkaRemove cmd)
        {
            var remove = db.Dochazka.FirstOrDefault(u => u.DochazkaId == cmd.DochazkaId);
            if (remove != null)
            {
                var ev = new EventDochazkaDeleted()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    DochazkaId = cmd.DochazkaId,
                };
                db.Dochazka.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.DochazkaDeleted, ev.EventId, remove.EventGuid, remove.Generation, remove.DochazkaId);
                await db.SaveChangesAsync();
            }

        }


    }
}
