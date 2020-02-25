using System;
using System.Collections.Generic;
using System.Text;

namespace EventLibrary
{
    public class Event
    {
        public Guid Guid { get; set; }
        public MessageType MessageType { get; set; }
        public int Version { get; set; }
        public DateTime Vytvoren { get; set; }
        public Guid? ParentGuid { get; set; }
        public string Message { get; set; }
    }
    public class Command
    {
        public Guid Guid { get; set; }
        public MessageType MessageType { get; set; }
        public int Version { get; set; }
        public DateTime Vytvoren { get; set; }
        public Guid? ParentGuid { get; set; }
        public string Message { get; set; }
    }
}
