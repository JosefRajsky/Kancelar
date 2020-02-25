
using CommandHandler;
using EventLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Uzivatel_Api.Repositories
{
    public class UzivatelRepository : IUzivatelRepository
    {
        private readonly UzivatelDbContext db;
        private Publisher _publisher;
        private MessageHandler _handler;
        public UzivatelRepository(UzivatelDbContext dbContext, Publisher publisher)
        {
            db = dbContext;
            _publisher = publisher;
            _handler = new MessageHandler(publisher);
        }
            public async Task Add(CommandUzivatelCreate cmd)
        {
            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.UzivatelCreate, null, version, false);

            //TODO: ulozit create do EventStore;
            var model = new Uzivatel()
            {
                Id = cmd.UzivatelId,
            TitulPred = cmd.TitulPred,
            Jmeno = cmd.Jmeno,
            Prijmeni = cmd.Prijmeni,
            TitulZa = cmd.TitulZa,
            Pohlavi = cmd.Pohlavi,
            DatumNarozeni = cmd.DatumNarozeni,
            Email = cmd.Email,
            Telefon = cmd.Telefon,
            Foto = cmd.Foto,
        };
            db.Uzivatele.Add(model);
            await db.SaveChangesAsync();
            db.Dispose();

            //Description: Uložit a publikovat event UzivatelCreated
            var ev= JsonConvert.SerializeObject(new EventUzivatelCreated()
            {
               
                UzivatelId = model.Id,
                TitulPred = model.TitulPred,
                Jmeno = model.Jmeno,
                Prijmeni = model.Prijmeni,
                TitulZa = model.TitulZa,
                Pohlavi = model.Pohlavi,
                DatumNarozeni = model.DatumNarozeni,
                Email = model.Email,
                Telefon = model.Telefon,
                Foto = model.Foto,
            });
            //TODO: Uložení Event do EventStore;
            var responseGuid = await _handler.PublishEvent(ev, MessageType.UzivatelCreated, cmdGuid, version, true);
        }

        public async Task<Uzivatel> Get(string id) => await Task.Run(() => db.Uzivatele.FirstOrDefault(b => b.Id == Convert.ToInt32(id)));
        public async Task<List<Uzivatel>> GetList() => await db.Uzivatele.ToListAsync();
        public async Task Remove(CommandUzivatelRemove cmd)
        {

            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.UzivatelRemove, null, version, false);
            var remove = db.Uzivatele.Find(cmd.UzivatelId);
            db.Uzivatele.Remove(remove);
            await db.SaveChangesAsync();

            var ev = JsonConvert.SerializeObject(new EventUzivatelDeleted()
            {
                UzivatelId = cmd.UzivatelId,
            });

            var responseGuid = await _handler.PublishEvent(ev, MessageType.UzivatelRemoved, cmdGuid, version, true);
        }

        public async Task Update(CommandUzivatelUpdate cmd)
        {
            var version = 1;
            var cmdGuid = await _handler.MakeCommand(cmd, MessageType.UzivatelUpdate, null, version, false);

            var model= db.Uzivatele.Find(cmd.UzivatelId);
            db.Uzivatele.Remove(model);
            await db.SaveChangesAsync();

            var ev = JsonConvert.SerializeObject(
                new EventUzivatelUpdated()
                {
                   
                    UzivatelId = model.Id,
                    TitulPred = model.TitulPred,
                    Jmeno = model.Jmeno,
                    Prijmeni = model.Prijmeni,
                    TitulZa = model.TitulZa,
                    Pohlavi = model.Pohlavi,
                    DatumNarozeni = model.DatumNarozeni,
                    Email = model.Email,
                    Telefon = model.Telefon,
                    Foto = model.Foto,
                }) ;
            var responseGuid = await _handler.PublishEvent(ev, MessageType.DochazkaUpdated, cmdGuid, version, true);
        }

 
    }
}
