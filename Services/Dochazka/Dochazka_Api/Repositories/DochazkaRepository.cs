
using CommandHandler;
using Dochazka_Api.Entities;
using EventLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Api;

namespace Dochazka_Api.Repositories
{
    public class DochazkaRepository : IDochazkaRepository
    {
        private readonly DochazkaDbContext db;
        public DochazkaRepository(DochazkaDbContext dochazkaDbContext) {
            db = dochazkaDbContext;
           
        }
        public async Task<bool> Add(DochazkaModel input)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            var publisher = new PublishCommand(factory, "dochazka.ex");
            var body = JsonConvert.SerializeObject(
                   new EventDochazkaCreate()
                   {
                       Prichod = input.Prichod,
                       UzivatelId = input.UzivatelId,
                       CteckaId = input.CteckaId,
                       Datum = DateTime.Now,
                   });     
            return await publisher.Push(body);
        }

        public Dochazka Get(int id)
        {
           return db.Dochazka.FirstOrDefault(b => b.Id == id);
        }
        public IEnumerable<Dochazka> GetList()
        {
            return db.Dochazka;
        }
        public async Task<bool> Delete(int id)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            var publisher = new PublishCommand(factory, "dochazka.ex");
            var body = JsonConvert.SerializeObject(
                   new EventDochazkaRemove()
                   {
                       DochazkaId = id
                   });
            return await publisher.Push(body);
        }

        public async Task Update(DochazkaModel update)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            var publisher = new PublishCommand(factory, "dochazka.ex");
            var body = JsonConvert.SerializeObject(
                   new EventDochazkaUpdate()
                   {
                       DochazkaId = update.Id,
                       Prichod = update.Prichod,
                       UzivatelId = update.UzivatelId,
                       CteckaId = update.CteckaId,
                       Datum = DateTime.Now,
                   }) ;            
            await publisher.Push(body);
        }

 
    }
}
