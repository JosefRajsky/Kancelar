
using CommandHandler;
using EventLibrary;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Api.Entities;
using UdalostLibrary.Models;

namespace Udalost_Api.Repositories
{
    public class UdalostRepository : IUdalostRepository
    {
        private readonly UdalostDbContext db;
        private Publisher _publisher;

        public UdalostRepository(UdalostDbContext udalostDbContext, Publisher _publisher)
        {
            db = udalostDbContext;
            this._publisher = _publisher;
        }

        public async Task Add(UdalostModel model)
        {
            var cmd = JsonConvert.SerializeObject(
                   new CommandUdalostCreate()
                   {
                       UdalostTypId = model.UdalostTypId,
                       UzivatelId = model.UzivatelId,
                       DatumOd = model.DatumOd,
                       DatumDo = model.DatumDo,
                       DatumZadal = DateTime.Now,
                       Popis = model.Popis,
                   });

            var udalost = new Udalost()
            {
                DatumOd = model.DatumOd,
                DatumDo = model.DatumDo,
                DatumZadal = DateTime.Now,
                Popis = model.Popis,
                UdalostTypId = model.UdalostTypId,
                UzivatelId = model.UzivatelId,
            };
            db.Udalosti.Add(udalost);
            await db.SaveChangesAsync();

            var response = JsonConvert.SerializeObject(
                new EventUdalostCreated()
                {
                    DatumOd = model.DatumOd,
                    DatumDo = model.DatumDo,
                    DatumZadal = DateTime.Now,
                    Popis = model.Popis,
                    UdalostTypId = model.UdalostTypId,
                    UzivatelId = model.UzivatelId,
                }
                );

            await _publisher.Push(response);
        }

        public async Task<Udalost> Get(int id) => await Task.Run(() => db.Udalosti.FirstOrDefault(b => b.Id == id));

        public async Task<List<Udalost>> GetList() => await db.Udalosti.ToListAsync();

        public async Task Remove(int id)
        {
            var cmd = JsonConvert.SerializeObject(
                 new CommandUdalostRemove()
                 {
                    UdalostId = id,
                 });

            var remove = db.Udalosti.Find(id);
            db.Udalosti.Remove(remove);
            await db.SaveChangesAsync();

            var response = JsonConvert.SerializeObject(new EventUdalostRemoved()
            {
                UdalostId = id,
            });

            await _publisher.Push(response);
        }

        public async Task Update(UdalostModel model)
        {
            var cmd = JsonConvert.SerializeObject(
                    new CommandUdalostUpdate()
                    {
                        UdalostId = model.Id,
                        UdalostTypId = model.UdalostTypId,
                        UzivatelId = model.UzivatelId,
                        DatumOd = model.DatumOd,
                        DatumDo = model.DatumDo,
                        DatumZadal = DateTime.Now,
                        Popis = model.Popis,
                    }); 

            var udalost = new Udalost()
            {
                Id = model.Id,
                DatumOd = model.DatumOd,
                DatumDo = model.DatumDo,
                DatumZadal = DateTime.Now,
                Popis = model.Popis,
                UdalostTypId = model.UdalostTypId,
                UzivatelId = model.UzivatelId,
            };
            var update = db.Udalosti.Find(model.Id);
            update = udalost;
            await db.SaveChangesAsync();

            var response = JsonConvert.SerializeObject(
                new EventUdalostUpdated()
                {
                    UdalostId = model.Id,
                    DatumOd = model.DatumOd,
                    DatumDo = model.DatumDo,
                    DatumZadal = DateTime.Now,
                    Popis = model.Popis,
                    UdalostTypId = model.UdalostTypId,
                    UzivatelId = model.UzivatelId,
                }
                );
            await _publisher.Push(response);
        }

    }
}
