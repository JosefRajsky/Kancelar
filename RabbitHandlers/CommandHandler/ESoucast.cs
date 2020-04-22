using System;

namespace CommandHandler
{
    public class CommandSoucastCreate
    { 
        public string SoucastValue1 { get; set; }
        public int SoucastValue2 { get; set; }
    }
    public class CommandSoucastUpdate
    {
        public Guid SoucastId { get; set; }
        public string SoucastValue1 { get; set; }
        public int SoucastValue2 { get; set; }
    }
    public class CommandSoucastRemove
    {
        public Guid SoucastId { get; set; }
    }

    public class EventSoucastCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid SoucastId { get; set; }
        public string SoucastValue1 { get; set; }
        public int SoucastValue2 { get; set; }
    }
    public class EventSoucastUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid SoucastId { get; set; }
        public string SoucastValue1 { get; set; }
        public int SoucastValue2 { get; set; }
    }
    public class EventSoucastDeleted
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid SoucastId { get; set; }
    }
}
