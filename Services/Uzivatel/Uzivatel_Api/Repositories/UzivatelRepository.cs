
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
        public async Task Restore(CommandUzivatelCreate cmd, Guid entityId)
        {
            var item = db.Uzivatele.Find(entityId);
            if(item == null) {

                var model = new Uzivatel()
                {
                    Guid = cmd.UzivatelId,
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
            }
        }
        public async Task ReUpdate(CommandUzivatelUpdate cmd, Guid entityId) {
            var model = db.Uzivatele.Find(entityId);
            model.TitulPred = cmd.TitulPred;
            model.Jmeno = cmd.Jmeno;
            model.Prijmeni = cmd.Prijmeni;
            model.TitulZa = cmd.TitulZa;
            model.Pohlavi = cmd.Pohlavi;
            model.DatumNarozeni = cmd.DatumNarozeni;
            model.Email = cmd.Email;
            model.Telefon = cmd.Telefon;
            model.Foto = cmd.Foto;
            db.Uzivatele.Update(model);
            await db.SaveChangesAsync();
        }
        public async Task Remove(CommandUzivatelRemove cmd, Guid entityId)
        {
            var remove = db.Uzivatele.Find(cmd.UzivatelId);
            db.Uzivatele.Remove(remove);
            var ev = new EventUzivatelDeleted()
            {
                UzivatelId = cmd.UzivatelId,
            };
            var responseGuid = await _handler.PublishEvent(ev, cmd, MessageType.UzivatelRemoved, remove.LastEvent, remove.Generation + 1, remove.Guid);
            await db.SaveChangesAsync();
        }

        public async Task Add(CommandUzivatelCreate cmd)
        {
            var model = new Uzivatel()
            {
                Guid = cmd.UzivatelId,
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
            var ev = new EventUzivatelCreated()
            {
                UzivatelId = new Guid(),
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
            cmd.UzivatelId = model.Guid;
            var responseGuid = await _handler.PublishEvent(ev, cmd, MessageType.UzivatelCreated, null, model.Generation + 1, model.Guid);           
            model.LastEvent = responseGuid;
            model.Generation = model.Generation + 1;
            await db.SaveChangesAsync();
        }
        public async Task Remove(CommandUzivatelRemove cmd)
        {
            var remove = db.Uzivatele.Find(cmd.UzivatelId);
            db.Uzivatele.Remove(remove);
            var ev = new EventUzivatelDeleted()
            {
                UzivatelId = cmd.UzivatelId,
            };
            var responseGuid = await _handler.PublishEvent(ev, cmd, MessageType.UzivatelRemoved, remove.LastEvent, remove.Generation + 1, remove.Guid);
            await db.SaveChangesAsync();
        }      
        public async Task Update(CommandUzivatelUpdate cmd)
        {
                var model = db.Uzivatele.Find(cmd.UzivatelId);
                model.TitulPred = cmd.TitulPred;
                model.Jmeno = cmd.Jmeno;
                model.Prijmeni = cmd.Prijmeni;
                model.TitulZa = cmd.TitulZa;
                model.Pohlavi = cmd.Pohlavi;
                model.DatumNarozeni = cmd.DatumNarozeni;
                model.Email = cmd.Email;
                model.Telefon = cmd.Telefon;
                model.Foto = cmd.Foto;
                db.Uzivatele.Update(model);
                await db.SaveChangesAsync();
                
                    var ev = new EventUzivatelUpdated()
                    {
                        UzivatelId = model.Guid,
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
                    var responseGuid = await _handler.PublishEvent(ev, cmd, MessageType.UzivatelUpdated, model.LastEvent, model.Generation + 1, model.Guid);
                    model.LastEvent = responseGuid;
                    model.Generation = model.Generation + 1;
                    await db.SaveChangesAsync();
                }

            }

        }



  




