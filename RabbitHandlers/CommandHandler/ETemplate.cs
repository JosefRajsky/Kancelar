using System;

namespace CommandHandler
{
    public class CommandTempCreate
    { 
        public string TempValue1 { get; set; }
        public int TempValue2 { get; set; }
    }
    public class CommandTempUpdate
    {
        public Guid TempId { get; set; }
        public string TempValue1 { get; set; }
        public int TempValue2 { get; set; }
    }
    public class CommandTempRemove
    {
        public Guid TempId { get; set; }
    }

    public class EventTempCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid TempId { get; set; }
        public string TempValue1 { get; set; }
        public int TempValue2 { get; set; }
    }
    public class EventTempUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid TempId { get; set; }
        public string TempValue1 { get; set; }
        public int TempValue2 { get; set; }
    }
    public class EventTempDeleted
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid TempId { get; set; }
    }
}
