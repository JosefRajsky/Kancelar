using EventLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EventStore_Service
{

    public class EventStoreDbContext : DbContext
    {
        public EventStoreDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Message> Messages { get; set; }
    }
    public class Message {
        [Key]
        public Guid Guid { get; set; }
        public Guid CurrentGuid { get; set; }
        public Guid? ParentGuid { get; set; }
        public MessageType MessageType { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }      
        public string Body { get; set; }
    }

}

