
using CommandHandler;
using EventLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nastaveni_Api.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nastaveni_Api.Repositories
{
    public class NastaveniRepository : INastaveniRepository
    {
        private readonly NastaveniDbContext db;
        private Publisher _publisher;
        private MessageHandler _handler;
        public NastaveniRepository(NastaveniDbContext dbContext, Publisher publisher)
        {
            db = dbContext;
            _publisher = publisher;
            _handler = new MessageHandler(publisher);
        }
            public async Task Add(CommandNastaveniCreate cmd)
        {
            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.NastaveniCreate, null, version, false);

            //TODO: ulozit create do EventStore;
            var model = new Nastaveni()
            {
           
        };
            db.NastaveniDochazky.Add(model);
            await db.SaveChangesAsync();
            db.Dispose();

            //Description: Uložit a publikovat event UzivatelCreated
            var ev= JsonConvert.SerializeObject(new EventNastaveniCreated()
            {
               
                
            });
            //TODO: Uložení Event do EventStore;
            var responseGuid = await _handler.PublishEvent(ev, MessageType.NastaveniCreated, cmdGuid, version, true);
        }

        public async Task<Nastaveni> Get(string id) => await Task.Run(() => db.NastaveniDochazky.FirstOrDefault(b => b.Id == Convert.ToInt32(id)));
        public async Task<List<Nastaveni>> GetList() => await db.NastaveniDochazky.ToListAsync();
        public async Task Remove(CommandNastaveniRemove cmd)
        {

            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.NastaveniRemove, null, version, false);
            var remove = db.NastaveniDochazky.Find(cmd.UzivatelId);
            db.NastaveniDochazky.Remove(remove);
            await db.SaveChangesAsync();

            var ev = JsonConvert.SerializeObject(new EventUzivatelDeleted()
            {
                UzivatelId = cmd.UzivatelId,
            });

            var responseGuid = await _handler.PublishEvent(ev, MessageType.NastaveniRemoved, cmdGuid, version, true);
        }

        public async Task Update(CommandNastaveniUpdate cmd)
        {
            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.NastaveniUpdate, null, version, false);

            var model= db.NastaveniDochazky.Find(cmd.UzivatelId);
            db.NastaveniDochazky.Remove(model);
            await db.SaveChangesAsync();

            var ev = JsonConvert.SerializeObject(
                new EventUzivatelUpdated()
                {
                   
                   
                }) ;
            var responseGuid = await _handler.PublishEvent(ev, MessageType.NastaveniUpdated, cmdGuid, version, true);
        }

 
    }
}
