
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
        private ConnectionFactory factory;
        private Publisher publisher;
        public DochazkaRepository(DochazkaDbContext dochazkaDbContext) {
            db = dochazkaDbContext;
            factory = new ConnectionFactory() { HostName = "rabbitmq" };
           publisher = new Publisher(factory, "dochazka.ex");
        }
        public async Task<bool> Add(DochazkaModel input)
        {
          
            var body = JsonConvert.SerializeObject(
                   new CommandDochazkaCreate()
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
        public async Task<bool> Remove(int id)
        {
          
            var body = JsonConvert.SerializeObject(
                   new CommandDochazkaRemove()
                   {
                       DochazkaId = id
                   });
            return await publisher.Push(body);
        }

        public async Task Update(DochazkaModel update)
        {
           
            var body = JsonConvert.SerializeObject(
                   new CommandDochazkaUpdate()
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
