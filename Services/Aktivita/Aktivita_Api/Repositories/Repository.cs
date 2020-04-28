
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
        private MessageHandler _handler;
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
            msgTypes.Add(MessageType.AktivitaCreated);
            msgTypes.Add(MessageType.AktivitaUpdated);
            msgTypes.Add(MessageType.AktivitaRemoved);
            await _handler.RequestReplay("Aktivita.ex", entityId, msgTypes);
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
                    case MessageType.AktivitaCreated:
                        var create = JsonConvert.DeserializeObject<EventAktivitaCreated>(msg.Event);
                        var forCreate = db.Aktivity.FirstOrDefault(u => u.AktivitaId == create.AktivitaId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Aktivity.Add(forCreate);
                            db.SaveChanges();
                        }
                        break;
                    case MessageType.AktivitaRemoved:
                        var remove = JsonConvert.DeserializeObject<EventAktivitaRemoved>(msg.Event);
                        var forRemove = db.Aktivity.FirstOrDefault(u => u.AktivitaId == remove.AktivitaId);
                        if (forRemove != null) db.Aktivity.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventAktivitaUpdated>(msg.Event);
                        var forUpdate = db.Aktivity.FirstOrDefault(u => u.AktivitaId == update.AktivitaId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update, forUpdate);
                            db.Aktivity.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        public async Task<Aktivita> Get(Guid id) => await Task.Run(() => db.Aktivity.FirstOrDefault(b => b.Id == id));
        public async Task<List<Aktivita>> GetList() => await db.Aktivity.ToListAsync();
        private Aktivita Create(EventAktivitaCreated evt)
        {
            var model = new Aktivita()
            {
                AktivitaId = Guid.NewGuid(),
                DatumOd = evt.DatumOd,
                DatumDo = evt.DatumDo,
                DatumZadal = evt.EventCreated,
                UzivatelCeleJmeno = evt.UzivatelCeleJmeno,
                Nazev = evt.Nazev,
                Popis = evt.Popis,
                AktivitaTypId = evt.AktivitaTypId,
                UzivatelId = evt.UzivatelId,
            };
            return model;
        }
        private Aktivita Modify(EventAktivitaUpdated evt, Aktivita item)
        {
            item.DatumOd = evt.DatumOd;
            item.DatumDo = evt.DatumDo;
            item.DatumZadal = evt.EventCreated;
            item.UzivatelCeleJmeno = evt.UzivatelCeleJmeno;
            item.Nazev = evt.Nazev;
            item.Popis = evt.Popis;
            item.AktivitaTypId = evt.AktivitaTypId;
            item.UzivatelId = evt.UzivatelId;
            return item;
        }
        public async Task Add(CommandAktivitaCreate cmd)
        {
            var ev = new EventAktivitaCreated()
            {
                EventId = Guid.NewGuid(),
                AktivitaId = Guid.NewGuid(),
                EventCreated = DateTime.Now,
                DatumOd = cmd.DatumOd,
                DatumDo = cmd.DatumDo,
                DatumZadal = cmd.DatumZadal,
                Nazev = cmd.Nazev,
                Popis = cmd.Popis,
                AktivitaTypId = cmd.AktivitaTypId,
                UzivatelCeleJmeno = cmd.UzivatelCeleJmeno,
                Generation = 0,
            };
            var item = Create(ev);
            db.Aktivity.Add(item);
            await db.SaveChangesAsync();
            ev.AktivitaId = item.AktivitaId;
            ev.Generation = ev.Generation + 1;
            await _handler.PublishEvent(ev, MessageType.AktivitaCreated, ev.EventId, null, ev.Generation, ev.AktivitaId);
        }
        public async Task Remove(CommandAktivitaRemove cmd)
        {
            var remove = db.Aktivity.Find(cmd.AktivitaId);
            db.Aktivity.Remove(remove);
            var ev = new EventAktivitaRemoved()
            {
                Generation = remove.Generation + 1,
                EventId = Guid.NewGuid(),
                UzivatelId = cmd.AktivitaId,
            };
            await _handler.PublishEvent(ev, MessageType.AktivitaRemoved, ev.EventId, null, ev.Generation, ev.AktivitaId);
            await db.SaveChangesAsync();
        }
        public async Task Update(CommandAktivitaUpdate cmd)
        {
            var item = db.Aktivity.FirstOrDefault(u => u.AktivitaId == cmd.AktivitaId);
            if (item != null)
            {
                var ev = new EventAktivitaUpdated()
                {
                    EventId = Guid.NewGuid(),
                    AktivitaId = cmd.AktivitaId,
                    EventCreated = DateTime.Now,
                    DatumOd = cmd.DatumOd,
                    DatumDo = cmd.DatumDo,
                    DatumZadal = cmd.DatumZadal,
                    Nazev = cmd.Nazev,
                    Popis = cmd.Popis,
                    AktivitaTypId = cmd.AktivitaTypId,
                    UzivatelCeleJmeno = cmd.UzivatelCeleJmeno,
                    Generation = 0,
                };
                item = Modify(ev, item);
                db.Aktivity.Add(item);
                await db.SaveChangesAsync();
                ev.AktivitaId = item.AktivitaId;
                ev.Generation = ev.Generation + 1;
                await _handler.PublishEvent(ev, MessageType.AktivitaUpdated, ev.EventId, null, ev.Generation, ev.AktivitaId);
            }
        }



    }

}








