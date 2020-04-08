
using CommandHandler;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Api.Entities;
using Udalost_Api.Models;

namespace Udalost_Api.Repositories
{
    public class UdalostRepository : IUdalostRepository
    {
        private readonly UdalostDbContext db;
        private Publisher _publisher;
        private MessageHandler _handler;

        public UdalostRepository(UdalostDbContext dbContext, Publisher publisher)
        {
            db = dbContext;
            _publisher = publisher;
            _handler = new MessageHandler(publisher);
        }

        public async Task Add(CommandUdalostCreate cmd)
        {

            var model = new Udalost()
            {
                Id = Guid.NewGuid(),
                DatumOd = cmd.DatumOd,
                DatumDo = cmd.DatumDo,
                DatumZadal = DateTime.Now,
                UzivatelCeleJmeno = cmd.UzivatelCeleJmeno,
                Nazev = cmd.Nazev,
                Popis = cmd.Popis,
                UdalostTypId = cmd.UdalostTypId,
                UzivatelId = cmd.UzivatelId,
            };
            db.Udalosti.Add(model);
           
            await db.SaveChangesAsync();

            var ev = new EventUdalostCreated()
                {                   
                    DatumOd = model.DatumOd,
                    DatumDo = model.DatumDo,
                    DatumZadal = DateTime.Now,
                    UzivatelCeleJmeno = cmd.UzivatelCeleJmeno,
                    Nazev = cmd.Nazev,
                    Popis = model.Popis,
                    UdalostTypId = model.UdalostTypId,
                    UzivatelId = model.UzivatelId,
                };

          
        }

        public async Task<Udalost> Get(Guid id) => await Task.Run(() => db.Udalosti.FirstOrDefault(b => b.Id == id));

        public async Task<List<Udalost>> GetList() => await db.Udalosti.ToListAsync();

        public async Task Remove(CommandUdalostRemove cmd)
        {           
            var remove = db.Udalosti.Find(cmd.UdalostId);
            db.Udalosti.Remove(remove);
            await db.SaveChangesAsync();
            var ev = new EventUdalostRemoved()
            {               
                UdalostId = cmd.UdalostId,
            };   
        }
        public async Task Update(CommandUdalostUpdate cmd)
        {
           
            var udalost = new Udalost()
            {
                Id = cmd.UdalostId,
                DatumOd = cmd.DatumOd,
                DatumDo = cmd.DatumDo,
                UzivatelCeleJmeno = cmd.UzivatelCeleJmeno,
                Nazev = cmd.Nazev,
                DatumZadal = DateTime.Now,
                Popis = cmd.Popis,
                UdalostTypId = cmd.UdalostTypId,
                UzivatelId = cmd.UzivatelId,
            };
            var model = db.Udalosti.Find(cmd.UdalostId);
            model = udalost;
            await db.SaveChangesAsync();

            var ev =
                new EventUdalostUpdated()
                {

                    UdalostId = model.Id,
                    DatumOd = model.DatumOd,
                    DatumDo = model.DatumDo,
                    DatumZadal = DateTime.Now,
                    UzivatelCeleJmeno = cmd.UzivatelCeleJmeno,
                    Nazev = cmd.Nazev,
                    Popis = model.Popis,
                    UdalostTypId = model.UdalostTypId,
                    UzivatelId = model.UzivatelId,
                };
              
        }



    }
}
