
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
          
        public async Task ImportUzivatel(List<CommandUzivatelCreate> cmds)
        {
            foreach (var cmd in cmds)
            {
                var client = new System.Net.Http.HttpClient();
               
                await _handler.PublishEvent(, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, item.TransferId);

                var request = new System.Net.Http.HttpRequestMessage();
                var content = new System.Net.Http.StringContent(JsonConvert.SerializeObject(cmd, _settings.Value));
                content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                request.Content = content;
                request.Method = new System.Net.Http.HttpMethod("POST");
                PrepareRequest(client, request, urlBuilder);
                var url = urlBuilder.ToString();
                request.RequestUri = new System.Uri(url, RelativeOrAbsolute);
                PrepareRequest(client, request, url);
                var response_ = await client.SendAsync(request, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

            }
            var ev = new EventTransferCreated()
            {
                EventId = Guid.NewGuid(),                           
                Generation = 0,
                TransferId = Guid.NewGuid(),
            }; db.Transfers.Add(item);
            await db.SaveChangesAsync();



        }
      


    }

}








