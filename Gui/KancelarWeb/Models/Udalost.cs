using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Models
{
    public class Udalost
    {
        public int Id { get; set; }
        public string Nazev { get; set; }
        public DateTime Datum { get; set; }
    }
}
