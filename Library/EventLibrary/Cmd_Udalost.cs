using System;
using static EventLibrary.EventEnum;

namespace EventLibrary
{
    public class EventUdalostCreate : EventBase
    {
        public EventUdalostCreate()
        {
            MessageType = MessageType.DochazkaCreate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        
        public string Nazev { get; set; }
        public int UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
    }
    public class EventUdalostUpdate : EventBase
    {
        public EventUdalostUpdate()
        {
            MessageType = MessageType.DochazkaUpdate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UdalostId { get; set; }
        public string Nazev { get; set; }
        public int UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
    }
    public class EventUdalostRemove : EventBase
    {
        public EventUdalostRemove()
        {
            MessageType = MessageType.DochazkaRemove;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UdalostId { get; set; }


    }
}
