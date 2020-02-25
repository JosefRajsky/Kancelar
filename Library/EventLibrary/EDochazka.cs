using System;

namespace EventLibrary
{
    public class CommandDochazkaCreate
    {   
        public int UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId { get; set; }

    }
    public class CommandDochazkaUpdate 
    {
        public int DochazkaId { get; set; }
        public int UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId { get; set; }


    }
    public class CommandDochazkaRemove
    {        
        public int DochazkaId { get; set; }
    }
    public class EventDochazkaCreated 
    {
   
        public int UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId { get; set; }


    }
    public class EventDochazkaUpdated
    {      
        public int DochazkaId { get; set; }
        public int UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId { get; set; }


    }
    public class EventDochazkaDeleted 
    {
        public int MessageTypeId { get; set; }
       
        public int Version { get; set; }
        public int DochazkaId { get; set; }


    }
}
