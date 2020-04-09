
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
        private readonly KalendarDbContext db;
        private Publisher _publisher;
        private MessageHandler _handler;
        public Repository(KalendarDbContext dbContext, Publisher publisher)
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
        public async Task CreateByUdalost(EventUdalostCreated evt)
        {
            var interval = (evt.DatumDo - evt.DatumOd).TotalDays;
            var _tmpYear = evt.DatumOd.Year;
            var model = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == _tmpYear);           
            var kalendar = JsonConvert.DeserializeObject<Year>(model.Body);
            for (int i = 0; i <= interval; i++)
            {
                var focus = evt.DatumOd.AddDays(i);
                if (_tmpYear != focus.Year)
                {
                    _tmpYear = focus.Year;
                    model = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == _tmpYear);
                    kalendar = JsonConvert.DeserializeObject<Year>(model.Body);
                }
                var mesic = kalendar.Months[focus.Month - 1];
                var den = mesic.Days[focus.Day - 1];
                if (!den.Polozky.Where(p => p.Id == evt.UdalostId).Any()) {
                    var polozka = new Polozka()
                    {
                        Id = evt.UdalostId,                        
                        DatumDo = evt.DatumOd,
                        UdalostTypId = evt.UdalostTypId,
                        DatumOd = evt.DatumDo,
                        Nazev = evt.Nazev,
                        UzivatelId = evt.UzivatelId,
                        CeleJmeno = evt.UzivatelCeleJmeno
                    };
                    den.Polozky.Add(polozka);
                    var result = JsonConvert.SerializeObject(kalendar);
                    model.DatumAktualizace = evt.EventCreated;
                    model.Body = result;
                    model.EventGuid = evt.EventId;
                    model.Generation = evt.Generation;
                    db.Kalendare.Update(model);
                    await db.SaveChangesAsync(); }
            }
        }
        public async Task UpdateByUdalost(EventUdalostUpdated evt)
        {
            var interval = (evt.DatumDo - evt.DatumOd).TotalDays;
            var _tmpYear = evt.DatumOd.Year;
            var model = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == _tmpYear);
            var kalendar = JsonConvert.DeserializeObject<Year>(model.Body);
            for (int i = 0; i <= interval; i++)
            {
                var focus = evt.DatumOd.AddDays(i);
                if (_tmpYear != focus.Year)
                {
                    _tmpYear = focus.Year;
                    model = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == _tmpYear);
                    kalendar = JsonConvert.DeserializeObject<Year>(model.Body);
                }
                var mesic = kalendar.Months[focus.Month - 1];
                var den = mesic.Days[focus.Day - 1];
                var polozka = den.Polozky.Where(p => p.Id == evt.UdalostId).FirstOrDefault();
                if (polozka !=null)
                {
                    polozka.Id = evt.UdalostId;
                    polozka.DatumDo = evt.DatumOd;
                    polozka.UdalostTypId = evt.UdalostTypId;
                    polozka.DatumOd = evt.DatumDo;
                    polozka.Nazev = evt.Nazev;
                    polozka.UzivatelId = evt.UzivatelId;
                    polozka.CeleJmeno = evt.UzivatelCeleJmeno;
                    var result = JsonConvert.SerializeObject(kalendar);
                    model.DatumAktualizace = evt.EventCreated;
                    model.Body = result;
                    model.EventGuid = evt.EventId;
                    model.Generation = evt.Generation;                    
                    db.Kalendare.Update(model);
                    await db.SaveChangesAsync();
                }
            }
        }
        public async Task DeleteByUdalost(EventUdalostRemoved evt)
        {
            var interval = (evt.DatumDo - evt.DatumOd).TotalDays;
            var _tmpYear = evt.DatumOd.Year;
            var model = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == _tmpYear);
            var kalendar = JsonConvert.DeserializeObject<Year>(model.Body);
            for (int i = 0; i <= interval; i++)
            {
                var focus = evt.DatumOd.AddDays(i);
                if (_tmpYear != focus.Year)
                {
                    _tmpYear = focus.Year;
                    model = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == _tmpYear);
                    kalendar = JsonConvert.DeserializeObject<Year>(model.Body);
                }
                var mesic = kalendar.Months[focus.Month - 1];
                var den = mesic.Days[focus.Day - 1];
                var polozka = den.Polozky.Where(p => p.Id == evt.UdalostId).FirstOrDefault();
                if (polozka != null)
                {
                    den.Polozky.Remove(polozka);
                    var result = JsonConvert.SerializeObject(kalendar);
                    model.DatumAktualizace = evt.EventCreated;
                    model.Body = result;
                    model.EventGuid = evt.EventId;
                    model.Generation = evt.Generation;
                    db.Kalendare.Update(model);
                    await db.SaveChangesAsync();
                }
            }
            await db.SaveChangesAsync();

        }
        public async Task CreateByUzivatel(EventUzivatelCreated evt)
        {
            var kalendar = db.Kalendare.Where(k => k.UzivatelId == evt.UzivatelId && k.Rok == evt.EventCreated.Year).FirstOrDefault();
            if (kalendar == null)
            {
                kalendar = await Create(evt);
                db.Kalendare.Add(kalendar);
                await db.SaveChangesAsync();
            }
        }
        public async Task UpdateByUzivatel(EventUzivatelUpdated evt)
        {
            var kalendarList = db.Kalendare.Where(k => k.UzivatelId == evt.UzivatelId);
            if (kalendarList.Any())
            {
                foreach (var kalendar in kalendarList)
                {
                    kalendar.UzivatelId = evt.UzivatelId;
                    kalendar.Rok = evt.EventCreated.Year;
                    kalendar.Generation = evt.Generation;
                    kalendar.EventGuid = evt.EventId;
                    kalendar.UzivatelCeleJmeno = $"{evt.Prijmeni} {evt.Jmeno}";
                    kalendar.DatumAktualizace = DateTime.Now;
                    db.Kalendare.Update(kalendar);
                }
            }
            await db.SaveChangesAsync();

        }
        public async Task DeleteByUzivatel(EventUzivatelDeleted evt)
        {
            var forRemove = db.Kalendare.Where(k => k.UzivatelId == evt.UzivatelId);
            db.Kalendare.RemoveRange(forRemove);
            await db.SaveChangesAsync();
        }
        public async Task<List<Kalendar>> GetList()
        {
            return await db.Kalendare.ToListAsync();
        }
        public async Task<Kalendar> Get(Guid id) => await Task.Run(() => db.Kalendare.FirstOrDefault(b => b.KalendarId == id));


    }
}
