using System;

namespace CommandHandler
{
    public class CommandAktivitaCreate
    {
        public int AktivitaTypId { get; set; }
        public string Popis { get; set; }
        public string Nazev { get; set; }
        public string UzivatelCeleJmeno { get; set; }
        public Guid UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }

    }
    public class CommandAktivitaUpdate
    {
        public int AktivitaTypId { get; set; }
        public string Popis { get; set; }
        public Guid AktivitaId { get; set; }
        public string Nazev { get; set; }
        public string UzivatelCeleJmeno { get; set; }
        public Guid UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }

    }
    public class CommandAktivitaRemove
    {
        public Guid AktivitaId { get; set; }
    }
    public class EventAktivitaCreated
    {
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
        public Guid AktivitaId { get; set; }
        public int AktivitaTypId { get; set; }
        public string Popis { get; set; }
        public Guid UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public string Nazev { get; set; }
        public string UzivatelCeleJmeno { get; set; }
    }
    public class EventAktivitaUpdated
    {
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
        public Guid AktivitaId { get; set; }
        public int AktivitaTypId { get; set; }
        public string Popis { get; set; }
        public Guid UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public string Nazev { get; set; }
        public string UzivatelCeleJmeno { get; set; }
    }
    public class EventAktivitaRemoved
    {
        public Guid EventId { get; set; }
        public DateTime EventCreated { get; set; }
        public int Generation { get; set; }
        public Guid AktivitaId { get; set; }
        public Guid UzivatelId { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }


    }
}

