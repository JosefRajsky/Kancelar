using System;
using System.Collections.Generic;
using System.Text;

namespace EventLibrary
{
    public class MainMessage{
        public Guid Guid { get; set; }
        public MessageType MessageType { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
        public Guid? ParentGuid { get; set; }
        public string Message { get; set; }
    }
    public class Event : MainMessage
    {
        public string Odesilatel { get; set; }
    }
    public class Command : MainMessage
    {
        public string Puvodce { get; set; }
        public string Poznamka { get; set; }
        
    }
}
