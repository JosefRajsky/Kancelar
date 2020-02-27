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
        [Required]
        [Display(Name = "Jméno")]
        public string Jmeno { get; set; }
        [Required]
        [Display(Name = "Příjmení")]
        public string Prijmeni { get; set; }
        [Display(Name = "Titul")]
        public string TitulZa { get; set; }
        [Required]
        [Display(Name = "Pohlaví")]
        public string Pohlavi { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Datum narození")]
        public DateTimeOffset DatumNarozeni { get; set; }
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Telefon")]
        public string Telefon { get; set; }
        [Display(Name = "Fotka")]
        public string Foto { get; set; }


    }
}
