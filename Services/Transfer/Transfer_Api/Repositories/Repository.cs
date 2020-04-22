
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfer_Api.Repositories
{
    public class Repository : IRepository
    {
        private readonly ServiceDbContext db;
        private MessageHandler _handler;
        public Repository(ServiceDbContext dbContext, Publisher publisher)
        {
            db = dbContext;          
            _handler = new MessageHandler(publisher);

        }
        public async Task LastEventCheck(Guid eventId, Guid entityId)
        {
            var item = db.Transfers.FirstOrDefault(u => u.TransferId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.TransferCreated);
            msgTypes.Add(MessageType.TransferUpdated);
            msgTypes.Add(MessageType.TransferRemoved);
            await _handler.RequestReplay("transfer.ex", entityId, msgTypes);           
        }
        public async Task ReplayEvents(List<string> stream, Guid? entityId)
        {
            var messages = new List<Message>();
            foreach (var item in stream)
            {
                messages.Add(JsonConvert.DeserializeObject<Message>(item));
            }
            var replayOrderedStream = messages.OrderBy(d => d.Created);
            foreach (var msg in replayOrderedStream)
            {
                switch (msg.MessageType)
                {
                    case MessageType.UzivatelCreated:
                        var create = JsonConvert.DeserializeObject<EventTransferCreated>(msg.Event);
                        var forCreate = db.Transfers.FirstOrDefault(u => u.TransferId == create.TransferId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Transfers.Add(forCreate);
                            db.SaveChanges();
                        }
                        
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventTransferDeleted>(msg.Event);
                        var forRemove = db.Transfers.FirstOrDefault(u => u.TransferId == remove.TransferId);
                        if (forRemove != null) db.Transfers.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventTransferUpdated>(msg.Event);
                        var forUpdate = db.Transfers.FirstOrDefault(u => u.TransferId == update.TransferId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update,forUpdate);
                            db.Transfers.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Transfer Create(EventTransferCreated evt)
        {
            var model = new Transfer()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                TransferId = evt.TransferId,
                Value1 = evt.Value1,
                Value2 = evt.Value2
            };
            return model;
        }
        private Transfer Modify(EventTransferUpdated evt, Transfer item)
        {           
            item.EventGuid = evt.EventId;
            item.Value1 = evt.Value1;
            item.Value2 = evt.Value2;
            return item;
        }

        public async Task<Transfer> Get(Guid id) => await Task.Run(() => db.Transfers.FirstOrDefault(b => b.TransferId == id));
        public async Task<List<Transfer>> GetList() => await db.Transfers.ToListAsync();
        public async Task Add(CommandTransferCreate cmd)
        {
            var ev = new EventTransferCreated()
            {
                EventId = Guid.NewGuid(),                           
                Generation = 0,
                TransferId = Guid.NewGuid(),
            };              
                var item = Create(ev);
                db.Transfers.Add(item);
                await db.SaveChangesAsync();                
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.TransferId);
            
        }
        public async Task Update(CommandTransferUpdate cmd)
        {
            var item = db.Transfers.FirstOrDefault(u => u.TransferId == cmd.TransferId);                   
            if (item != null) {
                var ev = new EventTransferUpdated()
                {
                    EventId = Guid.NewGuid(),
                    Value1 = cmd.Value1,
                    Value2 = cmd.Value2,

                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.TransferUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.TransferId);
                db.Transfers.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandTransferRemove cmd)
        {
            var remove = db.Transfers.FirstOrDefault(u => u.TransferId == cmd.TransferId);
            if (remove != null) {
                
                var ev = new EventTransferDeleted()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    TransferId = cmd.TransferId,
                };
                db.Transfers.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.TransferRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.TransferId);
                await db.SaveChangesAsync();
            }

        }


    }

}








