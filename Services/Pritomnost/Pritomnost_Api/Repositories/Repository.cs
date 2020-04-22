
using CommandHandler;



using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pritomnost_Api.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Pritomnost_Api.Repositories
{
    public class Repository : IRepository
    {
        private readonly ServiceDbContext db;
        private readonly Publisher _publisher;
        private readonly MessageHandler _handler;
        public Repository(ServiceDbContext dbContext, Publisher publisher)
        {
            db = dbContext;
            _publisher = publisher;
            _handler = new MessageHandler(publisher);
        }
        public async Task LastEventCheck(Guid eventId, Guid entityId)
        {
            var item = db.Pritomnosti.FirstOrDefault(u => u.PritomnostId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId)
                {
                    await RequestEvents(entityId);
                }
                else
                {
                    item.Generation += 1;
                    await db.SaveChangesAsync();
                }
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>
            {
                MessageType.UzivatelCreated,
                MessageType.UzivatelUpdated,
                MessageType.UzivatelRemoved
            };
            await _handler.RequestReplay("pritomnost.ex", entityId, msgTypes);
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        private Pritomnost Create(EventUzivatelCreated evt)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            var model = new Pritomnost()
            {
                PritomnostId = Guid.NewGuid(),
                UzivatelId = evt.UzivatelId,
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                UzivatelCeleJmeno = $"{evt.Prijmeni} {evt.Jmeno}",
            };
            return model;
        }
        private Pritomnost Modify(EventUzivatelUpdated evt, Pritomnost item)
        {
            item.UzivatelId = evt.UzivatelId;
            item.Generation = evt.Generation;
            item.EventGuid = evt.EventId;
            item.UzivatelCeleJmeno = $"{evt.Prijmeni} {evt.Jmeno}";
     
            return item;
        }
        public async Task CreateByUdalost(EventUdalostCreated evt)
        {
         
                    await db.SaveChangesAsync(); 
         
        }
        public async Task UpdateByUdalost(EventUdalostUpdated evt)
        {
          
                    await db.SaveChangesAsync();
           
        }
        public async Task DeleteByUdalost(EventUdalostRemoved evt)
        {
            
            await db.SaveChangesAsync();

        }
        public async Task CreateByUzivatel(EventUzivatelCreated evt)
        {
            var kalendar = db.Pritomnosti.Where(k => k.UzivatelId == evt.UzivatelId).FirstOrDefault();
            if (kalendar == null)
            {
                kalendar = Create(evt);
                db.Pritomnosti.Add(kalendar);
                await db.SaveChangesAsync();
            }
        }
        public async Task UpdateByUzivatel(EventUzivatelUpdated evt)
        {
            var kalendarList = db.Pritomnosti.Where(k => k.UzivatelId == evt.UzivatelId);
            if (kalendarList.Any())
            {
                foreach (var kalendar in kalendarList)
                {
                    kalendar.UzivatelId = evt.UzivatelId;
                    kalendar.Generation = evt.Generation;
                    kalendar.EventGuid = evt.EventId;
                    kalendar.UzivatelCeleJmeno = $"{evt.Prijmeni} {evt.Jmeno}";
                  
                    db.Pritomnosti.Update(kalendar);
                }
            }
            await db.SaveChangesAsync();

        }
        public async Task DeleteByUzivatel(EventUzivatelDeleted evt)
        {
            var forRemove = db.Pritomnosti.Where(k => k.UzivatelId == evt.UzivatelId);
            db.Pritomnosti.RemoveRange(forRemove);
            await db.SaveChangesAsync();
        }
        public async Task<List<Pritomnost>> GetList()
        {
            return await db.Pritomnosti.ToListAsync();
        }
        public async Task<Pritomnost> Get(Guid id) => await Task.Run(() => db.Pritomnosti.FirstOrDefault(b => b.PritomnostId == id));


    }
}
