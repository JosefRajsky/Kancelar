
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
        private MessageHandler _handler;

        public UdalostRepository(UdalostDbContext dbContext, Publisher publisher)
        {
            db = dbContext;
            _publisher = publisher;
            _handler = new MessageHandler(publisher);
        }

        public async Task Add(CommandUdalostCreate cmd)
        {
            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.UdalostCreate, null, version, false);
            var model = new Udalost()
            {
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

            var ev = JsonConvert.SerializeObject(
                new EventUdalostCreated()
                {                   
                    DatumOd = model.DatumOd,
                    DatumDo = model.DatumDo,
                    DatumZadal = DateTime.Now,
                    UzivatelCeleJmeno = cmd.UzivatelCeleJmeno,
                    Nazev = cmd.Nazev,
                    Popis = model.Popis,
                    UdalostTypId = model.UdalostTypId,
                    UzivatelId = model.UzivatelId,
                }
                );

            var responseGuid = await _handler.PublishEvent(ev, MessageType.UdalostCreated, cmdGuid, version, true);
        }

        public async Task<Udalost> Get(int id) => await Task.Run(() => db.Udalosti.FirstOrDefault(b => b.Id == id));

        public async Task<List<Udalost>> GetList() => await db.Udalosti.ToListAsync();

        public async Task Remove(CommandUdalostRemove cmd)
        {
            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.UdalostRemove, null, version, false);
            var remove = db.Udalosti.Find(cmd.UdalostId);
            db.Udalosti.Remove(remove);
            await db.SaveChangesAsync();

            var ev = JsonConvert.SerializeObject(new EventUdalostRemoved()
            {               
                UdalostId = cmd.UdalostId,
            });
            var responseGuid = await _handler.PublishEvent(ev, MessageType.UdalostRemove, cmdGuid, version, true);
        }

        public async Task Update(CommandUdalostUpdate cmd)
        {
            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.UdalostUpdate, null, version, false);
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

            var ev = JsonConvert.SerializeObject(
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
                }
                ) ;
            var responseGuid = await _handler.PublishEvent(ev, MessageType.UdalostUpdated, cmdGuid, version, true);
        }

    }
}
