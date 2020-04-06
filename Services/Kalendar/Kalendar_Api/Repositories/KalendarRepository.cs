
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

        public DateTime GetLast()
        {
            var response = DateTime.MinValue;
            var list = db.Kalendare.OrderByDescending(k => k.DatumAktualizace);
            if (list.Any()) {
                return list.First().DatumAktualizace;
            }
            return DateTime.MinValue;
        }
        public async Task<Kalendar> Get(Guid id) => await Task.Run(() => db.Kalendare.FirstOrDefault(b => b.KalendarId == id));
        public async Task<List<Kalendar>> GetList()
        {
            //var model =new List<KalendarModel>();
            //var list = await db.Kalendare.ToListAsync();
            //foreach (var item in list)
            //{
            //    var kalendar = new KalendarModel();
            //    kalendar.Id = item.KalendarId;
            //    kalendar.Rok = item.Rok;
            //    kalendar.UzivatelId = item.UzivatelId;
            //    kalendar.Kalendar = JsonConvert.DeserializeObject<Year>(item.Body);
            //    model.Add(kalendar);
            //}
            return await db.Kalendare.ToListAsync(); 
        }


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
            db.Kalendare.Remove(update);
            await db.SaveChangesAsync();
            var ev = 
                new EventKalendarUpdated()
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

        public async Task AddByUzivatel(EventUzivatelCreated evt)
        {
            var rok = DateTime.Today.Year;
            if (!db.Kalendare.Where(k => k.UzivatelId == evt.UzivatelId && k.Rok == rok).Any())
            {
                var body = await new KalendarGenerator().KalendarNew();
                var kalendar = new Kalendar()
                {
                    UzivatelId = evt.UzivatelId,
                    Rok = rok,
                    Body = JsonConvert.SerializeObject(body),
                    DatumAktualizace = DateTime.Now
                    
                };
                db.Kalendare.Add(kalendar);
            }            
         await db.SaveChangesAsync();
        }

        public async Task UpdateByUzivatel(EventUzivatelUpdated evt)
        {
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
