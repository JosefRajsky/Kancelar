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
        public int KalendarId { get; set; }
        public Guid UzivatelId { get; set; }
        public string CeleJmeno { get; set; }
        public int Rok { get; set; }
    }
    public class EventKalendarCreated
    {
        public Guid UzivatelId { get; set; }
        public string CeleJmeno { get; set; }
        public int Rok { get; set; }
        public Guid SourceGuid { get; set; }
    }
    public class EventKalendarUpdated
    {
        public int KalendarId { get; set; }
        public Guid UzivatelId { get; set; }
        public string CeleJmeno { get; set; }
        public int Rok { get; set; }

    }
 
}
