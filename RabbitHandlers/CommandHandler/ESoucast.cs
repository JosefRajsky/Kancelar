using System;

namespace CommandHandler
{
    public class CommandSoucastCreate
    {
        public string ImportedId { get; set; }
       
        public string Nazev { get; set; }
        public string Zkratka { get; set; }
        public Guid ParentId { get; set; }
    }
    public class CommandSoucastUpdate
    {
        public Guid SoucastId { get; set; }
        public string ImportedId { get; set; }
       
        public string Nazev { get; set; }
        public string Zkratka { get; set; }
        public Guid ParentId { get; set; }
    }
    public class CommandSoucastRemove
    {
        public Guid SoucastId { get; set; }
    }

    public class EventSoucastCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public string ImportedId { get; set; }
        public Guid SoucastId { get; set; }
        public string Nazev { get; set; }      
        public string Zkratka { get; set; }      
        public Guid ParentId { get; set; }
    }
    public class EventSoucastUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public string ImportedId { get; set; }
        public Guid SoucastId { get; set; }
        public string Nazev { get; set; }
        public string Zkratka { get; set; }
        public Guid ParentId { get; set; }
    }

    public class EventSoucastRemoved
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid SoucastId { get; set; }
    }
}
