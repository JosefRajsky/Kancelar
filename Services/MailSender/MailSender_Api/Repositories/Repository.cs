
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MailSender_Api.Repositories
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
            var item = db.Mails.FirstOrDefault(u => u.MailId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.MailSenderCreated);
            msgTypes.Add(MessageType.MailSenderUpdated);
            msgTypes.Add(MessageType.MailSenderRemoved);
            await _handler.RequestReplay("MailSenderlate.ex", entityId, msgTypes);           
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
                        var create = JsonConvert.DeserializeObject<EventMailSenderCreated>(msg.Event);
                        var forCreate = db.Mails.FirstOrDefault(u => u.MailId == create.MailSenderId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Mails.Add(forCreate);
                            db.SaveChanges();
                        }
                        
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventMailSenderDeleted>(msg.Event);
                        var forRemove = db.Mails.FirstOrDefault(u => u.MailId == remove.MailSenderId);
                        if (forRemove != null) db.Mails.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventMailSenderUpdated>(msg.Event);
                        var forUpdate = db.Mails.FirstOrDefault(u => u.MailId == update.MailSenderId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update,forUpdate);
                            db.Mails.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Mail Create(EventMailSenderCreated evt)
        {
            var model = new Mail()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                MailId = evt.MailSenderId,
                Value1 = evt.MailSenderValue1,
                Value2 = evt.MailSenderValue2
            };
            return model;
        }
        private Mail Modify(EventMailSenderUpdated evt, Mail item)
        {           
            item.EventGuid = evt.EventId;
            item.Value1 = evt.MailSenderValue1;
            item.Value2 = evt.MailSenderValue2;
            return item;
        }

        public async Task<Mail> Get(Guid id) => await Task.Run(() => db.Mails.FirstOrDefault(b => b.MailId == id));
        public async Task<List<Mail>> GetList() => await db.Mails.ToListAsync();
        public async Task Add(CommandMailSenderCreate cmd)
        {
            var ev = new EventMailSenderCreated()
            {
                EventId = Guid.NewGuid(),                           
                Generation = 0,
                MailSenderId = Guid.NewGuid(),
            };              
                var item = Create(ev);
                db.Mails.Add(item);
                await db.SaveChangesAsync();                
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.MailId);
            
        }
        public async Task Update(CommandMailSenderUpdate cmd)
        {
            var item = db.Mails.FirstOrDefault(u => u.MailId == cmd.MailSenderId);                   
            if (item != null) {
                var ev = new EventMailSenderUpdated()
                {
                    EventId = Guid.NewGuid(),
                    MailSenderValue1 = cmd.MailSenderValue1,
                    MailSenderValue2 = cmd.MailSenderValue2,

                };
                ev.Generation = item.Generation + 1;
                item = Modify(ev, item);
                await _handler.PublishEvent(ev, MessageType.MailSenderUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.MailSenderId);
                db.Mails.Update(item);
                await db.SaveChangesAsync();
            }
        }
        public async Task Remove(CommandMailSenderRemove cmd)
        {
            var remove = db.Mails.FirstOrDefault(u => u.MailId == cmd.MailSenderId);
            if (remove != null) {
                
                var ev = new EventMailSenderDeleted()
                {
                    Generation = remove.Generation + 1,
                    EventId = Guid.NewGuid(),
                    MailSenderId = cmd.MailSenderId,
                };
                db.Mails.Remove(remove);
                await _handler.PublishEvent(ev, MessageType.MailSenderRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.MailId);
                await db.SaveChangesAsync();
            }

        }


    }

}








