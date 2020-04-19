
using CommandHandler;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Api.Entities;

namespace Udalost_Api.Repositories
{
    public class Repository : IRepository
    {
        private readonly UdalostDbContext db;
        private MessageHandler _handler;
        public Repository(UdalostDbContext dbContext, Publisher publisher)
        {
            db = dbContext;
            _handler = new MessageHandler(publisher);
        }
        public async Task LastEventCheck(Guid eventId, Guid entityId)
        {
            var item = db.Udalosti.FirstOrDefault(u => u.UdalostId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId)
                {
                    await RequestEvents(entityId);
                }
                else
                {
                    item.Generation = item.Generation + 1;
                    await db.SaveChangesAsync();
                }
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.UdalostCreated);
            msgTypes.Add(MessageType.UdalostUpdated);
            msgTypes.Add(MessageType.UdalostRemoved);
            await _handler.RequestReplay("udalost.ex", entityId, msgTypes);
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
                    case MessageType.UdalostCreated:
                        var create = JsonConvert.DeserializeObject<EventUdalostCreated>(msg.Event);
                        var forCreate = db.Udalosti.FirstOrDefault(u => u.UdalostId == create.UdalostId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Udalosti.Add(forCreate);
                            db.SaveChanges();
                        }
                        break;
                    case MessageType.UdalostRemoved:
                        var remove = JsonConvert.DeserializeObject<EventUdalostRemoved>(msg.Event);
                        var forRemove = db.Udalosti.FirstOrDefault(u => u.UdalostId == remove.UdalostId);
                        if (forRemove != null) db.Udalosti.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventUdalostUpdated>(msg.Event);
                        var forUpdate = db.Udalosti.FirstOrDefault(u => u.UdalostId == update.UdalostId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update, forUpdate);
                            db.Udalosti.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        public async Task<Udalost> Get(Guid id) => await Task.Run(() => db.Udalosti.FirstOrDefault(b => b.Id == id));
        public async Task<List<Udalost>> GetList() => await db.Udalosti.ToListAsync();
        private Udalost Create(EventUdalostCreated evt)
        {
            var model = new Udalost()
            {
                UdalostId = Guid.NewGuid(),
                DatumOd = evt.DatumOd,
                DatumDo = evt.DatumDo,
                DatumZadal = evt.EventCreated,
                UzivatelCeleJmeno = evt.UzivatelCeleJmeno,
                Nazev = evt.Nazev,
                Popis = evt.Popis,
                UdalostTypId = evt.UdalostTypId,
                UzivatelId = evt.UzivatelId,
            };
            return model;
        }
        private Udalost Modify(EventUdalostUpdated evt, Udalost item)
        {
            item.DatumOd = evt.DatumOd;
            item.DatumDo = evt.DatumDo;
            item.DatumZadal = evt.EventCreated;
            item.UzivatelCeleJmeno = evt.UzivatelCeleJmeno;
            item.Nazev = evt.Nazev;
            item.Popis = evt.Popis;
            item.UdalostTypId = evt.UdalostTypId;
            item.UzivatelId = evt.UzivatelId;
            return item;
        }
        public async Task Add(CommandUdalostCreate cmd)
        {
            var ev = new EventUdalostCreated()
            {
                EventId = Guid.NewGuid(),
                UdalostId = Guid.NewGuid(),
                EventCreated = DateTime.Now,
                DatumOd = cmd.DatumOd,
                DatumDo = cmd.DatumDo,
                DatumZadal = cmd.DatumZadal,
                Nazev = cmd.Nazev,
                Popis = cmd.Popis,
                UdalostTypId=cmd.UdalostTypId,
                UzivatelCeleJmeno = cmd.UzivatelCeleJmeno,
                Generation = 0,
            };
            var item = Create(ev);
            db.Udalosti.Add(item);
            await db.SaveChangesAsync();
            ev.UdalostId = item.UdalostId;
            ev.Generation = ev.Generation + 1;
            await _handler.PublishEvent(ev, MessageType.UdalostCreated, ev.EventId, null, ev.Generation, ev.UdalostId);
        }
        public async Task Remove(CommandUdalostRemove cmd)
        {
            var remove = db.Udalosti.Find(cmd.UdalostId);
            db.Udalosti.Remove(remove);           
            var ev = new EventUdalostRemoved()
            {
                Generation = remove.Generation + 1,
                EventId = Guid.NewGuid(),
                UzivatelId = cmd.UdalostId,
            };
            await _handler.PublishEvent(ev, MessageType.UdalostRemoved, ev.EventId, null, ev.Generation, ev.UdalostId);
            await db.SaveChangesAsync();
        }
        public async Task Update(CommandUdalostUpdate cmd)
        {
            var item = db.Udalosti.FirstOrDefault(u => u.UdalostId == cmd.UdalostId);
            if (item != null)
            {
                var ev = new EventUdalostUpdated()
                {
                    EventId = Guid.NewGuid(),
                    UdalostId = cmd.UdalostId,
                    EventCreated = DateTime.Now,
                    DatumOd = cmd.DatumOd,
                    DatumDo = cmd.DatumDo,
                    DatumZadal = cmd.DatumZadal,
                    Nazev = cmd.Nazev,
                    Popis = cmd.Popis,
                    UdalostTypId = cmd.UdalostTypId,
                    UzivatelCeleJmeno = cmd.UzivatelCeleJmeno,
                    Generation = 0,
                };
                item = Modify(ev,item);
                db.Udalosti.Add(item);
                await db.SaveChangesAsync();
                ev.UdalostId = item.UdalostId;
                ev.Generation = ev.Generation + 1;
                await _handler.PublishEvent(ev, MessageType.UdalostUpdated, ev.EventId, null, ev.Generation, ev.UdalostId);
            }
        }



    }
}
