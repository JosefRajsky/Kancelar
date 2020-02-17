using System;
using static EventLibrary.MessageType;

namespace EventLibrary
{
    public class EventDochazkaCreated : Base
    {
        public EventDochazkaCreated()
        {
            MessageType = EventLibrary.MessageType.DochazkaCreate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId {get;set;}


    }
    public class EventDochazkaUpdated : Base
    {
        public EventDochazkaUpdated()
        {
            MessageType = EventLibrary.MessageType.DochazkaUpdate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int DochazkaId { get; set; }
        public int UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId { get; set; }


    }
    public class EventDochazkaDeleted : Base
    {
        public EventDochazkaDeleted()
        {
            MessageType = MessageType.DochazkaRemove;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int DochazkaId { get; set; }


    }
}
