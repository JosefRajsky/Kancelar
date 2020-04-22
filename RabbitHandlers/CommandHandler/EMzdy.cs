using System;

namespace CommandHandler
{
    public class CommandMzdyCreate
    { 
        public string MzdyValue1 { get; set; }
        public int MzdyValue2 { get; set; }
    }
    public class CommandMzdyUpdate
    {
        public Guid MzdyId { get; set; }
        public string MzdyValue1 { get; set; }
        public int MzdyValue2 { get; set; }
    }
    public class CommandMzdyRemove
    {
        public Guid MzdyId { get; set; }
    }

    public class EventMzdyCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid MzdyId { get; set; }
        public string MzdyValue1 { get; set; }
        public int MzdyValue2 { get; set; }
    }
    public class EventMzdyUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid MzdyId { get; set; }
        public string MzdyValue1 { get; set; }
        public int MzdyValue2 { get; set; }
    }
    public class EventMzdyDeleted
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid MzdyId { get; set; }
    }
}
