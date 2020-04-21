using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KancelarWeb.ViewModels
{

    public partial class Uzivatel
    {
        public Guid Id { get; set; }
        [Display(Name = "Uživatel Id")]
        public Guid UzivatelId { get; set; }
        [Display(Name = "Titul")]
          public  string ImportedId { get; set; }

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
        public Guid? EventGuid { get; set; }
        public int Generation { get; set; }


    }
}
