using System;

namespace CommandHandler
{
    public class CommandKalendarCreate 
    {     

        public Guid UzivatelId { get; set; }
        public string CeleJmeno { get; set; }
        public int Rok { get; set; }
       
    }
    public class CommandKalendarUpdate
    {
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
        public Guid UzivatelId { get; set; }
        public string CeleJmeno { get; set; }
        public int Rok { get; set; }
    }
    public class EventKalendarCreated
    {
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
        public Guid UzivatelId { get; set; }
        public string CeleJmeno { get; set; }
        public int Rok { get; set; }
        public Guid SourceGuid { get; set; }
    }
    public class EventKalendarUpdated
    {
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
        public int KalendarId { get; set; }
        public Guid UzivatelId { get; set; }
        public string CeleJmeno { get; set; }
        public int Rok { get; set; }
        public string Body { get; set; }
        public Guid SourceGuid { get; set; }

    }
    public class EventKalendarRemoved
    {
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
        public Guid UzivatelId { get; set; }
        public int Rok { get; set; }
        public Guid SourceGuid { get; set; }
    }


}
