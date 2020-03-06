using System;
using System.Collections.Generic;
using System.Text;

namespace EventLibrary
{
    public class Message{
        public Guid Guid { get; set; }
        public MessageType MessageType { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
        public Guid? ParentGuid { get; set; }
        public string Body { get; set; }
    }
}
