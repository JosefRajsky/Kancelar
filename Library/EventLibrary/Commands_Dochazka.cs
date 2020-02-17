using System;
using static EventLibrary.MessageType;

namespace EventLibrary
{
    public class CommandDochazkaCreate : Base
    {
        public CommandDochazkaCreate()
        {
            MessageType = EventLibrary.MessageType.DochazkaCreate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId {get;set;}


    }
    public class CommandDochazkaUpdate : Base
    {
        public CommandDochazkaUpdate()
        {
            MessageType = EventLibrary.MessageType.DochazkaUpdate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int DochazkaId { get; set; }
        public int UzivatelId { get; set; }
        public bool Prichod { get; set; }
        public DateTime Datum { get; set; }
        public string CteckaId { get; set; }


    }
    public class CommandDochazkaRemove : Base
    {
        public CommandDochazkaRemove()
        {
            MessageType = EventLibrary.MessageType.DochazkaRemove;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int DochazkaId { get; set; }


    }
}
