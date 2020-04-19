using System;

namespace CommandHandler
{
    public class CommandPritomnostCreate 
    {     

        public Guid UzivatelId { get; set; }
        public string CeleJmeno { get; set; }
        public int Rok { get; set; }
       
    }
    public class CommandPritomnostUpdate
    {
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
        public Guid UzivatelId { get; set; }
        public string CeleJmeno { get; set; }
        public int Rok { get; set; }
    }
    public class EventPritomnostCreated
    {
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
        public Guid UzivatelId { get; set; }
        public string CeleJmeno { get; set; }
        public int Rok { get; set; }
        public Guid SourceGuid { get; set; }
    }
    public class EventPritomnostUpdated
    {
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
        public int KalendarId { get; set; }
        public Guid UzivatelId { get; set; }
        public string CeleJmeno { get; set; }
        public int Rok { get; set; }

    }
 
}
