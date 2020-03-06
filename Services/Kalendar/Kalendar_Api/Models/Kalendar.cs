using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kalendar_Api.Models
{
    public class Kalendar
    {
        [Key]
        public int Id { get; set; }
        public int UzivatelId { get; set; }
        public int Rok { get; set; }
        public string Body { get; set; }
    }
}
