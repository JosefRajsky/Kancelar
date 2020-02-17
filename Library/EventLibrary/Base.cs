using System;
using System.Collections.Generic;
using System.Text;
using static EventLibrary.MessageType;

namespace EventLibrary
{
    public class Base
    {
        public int Id { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Version { get; set; }     
    }
}
