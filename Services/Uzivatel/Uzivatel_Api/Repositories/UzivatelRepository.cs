
using CommandHandler;

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

        public async Task RequestReplay(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.UzivatelCreated);
            msgTypes.Add(MessageType.UzivatelUpdated);
            msgTypes.Add(MessageType.UzivatelRemoved);
            var evt = new ProvideHealingStream() { Exchange = "uzivatel.ex", MessageTypes = msgTypes, EntityId = entityId };
            var msg = new Message()
            {
                Guid = Guid.NewGuid(),
                MessageType = MessageType.ProvideHealingStream,
                Created = DateTime.Now,
                EntityId = (entityId == Guid.Empty) ? Guid.Empty : Guid.Parse(entityId.ToString()),
                ParentGuid = null,
                Event = await Task.Run(() => JsonConvert.SerializeObject(evt))
            };
            await _publisher.Push(JsonConvert.SerializeObject(msg));
        }
        public async Task ReplayStream(List<string> msgStream, Guid? entityId)
        {
            var msgList = new List<Message>();
            foreach (var item in msgStream)
            {
                msgList.Add(JsonConvert.DeserializeObject<Message>(item));
            }
            foreach (var msg in msgList.OrderBy(g => g.Generation))
            {
                switch (msg.MessageType)
                {
                    case MessageType.UzivatelCreated:
                        var create = JsonConvert.DeserializeObject<EventUzivatelCreated>(msg.Event);
                        var forCreate = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == create.UzivatelId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                        }
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventUzivatelDeleted>(msg.Event);
                        var forRemove = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == remove.UzivatelId);
                        if(forRemove != null)  db.Uzivatele.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventUzivatelUpdated>(msg.Event);
                        var forUpdate = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == update.UzivatelId);
                        if (forUpdate == null)
                        {
                            forUpdate = Modify(update);
                        }
                        break;
                }
            }            
        }

        public async Task<Uzivatel> Get(Guid id) => await Task.Run(() => db.Uzivatele.FirstOrDefault(b => b.UzivatelId == id));
        public async Task<List<Uzivatel>> GetList() => await db.Uzivatele.ToListAsync();
        public async Task ConfirmAdd(EventUzivatelCreated evt, Guid entityId)
        {
            var model = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == entityId);
            if (model.Generation == evt.Generation - 1)
            {
                model.EventGuid = evt.EventId;
                model.Generation = evt.Generation;
                db.Uzivatele.Update(model);
                await db.SaveChangesAsync();
            }
            else
            {
                await RequestReplay(entityId);
            }

        }
        public async Task ConfirmUpdate(EventUzivatelUpdated evt, Guid entityId)
        {
            var model = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == evt.UzivatelId);
            if (model.Generation == evt.Generation - 1)
            {
                model.EventGuid = evt.EventId;
                model.Generation = evt.Generation;
                db.Uzivatele.Update(model);
                await db.SaveChangesAsync();
            }
            else
            {
                await RequestReplay(entityId);
            }
        }
        public async Task Add(CommandUzivatelCreate cmd)
        {
            var ev = new EventUzivatelCreated()
            {
                EventId = Guid.NewGuid(),
                UzivatelId = Guid.NewGuid(),
                TitulPred = cmd.TitulPred,
                Jmeno = cmd.Jmeno,
                Prijmeni = cmd.Prijmeni,
                TitulZa = cmd.TitulZa,
                Pohlavi = cmd.Pohlavi,
                DatumNarozeni = cmd.DatumNarozeni,
                Email = cmd.Email,
                Telefon = cmd.Telefon,
                Generation = 0,
            };
            var model = Create(ev);           
            if (model != null)
            {
                ev.Generation = model.Generation + 1;
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, model.UzivatelId);
            }
        }
        public async Task Update(CommandUzivatelUpdate cmd)
        {
            var ev = new EventUzivatelUpdated()
            {
                EventId = Guid.NewGuid(),
                UzivatelId = cmd.UzivatelId,
                TitulPred = cmd.TitulPred,
                Jmeno = cmd.Jmeno,
                Prijmeni = cmd.Prijmeni,
                TitulZa = cmd.TitulZa,
                Pohlavi = cmd.Pohlavi,
                DatumNarozeni = cmd.DatumNarozeni,
                Email = cmd.Email,
                Telefon = cmd.Telefon,
            };
            var model =Modify(ev);            
            if (model != null) {
             
                ev.Generation = model.Generation + 1;
                await _handler.PublishEvent(ev, MessageType.UzivatelUpdated, ev.EventId, model.EventGuid, ev.Generation, model.UzivatelId);
            }
        }
        public async Task Remove(CommandUzivatelRemove cmd)
        {
            var remove = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == cmd.UzivatelId);
            db.Uzivatele.Remove(remove);
            var ev = new EventUzivatelDeleted()
            {
                Generation = remove.Generation + 1,
                EventId = Guid.NewGuid(),
                UzivatelId = cmd.UzivatelId,
            };
            await _handler.PublishEvent(ev, MessageType.UzivatelRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.UzivatelId);
            await db.SaveChangesAsync();
        }

        private Uzivatel Create(EventUzivatelCreated evt)
        {
            var model = new Uzivatel()
            {
                UzivatelId = evt.UzivatelId,
                TitulPred = evt.TitulPred,
                Jmeno = evt.Jmeno,
                Prijmeni = evt.Prijmeni,
                TitulZa = evt.TitulZa,
                Pohlavi = evt.Pohlavi,
                DatumNarozeni = evt.DatumNarozeni,
                Email = evt.Email,
                Telefon = evt.Telefon,
                Generation = evt.Generation
            };
            db.Uzivatele.Add(model);
            db.SaveChanges();
            return model;
        }
        private Uzivatel Modify(EventUzivatelUpdated evt)
        {
            var model = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == evt.UzivatelId);
            model.TitulPred = evt.TitulPred;
            model.Jmeno = evt.Jmeno;
            model.Prijmeni = evt.Prijmeni;
            model.TitulZa = evt.TitulZa;
            model.Pohlavi = evt.Pohlavi;
            model.DatumNarozeni = evt.DatumNarozeni;
            model.Email = evt.Email;
            model.Telefon = evt.Telefon;
            db.Uzivatele.Update(model);
            db.SaveChanges();
            return model;
        }

    }

}








