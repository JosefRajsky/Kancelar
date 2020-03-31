using System;
using System.Collections.Generic;
using System.Text;

namespace CommandHandler
{
    public class ProvideHealingStream
    {
        public string Exchange { get; set; }

        public List<MessageType> MessageTypes { get; set; }

        public Guid? EntityId { get; set; }

    }
    public class HealingStreamProvided
    {
        public List<string> MessageList { get; set; }
        public Guid? EntityId { get; set; }

    }
}
