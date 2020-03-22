
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
        public async Task<Uzivatel> Get(Guid id) => await Task.Run(() => db.Uzivatele.FirstOrDefault(b => b.Guid == id));
        public async Task<List<Uzivatel>> GetList() => await db.Uzivatele.ToListAsync();

        public async Task AddGeneration(Guid entityId)
        {
            var model = db.Uzivatele.Find(entityId);
            model.Generation = model.Generation + 1;
            await db.SaveChangesAsync();
        }

        public async Task<Uzivatel> CreateRecord(CommandUzivatelCreate cmd) {
            try
            {
                var model = new Uzivatel()
                {
                    Guid = cmd.UzivatelGuid,
                    TitulPred = cmd.TitulPred,
                    Jmeno = cmd.Jmeno,
                    Prijmeni = cmd.Prijmeni,
                    TitulZa = cmd.TitulZa,
                    Pohlavi = cmd.Pohlavi,
                    DatumNarozeni = cmd.DatumNarozeni,
                    Email = cmd.Email,
                    Telefon = cmd.Telefon,
                    Foto = cmd.Foto,
                    Generation = 0
                };
                db.Uzivatele.Add(model);
                await db.SaveChangesAsync();
              
                db.Uzivatele.Update(model);
                await db.SaveChangesAsync();
                return model;
            }
            catch (Exception e) 
            {
                var b = e;
                throw;
            }
          
        }

        public async Task Add(CommandUzivatelCreate cmd, Guid? replayed)
        {
            if (replayed != null)
            {
                var model = db.Uzivatele.Find(cmd.UzivatelGuid);
                if (model == null)
                {
                    await CreateRecord(cmd);
                }
                else
                {
                    await AddGeneration(cmd.UzivatelGuid);
                }
            }
            else {
                var model = await CreateRecord(cmd);
                //Description: Uložit a publikovat event UzivatelCreated
                var ev = new EventUzivatelCreated()
                {
                    UzivatelGuid = new Guid(),
                    TitulPred = model.TitulPred,
                    Jmeno = model.Jmeno,
                    Prijmeni = model.Prijmeni,
                    TitulZa = model.TitulZa,
                    Pohlavi = model.Pohlavi,
                    DatumNarozeni = model.DatumNarozeni,
                    Email = model.Email,
                    Telefon = model.Telefon,
                    Foto = model.Foto,
                };
                //TODO: Uložení Event do EventStore;
                cmd.UzivatelGuid = model.Guid;
                var responseGuid = await _handler.PublishEvent(ev, cmd, MessageType.UzivatelCreated, null, model.Generation + 1, model.Guid);                  
                model.LastEvent = responseGuid;
                await db.SaveChangesAsync();
               
            }
        }

        public async Task Remove(CommandUzivatelRemove cmd, Guid? replayed)
        {
            var remove = db.Uzivatele.Find(cmd.UzivatelGuid);
            db.Uzivatele.Remove(remove);
            var ev = new EventUzivatelDeleted()
            {
                UzivatelGuid = cmd.UzivatelGuid,
            };
            var responseGuid = await _handler.PublishEvent(ev, cmd, MessageType.UzivatelRemoved, remove.LastEvent, remove.Generation + 1, remove.Guid);
            await db.SaveChangesAsync();
        }
        public async Task Update(CommandUzivatelUpdate cmd, Guid? replayed)
        {
            var model = db.Uzivatele.Find(cmd.UzivatelGuid);
            model.TitulPred = cmd.TitulPred;
            model.Jmeno = cmd.Jmeno;
            model.Prijmeni = cmd.Prijmeni;
            model.TitulZa = cmd.TitulZa;
            model.Pohlavi = cmd.Pohlavi;
            model.DatumNarozeni = cmd.DatumNarozeni;
            model.Email = cmd.Email;
            model.Telefon = cmd.Telefon;
            model.Foto = cmd.Foto;

            var ev = new EventUzivatelUpdated()
            {
                UzivatelGuid = model.Guid,
                TitulPred = model.TitulPred,
                Jmeno = model.Jmeno,
                Prijmeni = model.Prijmeni,
                TitulZa = model.TitulZa,
                Pohlavi = model.Pohlavi,
                DatumNarozeni = model.DatumNarozeni,
                Email = model.Email,
                Telefon = model.Telefon,
                Foto = model.Foto,
            };
            db.Uzivatele.Update(model);
            await db.SaveChangesAsync();

            var responseGuid = await _handler.PublishEvent(ev, cmd, MessageType.UzivatelUpdated, model.LastEvent, model.Generation + 1, model.Guid);
            model.LastEvent = responseGuid;
            model.Generation = model.Generation + 1;


            await db.SaveChangesAsync();

        }



    }
}
