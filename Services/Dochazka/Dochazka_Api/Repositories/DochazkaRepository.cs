
using CommandHandler;
using Dochazka_Api.Models;
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
        private MessageHandler _handler;
        public DochazkaRepository(DochazkaDbContext dbContext, Publisher publisher) {
            db = dbContext;
            _publisher = publisher;
            _handler = new MessageHandler(publisher);
        }
      

        public async Task<Dochazka> Get(string id) => await Task.Run(() => db.Dochazka.FirstOrDefault(b => b.Id == Convert.ToInt32(id)));
        public async Task<List<Dochazka>> GetList() => await db.Dochazka.ToListAsync();
        public async Task Add(CommandDochazkaCreate cmd)
        {
            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.DochazkaCreate, null, version, false);

             var model = new Dochazka()
            {
                Den = cmd.Datum.Day,
                DenTydne = (int)cmd.Datum.DayOfWeek,
                Mesic = cmd.Datum.Month,
                Rok = cmd.Datum.Year,
                Prichod = cmd.Prichod,
                Tick = cmd.Datum.Ticks,
                UzivatelId = cmd.UzivatelId,
                Datum = cmd.Datum,
                CteckaId = cmd.CteckaId



            };
            db.Dochazka.Add(model);
            await db.SaveChangesAsync();
            db.Dispose();

            //Description: Uložit a publikovat event DochazkaCreated
            var ev = JsonConvert.SerializeObject(new EventDochazkaCreated()
            {                
                Prichod = model.Prichod,
                UzivatelId = model.UzivatelId,
                CteckaId = model.CteckaId,
                Datum = model.Datum,
            });
             var responseGuid = await _handler.PublishEvent(ev, MessageType.DochazkaCreated, cmdGuid, version, true);
            //TODO: Uložení Event do EventStore;
           
        }
        public async Task Remove(CommandDochazkaRemove cmd)
        {
            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.DochazkaRemove, null, version,false);
            var remove = db.Dochazka.Find(cmd.DochazkaId);
            db.Dochazka.Remove(remove);
            await db.SaveChangesAsync();

            var ev = JsonConvert.SerializeObject(new EventDochazkaDeleted()
            {               
                DochazkaId = cmd.DochazkaId,
            });

            var responseGuid = await _handler.PublishEvent(ev, MessageType.DochazkaRemoved, cmdGuid, version,true);
        }

        public async Task Update(CommandDochazkaUpdate cmd)
        {
            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.DochazkaUpdate, null, version,false);
            var update= db.Dochazka.Find(cmd.DochazkaId);
            db.Dochazka.Remove(update);
            await db.SaveChangesAsync();

            var ev = JsonConvert.SerializeObject(
                new EventDochazkaUpdated()
                {
                   
                    DochazkaId = cmd.DochazkaId,
                    Prichod = cmd.Prichod,
                    UzivatelId = cmd.UzivatelId,
                    CteckaId = cmd.CteckaId,
                    Datum = DateTime.Now,
                }) ;
            var responseGuid = await _handler.PublishEvent(ev, MessageType.DochazkaUpdated, cmdGuid, version,true);
        }

 
    }
}
