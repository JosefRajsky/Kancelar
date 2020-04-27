using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Soucast_Api
{
    public class Soucast
    {
        [Key]
        public Guid Id { get; set; }        
        public Guid? EventGuid { get; set; }
        public int Generation { get; set; }
        public string ImportedId { get; set; }
        [Display(Name = "Součást ID")]
        public Guid SoucastId { get; set; }
        [Display(Name = "Název")]
        public string Nazev { get; set; }
        [Display(Name = "Zkratka")]
        public string Zkratka { get; set; }
        [Display(Name = "Nadřízená součást")]
        public Guid ParentId { get; set; }


    }
}
