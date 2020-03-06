using System;

namespace EventLibrary
{
    public class CommandKalendarCreate 
    {     

        public int UzivatelId { get; set; }
        public string CeleJmeno { get; set; }
       
    }
    public class CommandKalendarUpdate
    {    
        public int KalendarId { get; set; }
        public int UzivatelId { get; set; }   
    }
    public class EventKalendarCreated
    {
        public int UzivatelId { get; set; }
    }
    public class EventKalendarUpdated
    {
        public int UzivatelId { get; set; }
      
    }
 
}
