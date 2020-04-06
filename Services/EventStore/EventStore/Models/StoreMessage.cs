
using CommandHandler;
using System;
using System.ComponentModel.DataAnnotations;

namespace EventStore
{
    public class StoreMessage {
        [Key] 
        public Guid Guid { get; set; }
        public MessageType MessageType { get; set; }
        public string MessageTypeText { get; set; }
        public DateTime Created { get; set; }
        public Guid EntityId { get; set; }
        public string Message { get; set; }
    
    }

}

