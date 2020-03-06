
using CommandHandler;

using EventLibrary;
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
    public class KalendarRepository : IKalendarRepository
    {
        private readonly KalendarDbContext db;
        private Publisher _publisher;
        private MessageHandler _handler;
        public KalendarRepository(KalendarDbContext dbContext, Publisher publisher) {
            db = dbContext;
            _publisher = publisher;
            _handler = new MessageHandler(publisher);
        }
      

        public async Task<Kalendar> Get(string id) => await Task.Run(() => db.Kalendare.FirstOrDefault(b => b.Id == Convert.ToInt32(id)));
        public async Task<IEnumerable<KalendarModel>> GetList()
        {
            var model =new List<KalendarModel>();
            var list = await db.Kalendare.ToListAsync();
            foreach (var item in list)
            {
                var kalendar = new KalendarModel();
                kalendar.Id = item.Id;
                kalendar.Rok = item.Rok;
                kalendar.UzivatelId = item.UzivatelId;
                kalendar.Kalendar = JsonConvert.DeserializeObject<Year>(item.Body);
                model.Add(kalendar);
            }
            return model;
        }

        public async Task Add(CommandKalendarCreate cmd)
        {
            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.DochazkaCreate, null, version, false);

             var model = new Kalendar()
            {
                

                
            };
            db.Kalendare.Add(model);
            await db.SaveChangesAsync();
            db.Dispose();

            //Description: Uložit a publikovat event DochazkaCreated
            var ev = JsonConvert.SerializeObject(new EventKalendarCreated()
            {                
         
            });
             var responseGuid = await _handler.PublishEvent(ev, MessageType.DochazkaCreated, cmdGuid, version, true);
            //TODO: Uložení Event do EventStore;
           
        } 

        public async Task Update(CommandKalendarUpdate cmd)
        {
            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.DochazkaUpdate, null, version,false);
            var update= db.Kalendare.Find(cmd.KalendarId);
            db.Kalendare.Remove(update);
            await db.SaveChangesAsync();

            var ev = JsonConvert.SerializeObject(
                new EventKalendarUpdated()
                {        
                    UzivatelId = cmd.UzivatelId,                
                  
                }) ;
            var responseGuid = await _handler.PublishEvent(ev, MessageType.DochazkaUpdated, cmdGuid, version,true);
        }

        public async Task UpdateByUdalost(EventUdalostCreated evt)
        {
            var model = db.Kalendare.FirstOrDefault(k => k.UzivatelId == evt.UzivatelId && k.Rok == evt.DatumOd.Year);
            var kalendar = JsonConvert.DeserializeObject<Year>(model.Body);

            var mesic = kalendar.Months[evt.DatumOd.Month-1];
             var den = mesic.Days[evt.DatumOd.Day-1];
            //den.Body = JsonConvert.SerializeObject(evt);
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

            model.Body = result;
            await db.SaveChangesAsync();
        }

        public async Task AddByUzivatel(EventUzivatelCreated evt)
        {
            var rok = DateTime.Today.Year;

            if (!db.Kalendare.Where(k => k.UzivatelId == evt.UzivatelId && k.Rok == rok).Any())
            {

                var body =await new KalendarGenerator().KalendarNew();

                var kalendar = new Kalendar()
                {
                    UzivatelId = evt.UzivatelId,
                    Rok = rok,
                    Body = JsonConvert.SerializeObject(body)
                };
                db.Kalendare.Add(kalendar);
            }
            
         await db.SaveChangesAsync();
        }
    }
}
