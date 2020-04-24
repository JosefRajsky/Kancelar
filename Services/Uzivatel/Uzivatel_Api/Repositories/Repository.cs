
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
        private readonly ServiceDbContext db; 
        private readonly MessageHandler _handler;

        //Description: Vytvoření repositáře. Přiřazení databáze a objektu s metodami pro Publikaci
        public Repository(ServiceDbContext dbContext, Publisher publisher)
        {
            db = dbContext;           
            _handler = new MessageHandler(publisher);
        }
        //Description: Ověření aktuálního stavu entity po uložení a publikaci
        public async Task LastEventCheck(Guid eventId, Guid entityId)
        {
            //Description: Nalezení záznamu
            var item = db.Uzivatele.FirstOrDefault(u => u.UzivatelId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId)
                {
                    //Description: Pokud nesouhlasi ID události, vyžádej obnovu entity
                    await RequestEvents(entityId);
                }
                else {
                    //Description: Potvrzení úravy entity
                    item.Generation += 1;
                   await db.SaveChangesAsync();
                }
            }
        }
        //Description: Metoda pro vyžádání obnovy u zájmových událostí
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>
            {
                MessageType.UzivatelCreated,
                MessageType.UzivatelUpdated,
                MessageType.UzivatelRemoved
            };
            //Description: Publikace požadavku na Obnovu. Určení na který exchange a kterou entitu podle ID
            await _handler.RequestReplay("uzivatel.ex", entityId, msgTypes);           
        }
        //Description: Reakce na přijatou obnovovací sekvenci událostí. Může být určen jen pro entitu
        public async Task ReplayEvents(List<string> stream, Guid? entityId)
        {
            //Description: Deserializace všech zpráv z Json
            var messages = new List<Message>();
            foreach (var item in stream)
            {
                messages.Add(JsonConvert.DeserializeObject<Message>(item));
            }
            //Description: Provedení setřídění podle data vytvoření
            var replayOrderedStream = messages.OrderBy(d => d.Created);
            //Description: Postupné zpracování zpráv
            foreach (var msg in replayOrderedStream)
            {
                //Description: Reakce na zprávy podle typu události
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
        //Description: pomocná metoda na vytvoření záznamu na základě události
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
        //Description: Pomocná metoda na úpravu záznamu na základě události
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
        //Description: Načtení uživatele podle ID
        public async Task<Uzivatel> Get(Guid id) => await Task.Run(() => db.Uzivatele.FirstOrDefault(b => b.UzivatelId == id));
        //Description: Načtení listu všech uživatelů
        public async Task<List<Uzivatel>> GetList() => await db.Uzivatele.ToListAsync();
        //Description: Přidání uživatele, příkaz
        public async Task Add(CommandUzivatelCreate cmd)
        {
            //Description: Zpracování události na základě obdrženého příkazu
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
            //Description: Vytvoření uživatele
                var item = Create(ev);
            //Description: Přidání uživatele
                db.Uzivatele.Add(item);     
            //Description: Uložení uživatele
                await db.SaveChangesAsync();
            //Description: Přidání Id uživatele do události
                ev.UzivatelId = item.UzivatelId;
            //Description: Přidání generace záznamu do události, zvýšení o stupeň
            //Description: Záznam v DB bude uveden do generace eventu po jeho zpětné konzumaci
                ev.Generation += 1;
            //Description: Publikace události o vytvoření uživatele
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








