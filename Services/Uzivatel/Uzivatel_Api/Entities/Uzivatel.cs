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
        public Guid Guid { get; set; }
        public string TitulPred { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string TitulZa { get; set; }
        public string Pohlavi { get; set; }

        public DateTime DatumNarozeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Foto { get; set; }
        public Guid LastEvent { get; set; }
        public int Generation { get; set; }
        
    }
}
