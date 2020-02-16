
using CommandHandler;
using EventLibrary;
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
        public UdalostRepository(UdalostDbContext udalostDbContext) {
            db = udalostDbContext;
           
        }

        public async Task Add(UdalostModel input)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            var publisher = new PublishCommand(factory, "udalost.ex");
            var body = JsonConvert.SerializeObject(
                   new EventUdalostCreate()
                   {
                       UzivatelId = input.UzivatelId,
                       DatumOd = input.DatumOd,
                       DatumDo = input.DatumDo,
                       DatumZadal = DateTime.Now,
                       Nazev = input.Nazev,
                   });
            await publisher.Push(body);
        }

        public Udalost Get(int id)
        {
           return db.Udalosti.FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Udalost> GetList()
        {
            return db.Udalosti;
        }

        public async Task Delete(int id)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            var publisher = new PublishCommand(factory, "udalost.ex");
            var body = JsonConvert.SerializeObject(
                   new EventUdalostRemove()
                   {
                       UdalostId = id
                   });
            await publisher.Push(body);

        }

        public async Task Update(UdalostModel update)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            var publisher = new PublishCommand(factory, "udalost.ex");
            var body = JsonConvert.SerializeObject(
                   new EventUdalostUpdate()
                   {
                       UdalostId = update.Id,
                       UzivatelId = update.UzivatelId,
                       DatumOd = update.DatumOd,
                       DatumDo = update.DatumDo,
                       DatumZadal = DateTime.Now,
                       Nazev = update.Nazev,
                   }); ;
            await publisher.Push(body);
        }


    }
}
