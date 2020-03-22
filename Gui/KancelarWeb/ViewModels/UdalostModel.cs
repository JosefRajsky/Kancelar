
using KancelarWeb.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.ViewModels

{
    public class UdalostModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Typ události")]
        public int UdalostTypId { get; set; }
        [Display(Name = "Popis")]
        public string Popis { get; set; }

        [Display(Name = "Od")]
        [DataType(DataType.DateTime)]
        public DateTime DatumOd { get; set; }

        [Display(Name = "Do")]
        [DataType(DataType.DateTime)]

        public DateTime DatumDo { get; set; }

        [Display(Name = "Uživatel")]
        public Guid UzivatelId { get; set; }

        [Display(Name = "Jméno a Příjmení")]
        public string UzivatelCeleJmeno { get; set; }

        [Display(Name = "Název")]
        public virtual string Nazev { get; set; }

    }
}
