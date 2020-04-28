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
        public Guid SoucastId { get; set; }
        public Guid ParentId { get; set; }
        public string Nazev { get; set; }
        public string Zkratka { get; set; }    
        public string Clenove { get; set; }
        public DateTime DatumVytvoreni { get; set; }
    }
    public class EventStrukturaUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid StrukturaId { get; set; }
        public Guid SoucastId { get; set; }
        public Guid ParentId { get; set; }
        public string Nazev { get; set; }
        public string Zkratka { get; set; }
        public string Clenove { get; set; }
        public DateTime DatumAktualizace { get; set; }
    }
    public class EventStrukturaRemoved
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid StrukturaId { get; set; }
    }
}
