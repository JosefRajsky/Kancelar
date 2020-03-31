using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Uzivatel_Api
{
    public class UzivatelModel
    {
        public UzivatelModel(Uzivatel uzivatel)
        {
            Id = uzivatel.UzivatelId;
            TitulPred = uzivatel.TitulPred;
            Jmeno = uzivatel.Jmeno;
            Prijmeni = uzivatel.Prijmeni;
            TitulZa = uzivatel.TitulZa;
            Pohlavi = uzivatel.Pohlavi;
            DatumNarozeni = uzivatel.DatumNarozeni;
            Email = uzivatel.Email;
            Telefon = uzivatel.Telefon;
        }

        [Key]
        [DisplayName("Id")]
        public Guid Id { get; set; }

        [DisplayName("Titul")]
        public string TitulPred { get; set; }
        [DisplayName("Jméno")]
        public string Jmeno { get; set; }
        [DisplayName("Příjmení")]
        public string Prijmeni { get; set; }
        [DisplayName("Titul")]
        public string TitulZa { get; set; }
        [DisplayName("Pohlaví")]
        public string Pohlavi { get; set; }
        [DisplayName("Datum narození")]
        public DateTime DatumNarozeni { get; set; }
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [DisplayName("Telefon")]
        public string Telefon { get; set; }
        [DisplayName("Fotka")]
        public string Foto { get; set; }
        

    }
}
