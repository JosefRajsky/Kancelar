using System;

namespace CommandHandler
{
    public class CommandAktivitaCreate
    { 
        public string AktivitaValue1 { get; set; }
        public int AktivitaValue2 { get; set; }
    }
    public class CommandAktivitaUpdate
    {
        public Guid AktivitaId { get; set; }
        public string AktivitaValue1 { get; set; }
        public int AktivitaValue2 { get; set; }
    }
    public class CommandAktivitaRemove
    {
        public Guid AktivitaId { get; set; }
    }

    public class EventAktivitaCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid AktivitaId { get; set; }
        public string AktivitaValue1 { get; set; }
        public int AktivitaValue2 { get; set; }
    }
    public class EventAktivitaUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid AktivitaId { get; set; }
        public string AktivitaValue1 { get; set; }
        public int AktivitaValue2 { get; set; }
    }
    public class EventAktivitaDeleted
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid AktivitaId { get; set; }
    }
}
