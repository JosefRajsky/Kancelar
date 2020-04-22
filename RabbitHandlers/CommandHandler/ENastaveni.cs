using System;

namespace CommandHandler
{
    public class CommandNastaveniCreate
    { 
        public string NastaveniValue1 { get; set; }
        public int NastaveniValue2 { get; set; }
    }
    public class CommandNastaveniUpdate
    {
        public Guid NastaveniId { get; set; }
        public string NastaveniValue1 { get; set; }
        public int NastaveniValue2 { get; set; }
    }
    public class CommandNastaveniRemove
    {
        public Guid NastaveniId { get; set; }
    }

    public class EventNastaveniCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid NastaveniId { get; set; }
        public string NastaveniValue1 { get; set; }
        public int NastaveniValue2 { get; set; }
    }
    public class EventNastaveniUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid NastaveniId { get; set; }
        public string NastaveniValue1 { get; set; }
        public int NastaveniValue2 { get; set; }
    }
    public class EventNastaveniDeleted
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid NastaveniId { get; set; }
    }
}
