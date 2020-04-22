using System;

namespace CommandHandler
{
    public class CommandVykazCreate
    { 
        public string VykazValue1 { get; set; }
        public int VykazValue2 { get; set; }
    }
    public class CommandVykazUpdate
    {
        public Guid VykazId { get; set; }
        public string VykazValue1 { get; set; }
        public int VykazValue2 { get; set; }
    }
    public class CommandVykazRemove
    {
        public Guid VykazId { get; set; }
    }

    public class EventVykazCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid VykazId { get; set; }
        public string VykazValue1 { get; set; }
        public int VykazValue2 { get; set; }
    }
    public class EventVykazUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid VykazId { get; set; }
        public string VykazValue1 { get; set; }
        public int VykazValue2 { get; set; }
    }
    public class EventVykazDeleted
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid VykazId { get; set; }
    }
}
