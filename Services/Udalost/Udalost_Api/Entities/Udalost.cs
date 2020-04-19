using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Udalost_Api.Entities
{
    public class Udalost
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UdalostId { get; set; }  
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public Guid UzivatelId { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public DateTime DatumZadal { get; set; }
        public string Nazev { get; set; }
        public string UzivatelCeleJmeno { get; set; }
        public Guid? EventGuid { get; set; }
        public int Generation { get; set; }

    }
}
