﻿using System;

namespace CommandHandler
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
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
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
    public class EventUdalostUpdated
    {
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
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
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
        public Guid UdalostId { get; set; }
        public Guid UzivatelId { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }


    }
}
