using EventLibrary;
using System;
using System.ComponentModel.DataAnnotations;

namespace EventStore_Api
{
    public class StoreMessage {
        [Key] 
        public Guid Guid { get; set; }
        public MessageType MessageType { get; set; }
        public string MessageTypeText { get; set; }
        public DateTime Created { get; set; }
        public Guid? ParentGuid { get; set; }
        public Guid EntityId { get; set; }
        public int Generation { get; set; }
        public string Event { get; set; }
        public string Command { get; set; }
    
    }

}

