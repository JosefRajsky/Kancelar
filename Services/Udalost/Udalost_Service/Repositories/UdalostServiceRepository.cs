
using CommandHandler;
using Dochazka_Service.Repositories;
using EventLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Service.Entities;

namespace Udalost_Service.Repositories
{
    public class UdalostServiceRepository : IUdalostServiceRepository
    {
        private string _connectionString;
        private ConnectionFactory factory;
        private Publisher publisher;


        public UdalostServiceRepository(string connectionString)
        {
            _connectionString = connectionString;
            factory = new ConnectionFactory() { HostName = "rabbitmq" };
            publisher = new Publisher(factory, config.GetValue<string>("Setting:Exchange"));
        }
        public async Task AddCommand(string message)
        {
            //-------------Description: Deserializace Json objektu na základní typ zprávy
            var envelope = JsonConvert.DeserializeObject<Base>(message);
            //-------------Description: Rozhodnutí o typu získazné zprávy. Typ vázaný na Enum z knihovny
            switch (envelope.MessageType)
            {
                case MessageType.UdalostCreate:
                    //-------------Description: Kontrola verze zprávy 
                    if (envelope.Version == 1)
                    {
                        //-------------Description: Deserializace zprávy do správného typu a odeslání k uložení do DB; 
                        await this.Add(JsonConvert.DeserializeObject<CommandUdalostCreate>(message));
                    }
                    break;
                case MessageType.UdalostRemove:
                    if (envelope.Version == 1)
                    {
                        await this.Remove(JsonConvert.DeserializeObject<CommandUdalostRemove>(message));
                    }
                    break;
               case MessageType.UdalostUpdate:
                    if (envelope.Version == 1)
                    {
                        await this.Update(JsonConvert.DeserializeObject<CommandUdalostUpdate>(message));
                    }
                    break;
      
            }
        }
        public async Task Add(CommandUdalostCreate msg)
        {
            using (var db = new UdalostFactory(_connectionString).CreateDbContext())
            {
                var add = new Udalost()
                {
                    DatumOd = msg.DatumOd,
                    DatumDo = msg.DatumDo,        
                    UdalostTypId = msg.UdalostTypId,
                    Popis = msg.Popis,
                    UzivatelId = msg.UzivatelId,
                    DatumZadal = msg.DatumZadal,
                };                
                db.Add(add);
                var result = db.SaveChangesAsync().IsCompletedSuccessfully;
                if (result) {
                    var body = JsonConvert.SerializeObject(
                     new EventUdalostCreated()
                     {
                         Id = add.Id,
                         DatumOd = add.DatumOd,
                         DatumDo = add.DatumDo,
                         UdalostTypId = add.UdalostTypId,
                         Popis = add.Popis,
                         UzivatelId = add.UzivatelId,
                         DatumZadal = msg.DatumZadal,
                     }); ;
                    await publisher.Push(body);
                }

            }          
        }
        public async Task Remove(CommandUdalostRemove msg)
        {
            using (var db = new UdalostFactory(_connectionString).CreateDbContext())
            {
                var remove = db.Udalosti.FirstOrDefault(b => b.Id == msg.UdalostId);
                if (remove != null) {
                    db.Udalosti.Remove(remove);
                    var result = db.SaveChangesAsync().IsCompletedSuccessfully;
                    if (result)
                    {
                        var body = JsonConvert.SerializeObject(
                         new EventUdalostCreated()
                         {
                             Id = remove.Id,
                         }); ;
                        await publisher.Push(body);
                    }
                } 
            }
        }
        public async Task Update(CommandUdalostUpdate msg)
        {
            using (var db = new UdalostFactory(_connectionString).CreateDbContext())
            {
                var update = db.Udalosti.FirstOrDefault(b => b.Id == msg.UdalostId);
                update.DatumOd = msg.DatumOd;
                update.DatumDo = msg.DatumDo;
                update.UdalostTypId = msg.UdalostTypId;
                update.Popis = msg.Popis;
                update.UzivatelId = msg.UzivatelId;
                update.DatumZadal = msg.DatumZadal;
                db.Udalosti.Update(update);

                var result = db.SaveChangesAsync().IsCompletedSuccessfully ;
                if (result)
                {
                    var body = JsonConvert.SerializeObject(
                     new EventUdalostCreated()
                     {
                         Id = update.Id,
                         DatumOd = update.DatumOd,
                         DatumDo = update.DatumDo,
                         UdalostTypId = update.UdalostTypId,
                         Popis = update.Popis,
                         UzivatelId = update.UzivatelId,
                         DatumZadal = update.DatumZadal,
                     }); ;
                    await publisher.Push(body);
                }
            }

        }

        //public Udalost Get(int id)
        //{
        //    using (var db = new UdalostFactory(_connectionString).CreateDbContext())
        //    {
        //        return db.Udalosti.FirstOrDefault(b => b.Id == id);
        //    }
        //}
        //public IEnumerable<Udalost> GetList()
        //{
        //    using (var db = new UdalostFactory(_connectionString).CreateDbContext())
        //    {
        //        return db.Udalosti;
        //    }            
        //}



    }
}
