using System;
using static EventLibrary.MessageType;

namespace EventLibrary
{
    public class EventUdalostCreate : EventBase
    {
        public EventUdalostCreate()
        {
            MessageType = EventLibrary.MessageType.DochazkaCreate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public int UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
    }
    public class EventUdalostUpdate : EventBase
    {
        public EventUdalostUpdate()
        {
            MessageType = EventLibrary.MessageType.DochazkaUpdate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public int UdalostId { get; set; }       
        public int UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
    }
    public class EventUdalostRemove : EventBase
    {
        public EventUdalostRemove()
        {
            MessageType = EventLibrary.MessageType.DochazkaRemove;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UdalostId { get; set; }


    }
}
