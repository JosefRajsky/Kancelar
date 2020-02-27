

using EventLibrary;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace EventStore_Service.Repositories
{
    public class EventStoreRepository
    {
        private string _connectionString;
        private EventStoreDbContext _db;
        public EventStoreRepository(string connectionString)
        {
            _connectionString = connectionString;
        }       

        public async Task AddMessageAsync(string msg)
        {
            var origin = JsonConvert.DeserializeObject<MainMessage>(msg);
            var message = new Message();
            message.Guid = Guid.NewGuid();
            message.CurrentGuid = origin.Guid;
            message.TopicId = 0;
            message.ParentGuid = origin.ParentGuid;
            message.MessageType = origin.MessageType;
            message.MessageTypeText = origin.MessageType.ToString();
            message.Version = origin.Version;
            message.Created = origin.Created;
            message.Body = origin.Message;
       
            await Task.Run(()=> _db = new EventStoreDbContextFactory(_connectionString).CreateDbContext());

            //using (var db = new EventStoreDbContextFactory(_connectionString).CreateDbContext())
            //{               
                _db.Messages.Add(message);
                await _db.SaveChangesAsync();
                _db.Dispose();
            //}
        }
    }
}
