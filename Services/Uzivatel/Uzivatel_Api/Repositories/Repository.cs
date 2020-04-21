
using CommandHandler;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Uzivatel_Api.Repositories
{
    public class Repository : IRepository
    {
        private readonly UzivatelDbContext db; 
        private MessageHandler _handler;
        public Repository(UzivatelDbContext dbContext, Publisher publisher)
        {
            db = dbContext;           
            _handler = new MessageHandler(publisher);
        }
        public async Task LastEventCheck(Guid eventId, Guid entityId)
        {
            var item = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId)
                {
                    await RequestEvents(entityId);
                }
                else {
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
            await _handler.RequestReplay("uzivatel.ex", entityId, msgTypes);           
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
                        var create = JsonConvert.DeserializeObject<EventUzivatelCreated>(msg.Event);
                        var forCreate = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == create.UzivatelId);
                        if (forCreate == null)
                        {
                            forCreate = Create(create);
                            db.Uzivatele.Add(forCreate);
                            db.SaveChanges();        
                        }
                        break;
                    case MessageType.UzivatelRemoved:
                        var remove = JsonConvert.DeserializeObject<EventUzivatelDeleted>(msg.Event);
                        var forRemove = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == remove.UzivatelId);
                        if (forRemove != null) db.Uzivatele.Remove(forRemove);

                        break;
                    case MessageType.UzivatelUpdated:
                        var update = JsonConvert.DeserializeObject<EventUzivatelUpdated>(msg.Event);
                        var forUpdate = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == update.UzivatelId);
                        if (forUpdate != null)
                        {
                            forUpdate = Modify(update,forUpdate);
                            db.Uzivatele.Update(forUpdate);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Uzivatel Create(EventUzivatelCreated evt)
        {
            var model = new Uzivatel()
            {
                ImportedId = evt.ImportedId,
                UzivatelId = evt.UzivatelId,
                TitulPred = evt.TitulPred,
                Jmeno = evt.Jmeno,
                Prijmeni = evt.Prijmeni,
                TitulZa = evt.TitulZa,
                Pohlavi = evt.Pohlavi,
                DatumNarozeni = evt.DatumNarozeni,
                Email = evt.Email,
                Telefon = evt.Telefon,
                Generation = evt.Generation,
                EventGuid = evt.EventId
            };
            return model;
        }
        private Uzivatel Modify(EventUzivatelUpdated evt, Uzivatel item)
        {
            item.TitulPred = evt.TitulPred;
            item.Jmeno = evt.Jmeno;
            item.Prijmeni = evt.Prijmeni;
            item.TitulZa = evt.TitulZa;
            item.Pohlavi = evt.Pohlavi;
            item.DatumNarozeni = evt.DatumNarozeni;
            item.Email = evt.Email;
            item.Telefon = evt.Telefon;
            item.EventGuid = evt.EventId;
            return item;
        }
        public async Task<Uzivatel> Get(Guid id) => await Task.Run(() => db.Uzivatele.FirstOrDefault(b => b.UzivatelId == id));
        public async Task<List<Uzivatel>> GetList() => await db.Uzivatele.ToListAsync();
        public async Task Add(CommandUzivatelCreate cmd)
        {
            var ev = new EventUzivatelCreated()
            {
                EventId = Guid.NewGuid(),
                UzivatelId = Guid.NewGuid(),                
                EventCreated = DateTime.Now,
                ImportedId = cmd.ImportedId,
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
                var item = Create(ev);
                db.Uzivatele.Add(item);                
                await db.SaveChangesAsync();
                ev.UzivatelId = item.UzivatelId;
                ev.Generation = ev.Generation + 1;
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, ev.UzivatelId);
            
        }
        public async Task Update(CommandUzivatelUpdate cmd)
        {
            var item = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == cmd.UzivatelId);                  
            if (item != null) {
                var ev = new EventUzivatelUpdated()
                {
                    EventId = Guid.NewGuid(),
                    EventCreated = DateTime.Now,
                    UzivatelId = item.UzivatelId,
                    TitulPred = cmd.TitulPred,
                    Jmeno = cmd.Jmeno,
                    Prijmeni = cmd.Prijmeni,
                    TitulZa = cmd.TitulZa,
                    Pohlavi = cmd.Pohlavi,
                    DatumNarozeni = cmd.DatumNarozeni,
                    Email = cmd.Email,
                    Telefon = cmd.Telefon,
                };
                item = Modify(ev, item);
                db.Uzivatele.Update(item);                
                ev.Generation = item.Generation + 1;
                await _handler.PublishEvent(ev, MessageType.UzivatelUpdated, ev.EventId, item.EventGuid, ev.Generation, item.UzivatelId);             
                await db.SaveChangesAsync();
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


    }

}








