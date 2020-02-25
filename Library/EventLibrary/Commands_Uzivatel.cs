using System;
using static EventLibrary.MessageType;

namespace EventLibrary
{
    public class CommandUzivatelCreate : Base
    {
        public CommandUzivatelCreate()
        {
            MessageType = EventLibrary.MessageType.UzivatelCreate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
       
        public int UzivatelId { get; set; }
        public string TitulPred { get; set; }
        public int Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string TitulZa { get; set; }
        public string Pohlavi { get; set; }
        public DateTime DatumNarozeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Foto { get; set; }


    }
    public class CommandUzivatelUpdate : Base
    {
        public CommandUzivatelUpdate()
        {
            MessageType = EventLibrary.MessageType.UzivatelUpdate;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UzivatelId { get; set; }
        public string TitulPred { get; set; }
        public int Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string TitulZa { get; set; }
        public string Pohlavi { get; set; }
        public DateTime DatumNarozeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Foto { get; set; }


    }
    public class CommandUzivatelRemove : Base
    {
        public CommandUzivatelRemove()
        {
            MessageType = EventLibrary.MessageType.UzivatelRemove;
            Version = 1;
            CreatedDate = DateTime.Now;
        }
        public int UzivatelId { get; set; }


    }
}
