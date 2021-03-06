﻿using System;

namespace EventLibrary
{
    public class CommandUzivatelCreate 
    {     

        public Guid UzivatelId { get; set; }
        public string TitulPred { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string TitulZa { get; set; }
        public string Pohlavi { get; set; }
        public DateTime DatumNarozeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Foto { get; set; }


    }
    public class CommandUzivatelUpdate 
    {
    
        public Guid UzivatelId { get; set; }
        public string TitulPred { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string TitulZa { get; set; }
        public string Pohlavi { get; set; }
        public DateTime DatumNarozeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Foto { get; set; }


    }
    public class CommandUzivatelRemove
    {
      public Guid UzivatelId { get; set; }


    }

    public class EventUzivatelCreated 
    {
        public Guid UzivatelId { get; set; }
        public string TitulPred { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string TitulZa { get; set; }
        public string Pohlavi { get; set; }

        public DateTime DatumNarozeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Foto { get; set; }



    }
    public class EventUzivatelUpdated 
    {

        public Guid UzivatelId { get; set; }
        public string TitulPred { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string TitulZa { get; set; }
        public string Pohlavi { get; set; }

        public DateTime DatumNarozeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Foto { get; set; }
    }
    public class EventUzivatelDeleted 
    {
        public Guid UzivatelId { get; set; }
    }
}
