using Newtonsoft.Json;
using Struktura_Api.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Struktura_Api
{
    public class Struktura
    {
        [Key]
        public Guid Id { get; set; }        
        public Guid? EventGuid { get; set; }
        public int Generation { get; set; }
        public Guid StrukturaId { get; set; }
        public Guid ParentStrukturaId { get; set; }
        public Guid SoucastId { get; set; }
        public string Nazev { get; set; }
        public string Zkratka { get; set; }       
        public string Clenove { get; set; }
        public DateTime DatumAktualizace { get; set; }

        public virtual List<Clen> Members
        {
            get
            {
                return JsonConvert.DeserializeObject<List<Clen>>(this.Clenove);
            }
        }

    }

    

}
