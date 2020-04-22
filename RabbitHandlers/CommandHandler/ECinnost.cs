using System;

namespace CommandHandler
{
    public class CommandCinnostCreate
    { 
        public string CinnostValue1 { get; set; }
        public int CinnostValue2 { get; set; }
    }
    public class CommandCinnostUpdate
    {
        public Guid CinnostId { get; set; }
        public string CinnostValue1 { get; set; }
        public int CinnostValue2 { get; set; }
    }
    public class CommandCinnostRemove
    {
        public Guid CinnostId { get; set; }
    }

    public class EventCinnostCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid CinnostId { get; set; }
        public string CinnostValue1 { get; set; }
        public int CinnostValue2 { get; set; }
    }
    public class EventCinnostUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid CinnostId { get; set; }
        public string CinnostValue1 { get; set; }
        public int CinnostValue2 { get; set; }
    }
    public class EventCinnostDeleted
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid CinnostId { get; set; }
    }
}
