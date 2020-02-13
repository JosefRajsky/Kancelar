using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Models
{
    public class UdalostModel
    {
        public int Id { get; set; }
        [DisplayName("Název")]
        public string Nazev { get; set; }
        [DisplayName("Do")]
        public DateTime DatumOd { get; set; }
        [DisplayName("Od")]
        public DateTime DatumDo { get; set; }
    }
}
