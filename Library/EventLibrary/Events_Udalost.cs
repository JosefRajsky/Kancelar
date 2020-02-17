using System;
using static EventLibrary.MessageType;

namespace EventLibrary
{
    public class EventUdalostCreated : Base
    {
        public EventUdalostCreated()
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
    public class EventUdalostUpdated : Base
    {
        public EventUdalostUpdated()
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
    public class EventUdalostRemoved : Base
    {
        public EventUdalostRemoved()
        {
            MessageType = EventLibrary.MessageType.DochazkaRemove;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UdalostId { get; set; }


    }
}
