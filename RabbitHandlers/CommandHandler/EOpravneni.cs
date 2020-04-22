using System;

namespace CommandHandler
{
    public class CommandOpravneniCreate
    { 
        public string OpravneniValue1 { get; set; }
        public int OpravneniValue2 { get; set; }
    }
    public class CommandOpravneniUpdate
    {
        public Guid OpravneniId { get; set; }
        public string OpravneniValue1 { get; set; }
        public int OpravneniValue2 { get; set; }
    }
    public class CommandOpravneniRemove
    {
        public Guid OpravneniId { get; set; }
    }

    public class EventOpravneniCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid OpravneniId { get; set; }
        public string OpravneniValue1 { get; set; }
        public int OpravneniValue2 { get; set; }
    }
    public class EventOpravneniUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid OpravneniId { get; set; }
        public string OpravneniValue1 { get; set; }
        public int OpravneniValue2 { get; set; }
    }
    public class EventOpravneniDeleted
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid OpravneniId { get; set; }
    }
}
