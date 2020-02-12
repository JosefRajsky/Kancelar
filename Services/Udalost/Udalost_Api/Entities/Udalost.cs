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
        public int Id { get; set; }
        public string Nazev { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
    }
}
