
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pritomnost_Api.Models
{
    public class Pritomnost
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PritomnostId { get; set; }
        public Guid UzivatelId { get; set; }
        public string UzivatelCeleJmeno { get; set; }     
        public Guid? EventGuid { get; set; }
        public int Generation { get; set; }
    }
}
