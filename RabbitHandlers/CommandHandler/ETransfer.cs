using System;

namespace CommandHandler
{
    public class CommandTransferCreate
    { 
        public string TransferValue1 { get; set; }
        public int TransferValue2 { get; set; }
    }
    public class CommandTransferUpdate
    {
        public Guid TransferId { get; set; }
        public string Value1 { get; set; }
        public int Value2 { get; set; }
    }
    public class CommandTransferRemove
    {
        public Guid TransferId { get; set; }
    }

    public class EventTransferCreated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid TransferId { get; set; }
        public string Value1 { get; set; }
        public int Value2 { get; set; }
    }
    public class EventTransferUpdated
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid TransferId { get; set; }
        public string Value1 { get; set; }
        public int Value2 { get; set; }
    }
    public class EventTransferDeleted
    {
        public Guid EventId { get; set; }
        public int Generation { get; set; }
        public Guid TransferId { get; set; }
    }
}
