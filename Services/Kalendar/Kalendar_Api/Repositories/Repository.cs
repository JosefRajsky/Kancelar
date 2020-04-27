
using CommandHandler;


using Kalendar_Api;
using Kalendar_Api.Functions;
using Kalendar_Api.Models;
using Kalendar_Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Kalendar_Api.Repositories
{
    public class Repository : IRepository
    {
        private readonly ServiceDbContext db;
        private Publisher _publisher;
        private MessageHandler _handler;
        public Repository(ServiceDbContext dbContext, Publisher publisher)
        {
            db = dbContext;
            _publisher = publisher;
            _handler = new MessageHandler(publisher);
        }
        public async Task LastEventCheck(Guid eventId, Guid entityId)
        {
            var item = db.Kalendare.FirstOrDefault(u => u.KalendarId == entityId);
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
            msgTypes.Add(MessageType.UzivatelCreated);
            msgTypes.Add(MessageType.UzivatelUpdated);
            msgTypes.Add(MessageType.UzivatelRemoved);
            msgTypes.Add(MessageType.AktivitaCreated);
            msgTypes.Add(MessageType.AktivitaUpdated);
            msgTypes.Add(MessageType.AktivitaRemoved);
            await _handler.RequestReplay("kalendar.ex", entityId, msgTypes);
        }

        private async Task<Kalendar> Create(EventUzivatelCreated evt)
        {
            var body = JsonConvert.SerializeObject(await new KalendarGenerator().KalendarNew());
            var model = new Kalendar()
            {
                KalendarId = Guid.NewGuid(),
                UzivatelId = evt.UzivatelId,
                Rok = evt.EventCreated.Year,
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                UzivatelCeleJmeno = $"{evt.Prijmeni} {evt.Jmeno}",
                Body = body,
                DatumAktualizace = DateTime.Now
            };
            return model;
        }
        private Kalendar Modify(EventUzivatelUpdated evt, Kalendar item)
        {
            item.UzivatelId = evt.UzivatelId;
            item.Rok = evt.EventCreated.Year;
            item.Generation = evt.Generation;
            item.EventGuid = evt.EventId;
            item.UzivatelCeleJmeno = $"{evt.Prijmeni} {evt.Jmeno}";
            item.DatumAktualizace = DateTime.Now;
            return item;
        }
        public async Task CreateByAktivita(EventAktivitaCreated evt)
        {
            var interval = (evt.DatumDo - evt.DatumOd).TotalDays;
            var _tmpYear = evt.DatumOd.Year;
            var item = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == _tmpYear);
            var kalendar = JsonConvert.DeserializeObject<Year>(item.Body);
            for (int i = 0; i <= interval; i++)
            {
                var focus = evt.DatumOd.AddDays(i);
                if (_tmpYear != focus.Year)
                {
                    _tmpYear = focus.Year;
                    item = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == _tmpYear);
                    kalendar = JsonConvert.DeserializeObject<Year>(item.Body);
                }
                var mesic = kalendar.Months[focus.Month - 1];
                var den = mesic.Days[focus.Day - 1];
                if (!den.Polozky.Where(p => p.Id == evt.AktivitaId).Any())
                {
                    var polozka = new Polozka()
                    {
                        Id = evt.AktivitaId,
                        DatumDo = evt.DatumOd,
                        AktivitaTypId = evt.AktivitaTypId,
                        DatumOd = evt.DatumDo,
                        Nazev = evt.Nazev,
                        UzivatelId = evt.UzivatelId,
                        CeleJmeno = evt.UzivatelCeleJmeno
                    };
                    den.Polozky.Add(polozka);
                    var result = JsonConvert.SerializeObject(kalendar);
                    item.DatumAktualizace = evt.EventCreated;
                    item.Body = result;
                    item.EventGuid = evt.EventId;
                    item.Generation = evt.Generation;
                    db.Kalendare.Update(item);

                    var ev = new EventKalendarUpdated()
                    {
                        CeleJmeno = item.UzivatelCeleJmeno,
                        EventCreated = DateTime.Now,
                        EventId = Guid.NewGuid(),
                        Generation = item.Generation + 1,
                        SourceGuid = evt.EventId,
                        UzivatelId = evt.UzivatelId,
                        Body = item.Body,
                    };
                    await _handler.PublishEvent(ev, MessageType.KalendarUpdated, ev.EventId, null, ev.Generation, item.KalendarId);
                    await db.SaveChangesAsync();

                }
            }
        }
        public async Task UpdateByAktivita(EventAktivitaUpdated evt)
        {
            var interval = (evt.DatumDo - evt.DatumOd).TotalDays;
            var _tmpYear = evt.DatumOd.Year;
            var item = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == _tmpYear);
            if (item != null)
            {
                var kalendar = JsonConvert.DeserializeObject<Year>(item.Body);
                for (int i = 0; i <= interval; i++)
                {
                    var focus = evt.DatumOd.AddDays(i);
                    if (_tmpYear != focus.Year)
                    {
                        _tmpYear = focus.Year;
                        item = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == _tmpYear);
                        kalendar = JsonConvert.DeserializeObject<Year>(item.Body);
                    }
                    var mesic = kalendar.Months[focus.Month - 1];
                    var den = mesic.Days[focus.Day - 1];
                    var polozka = den.Polozky.Where(p => p.Id == evt.AktivitaId).FirstOrDefault();
                    if (polozka != null)
                    {
                        polozka.Id = evt.AktivitaId;
                        polozka.DatumDo = evt.DatumOd;
                        polozka.AktivitaTypId = evt.AktivitaTypId;
                        polozka.DatumOd = evt.DatumDo;
                        polozka.Nazev = evt.Nazev;
                        polozka.UzivatelId = evt.UzivatelId;
                        polozka.CeleJmeno = evt.UzivatelCeleJmeno;
                        var result = JsonConvert.SerializeObject(kalendar);
                        item.DatumAktualizace = evt.EventCreated;
                        item.Body = result;
                        item.EventGuid = evt.EventId;
                        item.Generation = evt.Generation;
                        db.Kalendare.Update(item);
                        await db.SaveChangesAsync();
                        var ev = new EventKalendarUpdated()
                        {
                            CeleJmeno = item.UzivatelCeleJmeno,
                            EventCreated = DateTime.Now,
                            EventId = Guid.NewGuid(),
                            Generation = item.Generation + 1,
                            SourceGuid = evt.EventId,
                            UzivatelId = evt.UzivatelId,
                            Body = item.Body,
                        };
                        await _handler.PublishEvent(ev, MessageType.KalendarUpdated, ev.EventId, null, ev.Generation, item.KalendarId);
                        await db.SaveChangesAsync();
                    }
                }
            }
        }
        public async Task DeleteByAktivita(EventAktivitaRemoved evt)
        {
            var interval = (evt.DatumDo - evt.DatumOd).TotalDays;
            var _tmpYear = evt.DatumOd.Year;
            var item = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == _tmpYear);
            var kalendar = JsonConvert.DeserializeObject<Year>(item.Body);
            for (int i = 0; i <= interval; i++)
            {
                var focus = evt.DatumOd.AddDays(i);
                if (_tmpYear != focus.Year)
                {
                    _tmpYear = focus.Year;
                    item = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == _tmpYear);
                    kalendar = JsonConvert.DeserializeObject<Year>(item.Body);
                }
                var mesic = kalendar.Months[focus.Month - 1];
                var den = mesic.Days[focus.Day - 1];
                var polozka = den.Polozky.Where(p => p.Id == evt.AktivitaId).FirstOrDefault();
                if (polozka != null)
                {
                    den.Polozky.Remove(polozka);
                    var result = JsonConvert.SerializeObject(kalendar);
                    item.DatumAktualizace = evt.EventCreated;
                    item.Body = result;
                    item.EventGuid = evt.EventId;
                    item.Generation = evt.Generation;
                    db.Kalendare.Update(item);
                    await db.SaveChangesAsync();
                    var ev = new EventKalendarUpdated()
                    {
                        CeleJmeno = item.UzivatelCeleJmeno,
                        EventCreated = DateTime.Now,
                        EventId = Guid.NewGuid(),
                        Generation = item.Generation + 1,
                        SourceGuid = evt.EventId,
                        UzivatelId = evt.UzivatelId,
                        Body = item.Body,
                    };
                    await _handler.PublishEvent(ev, MessageType.KalendarUpdated, ev.EventId, null, ev.Generation, item.KalendarId);
                }
            }
            await db.SaveChangesAsync();
        }
        public async Task CreateByUzivatel(EventUzivatelCreated evt)
        {
            var item = db.Kalendare.Where(k => k.UzivatelId == evt.UzivatelId && k.Rok == evt.EventCreated.Year).FirstOrDefault();
            if (item == null)
            {
                item = await Create(evt);
                db.Kalendare.Add(item);
                await db.SaveChangesAsync();

                var ev = new EventKalendarCreated()
                {
                    CeleJmeno = $"{evt.Prijmeni} {evt.Jmeno}",
                    EventCreated = DateTime.Now,
                    EventId = Guid.NewGuid(),
                    Generation = 0,
                    Rok = evt.EventCreated.Year,
                    SourceGuid = evt.EventId,
                    UzivatelId = evt.UzivatelId,
                };
                await _handler.PublishEvent(ev, MessageType.KalendarCreated, ev.EventId, null, ev.Generation, item.KalendarId);
            }
        }
        public async Task UpdateByUzivatel(EventUzivatelUpdated evt)
        {
            var kalendarList = db.Kalendare.Where(k => k.UzivatelId == evt.UzivatelId);
            if (kalendarList.Any())
            {
                foreach (var item in kalendarList)
                {
                    var ev = new EventKalendarUpdated()
                    {
                        CeleJmeno = $"{evt.Prijmeni} {evt.Jmeno}",
                        EventCreated = DateTime.Now,
                        EventId = Guid.NewGuid(),
                        Generation = item.Generation + 1,
                        SourceGuid = evt.EventId,
                        UzivatelId = evt.UzivatelId,
                        Body = item.Body,
                    };
                    await _handler.PublishEvent(ev, MessageType.KalendarCreated, ev.EventId, null, ev.Generation, item.KalendarId);

                    item.UzivatelId = ev.UzivatelId;                   
                    item.EventGuid = ev.EventId;
                    item.UzivatelCeleJmeno =ev.CeleJmeno;
                    item.DatumAktualizace = DateTime.Now;
                    db.Kalendare.Update(item);                
                }             
            }
            await db.SaveChangesAsync();

        }
        public async Task DeleteByUzivatel(EventUzivatelRemoved evt)
        {
            var kalendarList = db.Kalendare.Where(k => k.UzivatelId == evt.UzivatelId);
            if (kalendarList.Any())
            {
                foreach (var item in kalendarList)
                {
                    var ev = new EventKalendarRemoved()
                    {
                        EventCreated = DateTime.Now,
                        EventId = Guid.NewGuid(),
                        Generation = item.Generation + 1,
                        SourceGuid = evt.EventId,
                        UzivatelId = evt.UzivatelId,
                    };
                    await _handler.PublishEvent(ev, MessageType.KalendarRemoved, ev.EventId, null, ev.Generation, item.KalendarId);                                       
                    db.Kalendare.Remove(item);
                }
            }
            await db.SaveChangesAsync();
        }
        public async Task<List<Kalendar>> GetList()
        {
            return await db.Kalendare.ToListAsync();
        }
        public async Task<Kalendar> Get(Guid id) => await Task.Run(() => db.Kalendare.FirstOrDefault(b => b.KalendarId == id));


    }
}
