using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nastaveni_Api.Entities
{
    public class Nastaveni
    {
        public int Id { get; set; }
        public bool GlobalSetting { get; set; }
        public int UzivatelId { get; set; }
        public int SoucastId { get; set; }
        public int Rok { get; set; }
        public DateTime PlatnostOd { get; set; }
        public DateTime PlatnostDo { get; set; }
        public DateTime DatumVytvoreni { get; set; }
        public int DovolenaTyden { get; set; }
        public int DovolenaVikend { get; set; }
        public int PracovniDoba { get; set; }


    
    }
}
