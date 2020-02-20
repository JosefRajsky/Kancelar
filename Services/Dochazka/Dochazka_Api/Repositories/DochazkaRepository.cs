
using CommandHandler;
using Dochazka_Api.Entities;
using DochazkaLibrary.Models;
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
        //private ConnectionFactory factory;
        private Publisher _publisher;
        public DochazkaRepository(DochazkaDbContext dochazkaDbContext, Publisher publisher) {
            db = dochazkaDbContext;
            _publisher = publisher;
           // factory = new ConnectionFactory() { HostName = "rabbitmq" };
           //publisher = new Publisher(factory, "dochazka.ex");
        }
        public async Task Add(DochazkaModel input)
        {           
            var body = JsonConvert.SerializeObject(
                   new CommandDochazkaCreate()
                   {
                       Prichod = input.Prichod,
                       UzivatelId = input.UzivatelId,
                       CteckaId = input.CteckaId,
                       Datum = DateTime.Now,
                   });     

           await _publisher.Push(body);
        }

        public Dochazka Get(string id)
        {
           return db.Dochazka.FirstOrDefault(b => b.Id == Convert.ToInt32(id));
        }
        public List<Dochazka> GetList()
        {
            return db.Dochazka.ToList();
        }
        public async Task Remove(string id)
        {

            var body = JsonConvert.SerializeObject(
                   new CommandDochazkaRemove()
                   {
                       DochazkaId = Convert.ToInt32(id)
                   }); ;
            await _publisher.Push(body);
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
            await _publisher.Push(body);
        }

 
    }
}
