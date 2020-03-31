using System;
using System.Collections.Generic;
using System.Text;

namespace CommandHandler
{
    public class Message {
        public Guid Guid { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime Created { get; set; }
        public Guid? ParentGuid { get; set; }
        public Guid EntityId { get; set; }
        public int Generation { get; set; }
        public string Event { get; set; }
    }
}
