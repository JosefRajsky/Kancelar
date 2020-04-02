using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Template
{
    public class Temp
    {
        [Key]
        public Guid Id { get; set; }        
        public Guid? EventGuid { get; set; }
        public int Generation { get; set; }
        public Guid TempId { get; set; }
        public string TempValue1 { get; set; }
        public int TempValue2 { get; set; }

    }
}
