

using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace EventStore.Repositories
{
    public class Repository : IRepository
    {
        private readonly ServiceDbContext db;       
        private readonly MessageHandler _handler;
        public Repository(ServiceDbContext dbContext, Publisher publisher)
        {
            db = dbContext;            
            _handler = new MessageHandler(publisher);
        }
        public async Task<List<StoreMessage>> GetListByDate(DateTime datum)
        {
            return await Task.Run(() => db.Messages.Where(m => m.Created > datum).ToList());
        }
        public async Task AddMessageAsync(string msg)
        {
            //Description: Deserializace události jako obecné zprávy a uložení do DB
            var origin = JsonConvert.DeserializeObject<Message>(msg);
            var message = new StoreMessage
            {
                Guid = Guid.NewGuid(),
                MessageType = origin.MessageType,
                MessageTypeText = origin.MessageType.ToString(),
                Created = origin.Created,
                EntityId = origin.EntityId,
                Message = msg
            };
            db.Messages.Add(message);
            await db.SaveChangesAsync();
        }
        public async Task<StoreMessage> Get(string guid)
        {
            return await Task.Run(() => db.Messages.FirstOrDefault(m => m.Guid == Guid.Parse(guid)));
        }
        //Description: Metoda poskytující proud událostí pro obnovu na základě požadavku
        public async Task ProvideHealingStream(string message)
        {
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            var ev = JsonConvert.DeserializeObject<ProvideHealingStream>(envelope.Event);
            //Description: Vytvoření události o poskytnuté obnově
            var responseEvent = new HealingStreamProvided()
            {
                EntityId = ev.EntityId,
                MessageList = new List<string>()
            };
            //Description: Obnova pouze jedné entity
            if (ev.EntityId != Guid.Empty)
            {
                foreach (var type in ev.MessageTypes)
                {
                    responseEvent.MessageList.AddRange(db.Messages.Where(m => m.MessageType == type & m.EntityId == ev.EntityId).Select(m => m.Message));
                }
            }
            //Description: Načtení všech požadovaných typů zpráv
            else
            {
                foreach (var type in ev.MessageTypes)
                {
                    responseEvent.MessageList.AddRange(db.Messages.Where(m => m.MessageType == type).Select(m => m.Message));
                }
            }
            if (responseEvent.MessageList.Any()) {
                var msg = new Message();
                //Description: Publikace obnovovací události
                await _handler.PublishEventToExchange(responseEvent, MessageType.HealingStreamProvided, Guid.NewGuid(), null, 0, (ev.EntityId == Guid.Empty) ? Guid.Empty : Guid.Parse(ev.EntityId.ToString()), ev.Exchange);

            }
        }
    }
}
