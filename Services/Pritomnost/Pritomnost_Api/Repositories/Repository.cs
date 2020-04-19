
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
        private readonly PritomnostDbContext db;
        private Publisher _publisher;
        private MessageHandler _handler;
        public Repository(PritomnostDbContext dbContext, Publisher publisher)
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
                    item.Generation = item.Generation + 1;
                    await db.SaveChangesAsync();
                }
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.UzivatelCreated);
            msgTypes.Add(MessageType.UzivatelUpdated);
            msgTypes.Add(MessageType.UzivatelRemoved);
            await _handler.RequestReplay("pritomnost.ex", entityId, msgTypes);
        }

        private async Task<Pritomnost> Create(EventUzivatelCreated evt)
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
                kalendar = await Create(evt);
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
