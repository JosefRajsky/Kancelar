using System;

namespace CommandHandler
{
    public class CommandStrukturaCreate
    { 
        public string StrukturaValue1 { get; set; }
        public int StrukturaValue2 { get; set; }
    }
    public class CommandStrukturaUpdate
    {
        public Guid StrukturaId { get; set; }
        public string StrukturaValue1 { get; set; }
        public int StrukturaValue2 { get; set; }
    }
    public class CommandStrukturaRemove
    {
        public Guid StrukturaId { get; set; }
    }

    public class EventStrukturaCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid StrukturaId { get; set; }
        public string StrukturaValue1 { get; set; }
        public int StrukturaValue2 { get; set; }
    }
    public class EventStrukturaUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid StrukturaId { get; set; }
        public string StrukturaValue1 { get; set; }
        public int StrukturaValue2 { get; set; }
    }
    public class EventStrukturaDeleted
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid StrukturaId { get; set; }
    }
}
