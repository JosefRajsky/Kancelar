using System;

namespace EventLibrary
{
    public class CommandUdalostCreate 
    {
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public int UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
    }
    public class CommandUdalostUpdate
    {
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public int UdalostId { get; set; }
        public int UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
    }
    public class CommandUdalostRemove
    {       
        public int UdalostId { get; set; }
    }
    public class EventUdalostCreated
    {
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public int UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
    }
    public class EventUdalostUpdated
    {
        public int UdalostId { get; set; }
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public int UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
    }
    public class EventUdalostRemoved 
    {
        
        public int UdalostId { get; set; }


    }
}
