using System;
using static EventLibrary.MessageType;

namespace EventLibrary
{
    public class CommandUdalostCreate : Base
    {
        public CommandUdalostCreate()
        {
            MessageType = MessageType.UdalostCreate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public int UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
    }
    public class CommandUdalostUpdate : Base
    {
        public CommandUdalostUpdate()
        {
            MessageType =MessageType.UdalostUpdate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public int UdalostId { get; set; }       
        public int UzivatelId { get; set; }
        public DateTime DatumZadal { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
    }
    public class CommandUdalostRemove : Base
    {
        public CommandUdalostRemove()
        {
            MessageType =MessageType.UdalostRemove;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UdalostId { get; set; }


    }
}
