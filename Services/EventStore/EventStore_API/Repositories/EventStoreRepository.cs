

using CommandHandler;
using EventLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace EventStore_Api.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly EventStoreDbContext db;
        private Publisher _publisher;
        public EventStoreRepository(EventStoreDbContext dbContext, Publisher publisher)
        {
            db = dbContext;
            _publisher = publisher;
        }
        public async Task<List<StoreMessage>> GetListByDate(DateTime datum)
        {
            return await Task.Run(() => db.Messages.Where(m => m.Created > datum).ToList());
        }
        public async Task AddMessageAsync(string msg)
        {
            var origin = JsonConvert.DeserializeObject<Message>( msg);
            var message = new StoreMessage();
            message.Guid = Guid.NewGuid();
            message.ParentGuid = origin.ParentGuid;
            message.MessageType = origin.MessageType;
            message.MessageTypeText = origin.MessageType.ToString();
            message.Created = origin.Created;
            message.Generation = origin.Generation;
            message.EntityId = origin.EntityId;
            message.Event = msg;
            message.Command = origin.Command;
            db.Messages.Add(message);
            await db.SaveChangesAsync();
        }
        public async Task<StoreMessage> Get(string guid)
        {
            return await Task.Run(() => db.Messages.FirstOrDefault(m => m.Guid == Guid.Parse(guid)));
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<StoreMessage>> GetList()
        {
            return await db.Messages.ToListAsync();
        }

        public void ServiceHeal(string message)
        {
            
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            var ev = JsonConvert.DeserializeObject<EventServiceReady>(envelope.Event); 
           
            foreach (var type in ev.MessageTypes)
            {
                var storeMessages = db.Messages.Where(m =>  m.MessageType == type).OrderBy(d => d.Created);
                foreach (var msg in storeMessages)
                {
                    _publisher.PushToExchange(ev.Exchange, msg.Event);
                }
            }
            //AddMessageAsync(message);


        }

    }
}
