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
        [DisplayName("Od")]
        [DisplayFormat(DataFormatString = "{0:dd. MM. yyyy}")]
        public DateTime DatumOd { get; set; }
        [DisplayName("Do")]
        [DisplayFormat(DataFormatString = "{0:dd. MM. yyyy}")]
        public DateTime DatumDo { get; set; }
    }
}
