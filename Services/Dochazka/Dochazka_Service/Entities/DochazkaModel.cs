using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dochazka_Service.Entities
{
    public class DochazkaModel
    {
        
            public int Id { get; set; }
          
            public DateTime Datum { get; set; }
            public int UzivatelId { get; set; }
         
            public string UzivatelCeleJmeno { get; set; }
            
            public bool Prichod { get; set; }
        
    }
}
