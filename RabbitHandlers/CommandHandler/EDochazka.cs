using System;

namespace CommandHandler
{
    public class CommandDochazkaCreate
    {   
        public Guid UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId { get; set; }

    }
    public class CommandDochazkaUpdate 
    {
        public Guid DochazkaId { get; set; }
        public Guid UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId { get; set; }


    }
    public class CommandDochazkaRemove
    {        
        public Guid DochazkaId { get; set; }
    }
    public class EventDochazkaCreated 
    {
   
        public Guid UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId { get; set; }


    }
    public class EventDochazkaUpdated
    {      
        public Guid DochazkaId { get; set; }
        public Guid UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId { get; set; }


    }
    public class EventDochazkaDeleted 
    {
        public int MessageTypeId { get; set; }
       
        public int Version { get; set; }
        public Guid DochazkaId { get; set; }


    }
}
