using System;
using static EventLibrary.EventEnum;

namespace EventLibrary
{
    public class EventDochazkaCreate : EventBase
    {
        public int UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId {get;set;}

        public EventDochazkaCreate()
        {
            MessageType = MessageType.DochazkaCreate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
    }
}
