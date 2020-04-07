
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
                            var forAdd = Create(created);
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
        private Kalendar Create(EventUzivatelCreated evt)
        {
            var body = new KalendarGenerator().KalendarNew();
            var model = new Kalendar()
            {
                UzivatelId = evt.UzivatelId,
                Rok = evt.EventCreated.Year,
                Generation = evt.Generation,
                EventGuid = evt.EventId,KalendarId = new Guid(),
                UzivatelCeleJmeno = $"{evt.Prijmeni} {evt.Jmeno}",
                Body = JsonConvert.SerializeObject(body),
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

        public async Task<Kalendar> Get(Guid id) => await Task.Run(() => db.Kalendare.FirstOrDefault(b => b.KalendarId == id));
        public async Task<List<Kalendar>> GetList()=> await db.Kalendare.ToListAsync();   
        public async Task Add(CommandKalendarCreate cmd,bool publish)
        {

            var body = await new KalendarGenerator().KalendarNew();
            var kalendar = new Kalendar()
            {
                UzivatelId = cmd.UzivatelId,
                UzivatelCeleJmeno = cmd.CeleJmeno,
                Rok = cmd.Rok,
                Body = JsonConvert.SerializeObject(body),
                DatumAktualizace = DateTime.Now                
            };
            db.Kalendare.Add(kalendar);
            db.Dispose();           
        } 

        public async Task Update(CommandKalendarUpdate cmd, bool publish)
        {
          
            //var cmdGuid = await _handler.MakeCommand(cmd, MessageType.KalendarUpdate, null, version, publish);
            var update= db.Kalendare.Find(cmd.KalendarId);
            db.Kalendare.Update(update);
            await db.SaveChangesAsync();
            var ev = new EventKalendarUpdated()
                {        
                    UzivatelId = cmd.UzivatelId, 
                } ;
            update.DatumAktualizace = DateTime.Now;
        }
        public async Task UpdateByUdalost(EventUdalostCreated evt)
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

        public async Task CreateByUzivatel(EventUzivatelCreated evt)
        {
            var kalendar = db.Kalendare.Where(k => k.UzivatelId == evt.UzivatelId && k.Rok == evt.EventCreated.Year).FirstOrDefault();
            if (kalendar == null)
            {
                kalendar = Create(evt);
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

       
    }
}
