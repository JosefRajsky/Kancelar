
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using UdalostLibrary;

namespace KancelarWeb.ViewModels

{
    public class UdalostModel
    {
        public int Id { get; set; }
        
        public int UdalostTypId { get; set; }
        [Display(Name = "Popis")]
        public string Popis { get; set; }

        [Display(Name = "Od")]
        [DisplayFormat(DataFormatString = "{0:dd. MM. yyyy}")]
        public DateTime DatumOd { get; set; }

        [Display(Name = "Do")]
        [DisplayFormat(DataFormatString = "{0:dd. MM. yyyy}")]
        public DateTime DatumDo { get; set; }
            
        public int UzivatelId { get; set; }
        [Display(Name = "Jméno a Příjmení")]
        public string UzivatelCeleJmeno { get; set; }

        [Display(Name = "Název")]
        public virtual string Nazev { get {
                return EmumExtension.GetDescription((UdalostTyp)UdalostTypId);
            } }

        public virtual SelectList UdalostTypList { get; set; }

    }
}
