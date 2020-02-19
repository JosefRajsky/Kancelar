using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dochazka_Api.Entities
{
    public class DochazkaModel
    {
        public DochazkaModel(Dochazka ent)
        {
            Id = ent.Id;
            Datum = new DateTime(ent.Rok, ent.Mesic, ent.Den);
            UzivatelId = ent.UzivatelId;
            UzivatelCeleJmeno = "jmeno";
            Prichod = ent.Prichod;
            CteckaId = "guid123";
        }

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
