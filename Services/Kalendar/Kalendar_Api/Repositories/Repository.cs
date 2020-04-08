
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
        public Repository(KalendarDbContext dbContext, Publisher publisher) {
            db = dbContext;
            _publisher = publisher;
            _handler = new MessageHandler(publisher);
        }
        public async Task LastEventCheck<T>(T msg, Guid entityId)
        {
         

            if (typeof(EventUzivatelCreated).IsAssignableFrom(typeof(T))){
                var evt = msg as EventUzivatelCreated;
                var item = db.Kalendare.FirstOrDefault(u => u.UzivatelId == evt.UzivatelId);
                if (item != null)
                {
                    if (item.EventGuid != evt.EventId) await RequestEvents(entityId);
                }
                else {
                    await CreateByUzivatel(evt);
                }
            }
            if (typeof(EventUzivatelUpdated).IsAssignableFrom(typeof(T)))
            {
                var evt = msg as EventUzivatelUpdated;
                var item = db.Kalendare.FirstOrDefault(u => u.UzivatelId == evt.UzivatelId);
                if (item != null)
                {
                    if (item.EventGuid != evt.EventId)
                    {
                        await RequestEvents(entityId);
                    }
                    else {
                        await UpdateByUzivatel(evt);
                    }                    
                }
            }
            if (typeof(EventUzivatelDeleted).IsAssignableFrom(typeof(T)))
            {
                var evt = msg as EventUzivatelDeleted;
                var item = db.Kalendare.FirstOrDefault(u => u.UzivatelId == evt.UzivatelId);
                if (item != null)
                {
                    if (item.EventGuid != evt.EventId)
                    {
                        await RequestEvents(entityId);
                    }
                    else
                    {
                        await DeleteByUzivatel(evt);
                    }
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
                        var created = JsonConvert.DeserializeObject<EventUzivatelCreated>(msg.Event);
                        var forCreate = db.Kalendare.FirstOrDefault(u => u.UzivatelId == created.UzivatelId & u.Rok == created.EventCreated.Year);
                        if (forCreate != null)
                        {
                            var forAdd = await Create(created);
                            db.Kalendare.Add(forAdd);
                            db.SaveChanges();
                        }
                        break;                    
                    case MessageType.UzivatelUpdated:
                        var updated = JsonConvert.DeserializeObject<EventUzivatelUpdated>(msg.Event);
                        var kalendar = db.Kalendare.Where(k => k.UzivatelId == updated.UzivatelId && k.Rok == updated.EventCreated.Year).FirstOrDefault();
                        if (kalendar != null)
                        {
                            kalendar = Modify(updated, kalendar);
                            db.Kalendare.Update(kalendar);
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
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
                var model = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == evt.DatumOd.Year);
                var kalendar = JsonConvert.DeserializeObject<Year>(model.Body);
                var interval = (evt.DatumDo - evt.DatumOd).TotalDays;
                for (int i = 0; i <= interval; i++)
                {
                    var focus = evt.DatumOd.AddDays(i);
                    var mesic = kalendar.Months[focus.Month - 1];
                    var den = mesic.Days[focus.Day - 1];
                    var polozka = new Polozka()
                    {
                        Id = evt.UdalostTypId,
                        DatumDo = evt.DatumOd,
                        DatumOd = evt.DatumDo,
                        Nazev = evt.Nazev,
                        UzivatelId = evt.UzivatelId,
                        CeleJmeno = evt.UzivatelCeleJmeno
                    };
                    den.Polozky.Add(polozka);
                    var result = JsonConvert.SerializeObject(kalendar);
                    model.DatumAktualizace = DateTime.Now;
                    model.Body = result;
                    await db.SaveChangesAsync();
                }            
        }
        public async Task UpdateByUdalost(EventUdalostUpdated evt)
        {
            var model = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == evt.DatumOd.Year);
            var kalendar = JsonConvert.DeserializeObject<Year>(model.Body);
            var interval = (evt.DatumDo - evt.DatumOd).TotalDays;
            for (int i = 0; i <= interval; i++)
            {
                var focus = evt.DatumOd.AddDays(i);
                var mesic = kalendar.Months[focus.Month - 1];
                var den = mesic.Days[focus.Day - 1];
                var polozka = new Polozka()
                {
                    Id = evt.UdalostTypId,
                    DatumDo = evt.DatumOd,
                    DatumOd = evt.DatumDo,
                    Nazev = evt.Nazev,
                    UzivatelId = evt.UzivatelId,
                    CeleJmeno = evt.UzivatelCeleJmeno
                };
                den.Polozky.Add(polozka);
                var result = JsonConvert.SerializeObject(kalendar);
                model.DatumAktualizace = DateTime.Now;
                model.Body = result;
                await db.SaveChangesAsync();
            }
        }
        public async Task DeleteByUdalost(EventUdalostRemoved evt)
        {
         
                await db.SaveChangesAsync();
            
        }

        public async Task CreateByUzivatel(EventUzivatelCreated evt)
        {
            var kalendar = db.Kalendare.Where(k => k.UzivatelId == evt.UzivatelId && k.Rok == evt.EventCreated.Year).FirstOrDefault();
            if (kalendar == null)
            {
                kalendar = await Create(evt);
                db.Kalendare.Add(kalendar);
            }            
         await db.SaveChangesAsync();
        }

        public async Task UpdateByUzivatel(EventUzivatelUpdated evt)
        {
            var kalendar = db.Kalendare.Where(k => k.UzivatelId == evt.UzivatelId && k.Rok == evt.EventCreated.Year).FirstOrDefault();
            if (kalendar !=null)
            {
                kalendar = Modify(evt,kalendar);
                db.Kalendare.Update(kalendar);
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
