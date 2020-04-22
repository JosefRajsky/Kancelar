using System;

namespace CommandHandler
{
    public class CommandMailSenderCreate
    { 
        public string MailSenderValue1 { get; set; }
        public int MailSenderValue2 { get; set; }
    }
    public class CommandMailSenderUpdate
    {
        public Guid MailSenderId { get; set; }
        public string MailSenderValue1 { get; set; }
        public int MailSenderValue2 { get; set; }
    }
    public class CommandMailSenderRemove
    {
        public Guid MailSenderId { get; set; }
    }

    public class EventMailSenderCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid MailSenderId { get; set; }
        public string MailSenderValue1 { get; set; }
        public int MailSenderValue2 { get; set; }
    }
    public class EventMailSenderUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid MailSenderId { get; set; }
        public string MailSenderValue1 { get; set; }
        public int MailSenderValue2 { get; set; }
    }
    public class EventMailSenderDeleted
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid MailSenderId { get; set; }
    }
}
