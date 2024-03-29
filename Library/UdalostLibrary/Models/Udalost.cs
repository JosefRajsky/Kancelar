﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UdalostLibrary.Models
{
    public class Udalost
    {
        [Key]
        public int Id { get; set; }  
        public int UdalostTypId { get; set; }
        public string Popis { get; set; }
        public int UzivatelId { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public DateTime DatumZadal { get; set; }
        public string Nazev { get; set; }
        public string UzivatelCeleJmeno { get; set; }


    }
}
