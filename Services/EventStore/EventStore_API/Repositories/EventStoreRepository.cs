

using EventLibrary;
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
        private string _connectionString;
        private EventStoreDbContext _db;
        public EventStoreRepository(string connectionString)
        {
            _connectionString = connectionString;
        }       

        public async Task AddMessageAsync(string msg)
        {
            var origin = JsonConvert.DeserializeObject<Message>(msg);
            var message = new Message();
            message.Guid = Guid.NewGuid();
            message.CurrentGuid = origin.Guid;
            message.TopicId = 0;
            message.ParentGuid = origin.ParentGuid;
            message.MessageType = origin.MessageType;
            message.MessageTypeText = origin.MessageType.ToString();
            message.Version = origin.Version;
            message.Created = origin.Created;
            message.Body = origin.Body;
       
            await Task.Run(()=> _db = new EventStoreDbContextFactory(_connectionString).CreateDbContext());                        
                _db.Messages.Add(message);
                await _db.SaveChangesAsync();
                _db.Dispose();
           
        }

        public async Task<Message> Get(string guid)
        {

           return await Task.Run(() => _db.Messages.FirstOrDefault(m => m.Guid == Guid.Parse(guid))) ;
        }

        public async Task<List<Message>> GetList()
        {
            return await _db.Messages.ToListAsync();
        }
    }
}
