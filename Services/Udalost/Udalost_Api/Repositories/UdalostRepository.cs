
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
        private Publisher publisher;
       
        public UdalostRepository(UdalostDbContext udalostDbContext, Publisher _publisher) {
            db = udalostDbContext;
            publisher = _publisher;
    }

        public async Task Add(UdalostModel input)
        {
            var body = JsonConvert.SerializeObject(
                   new CommandUdalostCreate()
                   {
                       UdalostTypId = input.UdalostTypId,
                       UzivatelId = input.UzivatelId,
                       DatumOd = input.DatumOd,
                       DatumDo = input.DatumDo,
                       DatumZadal = DateTime.Now,
                       Popis = input.Popis,
                   });
            await publisher.Push(body);
        }

        public async Task<Udalost> Get(int id) => await Task.Run(() => db.Udalosti.FirstOrDefault(b => b.Id == id));

        public async Task<List<Udalost>> GetList() => await db.Udalosti.ToListAsync();

        public async Task Remove(int id)
        {
            var body = JsonConvert.SerializeObject(
                   new CommandUdalostRemove()
                   {
                       UdalostId = Convert.ToInt32(id)
                   }) ;
            await publisher.Push(body);
        }

        public async Task Update(UdalostModel update)
        {           
            var body = JsonConvert.SerializeObject(
                   new CommandUdalostUpdate()
                   {
                       UdalostId = update.Id,
                       UzivatelId = update.UzivatelId,
                       DatumOd = update.DatumOd,
                       DatumDo = update.DatumDo,
                       DatumZadal = DateTime.Now,
                       UdalostTypId = update.UdalostTypId,
                       Popis = update.Popis
                   }); ;
            await publisher.Push(body);
        }

    }
}
