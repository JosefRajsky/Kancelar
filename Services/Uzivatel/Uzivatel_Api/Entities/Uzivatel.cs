using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Uzivatel_Api
{
    public class Uzivatel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? EventGuid { get; set; }
        public int Generation { get; set; }
        public Guid UzivatelId { get; set; }
        public  string ImportedId { get; set; }

        public string TitulPred { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string TitulZa { get; set; }
        public string Pohlavi { get; set; }
        public DateTime DatumNarozeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }

        
    }
}
