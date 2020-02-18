﻿
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
        private ConnectionFactory factory;
        private Publisher publisher;
       
        public UdalostRepository(UdalostDbContext udalostDbContext) {
            db = udalostDbContext;
            factory = new ConnectionFactory() { HostName = "rabbitmq" };
            publisher = new Publisher(factory, "udalost.ex");
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
            var body = JsonConvert.SerializeObject(
                   new CommandUdalostRemove()
                   {
                       UdalostId = id
                   });
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
