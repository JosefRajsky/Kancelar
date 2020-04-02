﻿using Kalendar_Api.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kalendar_Api.Models
{
    public class KalendarModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UzivatelId { get; set; }
        public int Rok { get; set; }
        public Year Kalendar { get; set; }
    }
}