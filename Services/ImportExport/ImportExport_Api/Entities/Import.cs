using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImportExport_Api
{
    public class ImportExport
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? EventGuid { get; set; }
        public int Generation { get; set; }
        public Guid ImportExportId { get; set; }
        public DateTime Datum { get; set; }
        public  string Body { get; set; }
        
        
    }
}
