using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dochazka_Service.Entities
{
    public class Dochazka
    {
        [Key]
        public int Id { get; set; }
        public int UzivatelId { get; set; }
        public int Rok { get; set; }
        public int Den { get; set; }
        public int Mesic { get; set; }
        public int DenTydne { get; set; }
        public long Tick { get; set; }
        public bool Prichod { get; set; }




  
    }
}
