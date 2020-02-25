using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KancelarWeb.ViewModels
{
    
    public partial class UzivatelModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Titul")]
        public string TitulPred { get; set; }
        [Display(Name = "Jméno")]
        public string Jmeno { get; set; }
        [Display(Name = "Příjmení")]
        public string Prijmeni { get; set; }
        [Display(Name = "Titul")]
        public string TitulZa { get; set; }
        [Display(Name = "Pohlaví")]
        public string Pohlavi { get; set; }
        [Display(Name = "Datum narození")]
        public DateTimeOffset DatumNarozeni { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Telefon")]
        public string Telefon { get; set; }
        [Display(Name = "Fotka")]
        public string Foto { get; set; }


    }
}
