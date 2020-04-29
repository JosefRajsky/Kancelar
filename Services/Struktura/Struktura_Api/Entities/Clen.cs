using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Struktura_Api.Entities
{
    public class Clen
    {
        [Key]
        public Guid MemberId { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public Guid CeleJmeno { get; set; }
    }
}
