using System;

namespace CommandHandler
{
    public class CommandUkolCreate
    { 
        public string UkolValue1 { get; set; }
        public int UkolValue2 { get; set; }
    }
    public class CommandUkolUpdate
    {
        public Guid UkolId { get; set; }
        public string UkolValue1 { get; set; }
        public int UkolValue2 { get; set; }
    }
    public class CommandUkolRemove
    {
        public Guid UkolId { get; set; }
    }

    public class EventUkolCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid UkolId { get; set; }
        public string UkolValue1 { get; set; }
        public int UkolValue2 { get; set; }
    }
    public class EventUkolUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid UkolId { get; set; }
        public string UkolValue1 { get; set; }
        public int UkolValue2 { get; set; }
    }
    public class EventUkolDeleted
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid UkolId { get; set; }
    }
}
