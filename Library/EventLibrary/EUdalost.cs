using System;

namespace EventLibrary
{
    public class CommandUdalostCreate 
    {
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public string Nazev { get; set; }
        public string UzivatelCeleJmeno { get; set; }
        public Guid UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }      
       
    }
    public class CommandUdalostUpdate
    {
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public Guid UdalostId { get; set; }
        public string Nazev { get; set; }
        public string UzivatelCeleJmeno { get; set; }
        public Guid UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
      
    }
    public class CommandUdalostRemove
    {       
        public Guid UdalostId { get; set; }
    }
    public class EventUdalostCreated
    {
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public Guid UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public string Nazev { get; set; }
        public string UzivatelCeleJmeno { get; set; }
    }
    public class EventUdalostUpdated
    {
        public Guid UdalostId { get; set; }
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public Guid UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public string Nazev { get; set; }
        public string UzivatelCeleJmeno { get; set; }
    }
    public class EventUdalostRemoved 
    {
        
        public Guid UdalostId { get; set; }


    }
}
