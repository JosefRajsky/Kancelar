using System;
using static EventLibrary.EventEnum;

namespace EventLibrary
{
    public class EventDochazkaCreate : EventBase
    {
        public EventDochazkaCreate()
        {
            MessageType = MessageType.DochazkaCreate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId {get;set;}


    }
    public class EventDochazkaUpdate : EventBase
    {
        public EventDochazkaUpdate()
        {
            MessageType = MessageType.DochazkaUpdate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int DochazkaId { get; set; }
        public int UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId { get; set; }


    }
    public class EventDochazkaRemove : EventBase
    {
        public EventDochazkaRemove()
        {
            MessageType = MessageType.DochazkaRemove;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int DochazkaId { get; set; }


    }
}
