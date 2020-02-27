using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb
{
    public class AppEnums
    {
        public enum UdalostTyp
        {
            [Display(Name = "Přítomen")]
            Pritomen = 0,
            [Display(Name = "Dovolená")]
            Dovolena = 1,
            [Display(Name = "Pracovní cesta")]
            PracovniCesta = 2,
            [Display(Name = "Pracovní volno")]
            PracovniVolno = 3,
            [Display(Name = "Kurz")]
            Kurz = 4,
            [Display(Name = "Pracovní neschopnost")]
            PracovniNeschopnost = 5,
            [Display(Name = "Home office")]
            HomeOffice = 6,
        }
        public enum Pohlavi {
            [Display(Name = "Neurčeno")]
            Neurceno = 0,
            [Display(Name = "Muž")]
            Muz = 1,
            [Display(Name = "Žena")]
            Zena = 2,
        }
    }
}
