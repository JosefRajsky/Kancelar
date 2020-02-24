
using CommandHandler;
using Dochazka_Api.Entities;
using DochazkaLibrary.Models;
using EventLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Dochazka_Api.Repositories
{
    public class DochazkaRepository : IDochazkaRepository
    {
        private readonly DochazkaDbContext db;
        private Publisher _publisher;
        public DochazkaRepository(DochazkaDbContext dochazkaDbContext, Publisher publisher) {
            db = dochazkaDbContext;
            _publisher = publisher;          
        }
        public async Task Add(DochazkaModel model)
        {           
            var cmd = JsonConvert.SerializeObject(
                   new CommandDochazkaCreate()
                   {
                       Prichod = model.Prichod,
                       UzivatelId = model.UzivatelId,
                       CteckaId = model.CteckaId,
                       Datum = DateTime.Now,
                   });
            //TODO: ulozit create do EventStore;
            var add = new Dochazka()
            {
                Den = model.Datum.Day,
                DenTydne = (int)model.Datum.DayOfWeek,
                Mesic = model.Datum.Month,
                Rok = model.Datum.Year,
                Prichod = model.Prichod,
                Tick = model.Datum.Ticks,
                UzivatelId = model.UzivatelId,
            };
            db.Dochazka.Add(add);
            await db.SaveChangesAsync();
            db.Dispose();

            //Description: Uložit a publikovat event DochazkaCreated
            var response = JsonConvert.SerializeObject(new EventDochazkaCreated()
            {
                Prichod = model.Prichod,
                UzivatelId = model.UzivatelId,
                CteckaId = model.CteckaId,
                Datum = model.Datum,
            });
            //TODO: Uložení Event do EventStore;
            await _publisher.Push(response);
        }

        public async Task<Dochazka> Get(string id) => await Task.Run(() => db.Dochazka.FirstOrDefault(b => b.Id == Convert.ToInt32(id)));
        public async Task<List<Dochazka>> GetList() => await db.Dochazka.ToListAsync();
        public async Task Remove(int id)
        {
            var cmd = JsonConvert.SerializeObject(
                 new CommandDochazkaRemove()
                 {
                    DochazkaId = id,
                 });

            var remove = db.Dochazka.Find(id);
            db.Dochazka.Remove(remove);
            await db.SaveChangesAsync();

            var response = JsonConvert.SerializeObject(new EventDochazkaDeleted()
            {
                DochazkaId = id,
            });

            await _publisher.Push(response);
        }

        public async Task Update(DochazkaModel model)
        {
            var cmd = JsonConvert.SerializeObject(
                 new CommandDochazkaUpdate()
                 {
                     DochazkaId = model.Id,
                     Prichod = model.Prichod,
                     UzivatelId = model.UzivatelId,
                     CteckaId = model.CteckaId,
                     Datum = DateTime.Now,
                 }) ;

            var update= db.Dochazka.Find(model.Id);
            db.Dochazka.Remove(update);
            await db.SaveChangesAsync();

            var response = JsonConvert.SerializeObject(
                new EventDochazkaUpdated()
            {
                DochazkaId = model.Id,
                Prichod = model.Prichod,
                UzivatelId = model.UzivatelId,
                CteckaId = model.CteckaId,
                Datum = DateTime.Now,
            }) ;
            await _publisher.Push(response);
        }

 
    }
}
