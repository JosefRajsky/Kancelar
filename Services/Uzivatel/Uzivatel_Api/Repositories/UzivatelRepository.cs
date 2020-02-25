
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
        public UzivatelRepository(UzivatelDbContext UzivatelDbContext, Publisher publisher) {
            db = UzivatelDbContext;
            _publisher = publisher;          
        }
        public async Task Add(UzivatelModel model)
        {           
            //var cmd = JsonConvert.SerializeObject(
            //       new CommandUzivatelCreate()
            //       {
            //           ParentId = 0,
            //           UzivatelId = model.Id,
            //           TitulPred = model.TitulPred,
            //           Jmeno = model.Jmeno,
            //           Prijmeni = model.Prijmeni,
            //           TitulZa = model.TitulZa,
            //           Pohlavi = model.Pohlavi,
            //           DatumNarozeni = model.DatumNarozeni,
            //           Email = model.Email,
            //           Telefon = model.Telefon,
            //           Foto = model.Foto,
            //       });
            //TODO: ulozit create do EventStore;
            var add = new Uzivatel()
            {
                Id = model.Id,
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
            db.Uzivatele.Add(add);
            await db.SaveChangesAsync();
            db.Dispose();

            //Description: Uložit a publikovat event UzivatelCreated
            var response = JsonConvert.SerializeObject(new EventUzivatelCreated()
            {
                ParentId = 0,
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
            await _publisher.Push(response);
        }

        public async Task<Uzivatel> Get(string id) => await Task.Run(() => db.Uzivatele.FirstOrDefault(b => b.Id == Convert.ToInt32(id)));
        public async Task<List<Uzivatel>> GetList() => await db.Uzivatele.ToListAsync();
        public async Task Remove(int id)
        {
            //var cmd = JsonConvert.SerializeObject(
            //     new CommandUzivatelRemove()
            //     {
            //        UzivatelId = id,
            //     });

            var remove = db.Uzivatele.Find(id);
            db.Uzivatele.Remove(remove);
            await db.SaveChangesAsync();

            var response = JsonConvert.SerializeObject(new EventUzivatelDeleted()
            {
                UzivatelId = id,
            });

            await _publisher.Push(response);
        }

        public async Task Update(UzivatelModel model)
        {
            //var cmd = JsonConvert.SerializeObject(
            //     new CommandUzivatelUpdate()
            //     {
            //         ParentId = 0,
            //         UzivatelId = model.Id,
            //         TitulPred = model.TitulPred,
            //         Jmeno = model.Jmeno,
            //         Prijmeni = model.Prijmeni,
            //         TitulZa = model.TitulZa,
            //         Pohlavi = model.Pohlavi,
            //         DatumNarozeni = model.DatumNarozeni,
            //         Email = model.Email,
            //         Telefon = model.Telefon,
            //         Foto = model.Foto,
            //     }) ;

            var update= db.Uzivatele.Find(model.Id);
            db.Uzivatele.Remove(update);
            await db.SaveChangesAsync();

            var response = JsonConvert.SerializeObject(
                new EventUzivatelUpdated()
                {
                    ParentId = model.Id,
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
            await _publisher.Push(response);
        }

 
    }
}
