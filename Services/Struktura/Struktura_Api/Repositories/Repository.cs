
using CommandHandler;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Struktura_Api.Repositories
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
            var item = db.Struktury.FirstOrDefault(u => u.StrukturaId == entityId);
            if (item != null)
            {
                if (item.EventGuid != eventId) await RequestEvents(entityId);
            }
        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.SoucastCreated);
            msgTypes.Add(MessageType.SoucastUpdated);
            msgTypes.Add(MessageType.SoucastRemoved);
            msgTypes.Add(MessageType.UzivatelCreated);
            msgTypes.Add(MessageType.UzivatelUpdated);
            msgTypes.Add(MessageType.UzivatelRemoved);

            await _handler.RequestReplay("Struktura.ex", entityId, msgTypes);
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
                    case MessageType.SoucastCreated:
                        break;
                    case MessageType.SoucastRemoved:
                        break;
                    case MessageType.SoucastUpdated:
                        break;
                    case MessageType.UzivatelCreated:
                        break;
                    case MessageType.UzivatelRemoved:
                        break;
                    case MessageType.UzivatelUpdated:
                        break;
                }
            }
            await db.SaveChangesAsync();
        }
        private Struktura Create(EventStrukturaCreated evt)
        {
            var model = new Struktura()
            {
                Generation = evt.Generation,
                EventGuid = evt.EventId,
                StrukturaId = evt.StrukturaId,
                DatumAktualizace = DateTime.Now,
                Nazev = evt.Nazev,
                SoucastId = evt.SoucastId,
                Clenove = evt.Clenove,
                Zkratka = evt.Zkratka,
            };
            return model;
        }
        private Struktura Modify(EventStrukturaUpdated evt, Struktura item)
        {
            item.EventGuid = evt.EventId;

            return item;
        }

        public async Task<Struktura> Get(Guid id) => await Task.Run(() => db.Struktury.FirstOrDefault(b => b.StrukturaId == id));
        public async Task<List<Struktura>> GetList() => await db.Struktury.ToListAsync();

        //public async Task Add(CommandStrukturaCreate cmd)
        //{
        //    var ev = new EventStrukturaCreated()
        //    {
        //        EventId = Guid.NewGuid(),                           
        //        Generation = 0,
        //        StrukturaId = Guid.NewGuid(),
        //    };              
        //        var item = Create(ev);
        //        db.Struktury.Add(item);
        //        await db.SaveChangesAsync();                
        //        await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.StrukturaId);

        //}
        //public async Task Update(CommandStrukturaUpdate cmd)
        //{
        //    var item = db.Struktury.FirstOrDefault(u => u.StrukturaId == cmd.StrukturaId);                   
        //    if (item != null) {
        //        var ev = new EventStrukturaUpdated()
        //        {
        //            EventId = Guid.NewGuid(),
        //            StrukturaValue1 = cmd.StrukturaValue1,
        //            StrukturaValue2 = cmd.StrukturaValue2,

        //        };
        //        ev.Generation = item.Generation + 1;
        //        item = Modify(ev, item);
        //        await _handler.PublishEvent(ev, MessageType.StrukturaUpdated, ev.EventId, item.EventGuid, ev.Generation, cmd.StrukturaId);
        //        db.Struktury.Update(item);
        //        await db.SaveChangesAsync();
        //    }
        //}
        //public async Task Remove(CommandStrukturaRemove cmd)
        //{
        //    var remove = db.Struktury.FirstOrDefault(u => u.StrukturaId == cmd.StrukturaId);
        //    if (remove != null) {

        //        var ev = new EventStrukturaRemoved()
        //        {
        //            Generation = remove.Generation + 1,
        //            EventId = Guid.NewGuid(),
        //            StrukturaId = cmd.StrukturaId,
        //        };
        //        db.Struktury.Remove(remove);
        //        await _handler.PublishEvent(ev, MessageType.StrukturaRemoved, ev.EventId, remove.EventGuid, remove.Generation, remove.StrukturaId);
        //        await db.SaveChangesAsync();
        //    }

        //}

        public async Task CreateBySoucast(EventSoucastCreated evt)
        {
            var ev = new EventStrukturaCreated()
            {
                Zkratka = evt.Zkratka,
                Clenove = string.Empty,
                SoucastId = evt.SoucastId,
                Nazev = evt.Nazev,
                DatumVytvoreni = DateTime.Now,
                EventId = evt.EventId,
                Generation = evt.Generation,
                ParentId = evt.ParentId,
                StrukturaId = Guid.NewGuid()
            };
            var item = Create(ev);
            db.Struktury.Add(item);
            await db.SaveChangesAsync();
            await _handler.PublishEvent(ev, MessageType.StrukturaCreated, ev.EventId, null, ev.Generation, item.StrukturaId);
        }

        public async Task UpdateBySoucast(EventSoucastUpdated evt)
        {
            var struktury = db.Struktury.Where(s => s.SoucastId == evt.SoucastId);

            if (struktury.Any())
            {
                foreach (var item in struktury)
                {
                    var ev = new EventStrukturaUpdated()
                    {
                        Zkratka = evt.Zkratka,
                        Clenove = string.Empty,
                        SoucastId = evt.SoucastId,
                        Nazev = evt.Nazev,
                        DatumAktualizace = DateTime.Now,
                        EventId = evt.EventId,
                        Generation = evt.Generation,
                        ParentId = evt.ParentId,
                        StrukturaId = Guid.NewGuid()
                    };
                    var struktura = Modify(ev, item);
                    db.Struktury.Update(struktura);
                    await db.SaveChangesAsync();
                    await _handler.PublishEvent(ev, MessageType.StrukturaUpdated, ev.EventId, null, ev.Generation, struktura.StrukturaId);
                }
            }

        }

        public async Task DeleteBySoucast(EventSoucastRemoved evt)
        {
            var struktury = db.Struktury.Where(s => s.SoucastId == evt.SoucastId);

            if (struktury.Any())
            {
                foreach (var item in struktury)
                {
                    await DeleteStruktura(item);
                }
            }
            await db.SaveChangesAsync();
           
        }
        public async Task DeleteStruktura(Struktura struktura) {

            var childs = db.Struktury.Where(c => c.ParentStrukturaId == struktura.StrukturaId);
            foreach (var item in childs)
            {
                await DeleteStruktura(item);
            }

            var ev = new EventStrukturaRemoved()
            {
                EventId = Guid.NewGuid(),
                StrukturaId = struktura.StrukturaId,
                Generation = struktura.Generation + 1
            };
               await _handler.PublishEvent(ev, MessageType.StrukturaRemoved, ev.EventId, null, ev.Generation, struktura.StrukturaId);
            db.Struktury.Remove(struktura);
         
        }


        public Task CreateByUzivatel(EventUzivatelCreated evt)
        {
        
            return null;
        }

        public Task UpdateByUzivatel(EventUzivatelUpdated evt)
        {
            return null;
        }

        public Task DeleteByUzivatel(EventUzivatelRemoved evt)
        {
            return null;
        }
    }

}








