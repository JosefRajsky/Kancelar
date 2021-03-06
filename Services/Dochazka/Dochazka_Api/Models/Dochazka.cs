﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dochazka_Api.Models
{
    public class Dochazka
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? EventGuid { get; set; }
        public int Generation { get; set; }
        public Guid DochazkaId { get; set; }
        public Guid UzivatelId { get; set; }
        public int Rok { get; set; }
        public int Den { get; set; }
        public int Mesic { get; set; }
        public int DenTydne { get; set; }
        public long Tick { get; set; }
        public bool Prichod { get; set; }
        public string CteckaId { get; set; }
        public DateTime Datum { get; set; }
    }
}
