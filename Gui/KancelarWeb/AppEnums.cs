using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb
{

    public enum EAktivitaTyp
        {
            [Display(Description = "Přítomen")]
            Pritomen = 0,
            [Display(Description = "Dovolená")]
            Dovolena = 1,
            [Display(Description = "Pracovní cesta")]
            PracovniCesta = 2,
            [Display(Description = "Pracovní volno")]
            PracovniVolno = 3,
            [Display(Description = "Kurz")]
            Kurz = 4,
            [Display(Description = "Pracovní neschopnost")]
            PracovniNeschopnost = 5,
            [Display(Description = "Home office")]
            HomeOffice = 6,
        }
        public enum EPohlavi {
            [Display(Description = "Neurčeno")]
            Neurceno = 0,
            [Display(Description = "Muž")]
            Muz = 1,
            [Display(Description = "Žena")]
            Zena = 2,
        }




}
