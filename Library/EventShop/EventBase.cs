using System;
using System.Collections.Generic;
using System.Text;

namespace EventShop
{
    public class EventBase
    {
        public int Id { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Version { get; set; }     
    }
}
