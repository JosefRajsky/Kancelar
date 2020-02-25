using System;
using static EventLibrary.MessageType;

namespace EventLibrary
{
    public class EventUzivatelCreated : Base
    {
        public EventUzivatelCreated()
        {
            MessageType = EventLibrary.MessageType.UzivatelCreated;
            Version = 1;
            CreatedDate = DateTime.Now;            
        }
        public int UzivatelId { get; set; }
        public string TitulPred { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string TitulZa { get; set; }
        public string Pohlavi { get; set; }

        public DateTime DatumNarozeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Foto { get; set; }
    


}
    public class EventUzivatelUpdated : Base
    {
        public EventUzivatelUpdated()
        {
            MessageType = EventLibrary.MessageType.UzivatelUpdated;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UzivatelId { get; set; }
        public string TitulPred { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string TitulZa { get; set; }
        public string Pohlavi { get; set; }

        public DateTime DatumNarozeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Foto { get; set; }
    }
    public class EventUzivatelDeleted : Base
    {
        public EventUzivatelDeleted()
        {
            MessageType = MessageType.UzivatelRemoved;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UzivatelId { get; set; }


    }
}
