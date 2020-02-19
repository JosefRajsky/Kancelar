using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Api.Entities
{
    public class DochazkaModel
    {

        public int Id { get; set; }
        [DisplayName("Datum a čas")]
        public DateTime Datum { get; set; }
        public int UzivatelId { get; set; }
        [DisplayName("Jméno a Příjmení")]
        public string UzivatelCeleJmeno { get; set; }
        [DisplayName("Druh")]
        public bool Prichod { get; set; }
        [DisplayName("Čtečka")]
        public string CteckaId { get; set; }

    }
}
