using System;
using System.ComponentModel;

namespace UdalostLibrary
{
    public  enum UdalostTyp 
    {
            [Description("Dovolená")]
                Dovolena = 0,
            [Description("Pracovní cesta")]
                PracovniCesta = 1,
            [Description("Pracovní volno")]       
                PracovniVolno = 2,
            [Description("Kurz")]
                Kurz = 3,
            [Description("Pracovní neschopnost")]
                PracovniNeschopnost = 4,
            [Description("Home office")]
                HomeOffice = 5,
    }

}
