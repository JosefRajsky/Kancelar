using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kalendar_Api.Functions
{
    public class Day
    {
        public int Id { get; set; }
        public int TypId { get; set; }
        public string Nazev { get; set; }
        public bool IsSvatek { get; set; }
        public int UdalostCount { get; set; }
        public List<Polozka> Polozky { get; set; }
    }
    public class Month {

        public int Id { get; set; }
        public List<Day> Days {get;set;}
        public int DayCount { get; set; }
        public virtual string MonthName { get { return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Id); } }
    }
    public class Year
    {
        public int Id { get; set; }
        public List<Month> Months { get; set; }
    }

    public class Polozka { 
    public int Id { get; set; }
    public string Nazev { get; set; }
    public Guid UzivatelId { get; set; }
    public string CeleJmeno { get; set; }
    public DateTime DatumOd { get; set; }
    public DateTime DatumDo { get; set; }
    }

    public class KalendarGenerator {
        public async Task<Year> KalendarNew()
        {
            var baseDate = DateTime.Today;
            var Rok = new Year()
            {
                Id = baseDate.Year
            };

             

            await Task.Run(() =>
            {
         
                Rok.Months = new List<Month>();
                for (int m = 1; m <= 12; m++)
                {
                    var Mesic = new Month() {
                        Id = m,
                        DayCount = DateTime.DaysInMonth(baseDate.Year, m)
                    };
                    Mesic.Days = new List<Day>();
                    for (int d = 1; d <= Mesic.DayCount; d++)
                    {
                        var datum = new DateTime(Rok.Id,Mesic.Id,d);
                        var Den = new Day() {
                            Id = d,
                            IsSvatek = !string.IsNullOrEmpty(IsSvatek(datum)),
                            Nazev = IsSvatek(datum),
                            TypId = (datum.DayOfWeek ==0)? 7 : (int)datum.DayOfWeek,
                            UdalostCount = 0,
                            Polozky = new List<Polozka>()
                        };
                        Mesic.Days.Add(Den);
                    }
                    Rok.Months.Add(Mesic);
                };               
            });
            return Rok;
        }
    

    public string IsSvatek(DateTime datum)
    {
        var svatek = string.Empty;

            //if (datum == DateTime.Parse("30.3.2018") ||
            //    datum == DateTime.Parse("19.4.2019") ||
            //    datum == DateTime.Parse("10.4.2020") ||
            //    datum == DateTime.Parse("2.4.2021") ||
            //    datum == DateTime.Parse("15.4.2022") ||
            //    datum == DateTime.Parse("7.4.2023") ||
            //    datum == DateTime.Parse("29.3.2024") ||
            //    datum == DateTime.Parse("18.4.2025"))
            //{
            //    svatek = "Velký pátek";
            //}
         if (datum == new DateTime(2018,3,30) ||
        datum == new DateTime(2019, 4, 19) ||
        datum == new DateTime(2020, 4, 10) ||
        datum == new DateTime(2021, 4, 2)
    )
            {
                svatek = "Velký pátek";
            }

            // Pohyblivé velikonoce
            if (2199 >= datum.Year && datum.Year >= 1800)
        {
          
            if (datum.Year == 1954 || datum.Year == 1981)
            {
                if (datum.Year == 1954 && datum.Month == 4 && datum.Day == 12)
                {
                    svatek = "Velikonoční pondělí (Vyjímka)";
                }
                if (datum.Year == 1981 && datum.Month == 4 && datum.Day == 13)
                {
                    svatek = "Velikonoční pondělí (Vyjímka)";
                }
                if (datum.Year == 1954 && datum.Month == 4 && datum.Day == 9)
                {
                    svatek = "Velký pátek (Vyjímka)";
                }
                if (datum.Year == 1981 && datum.Month == 4 && datum.Day == 10)
                {
                    svatek = "Velký pátek (Vyjímka)";
                }
            }

           
            else
            {
                var m = 0;
                var n = 0;

                if (1899 >= datum.Year && datum.Year >= 1800)
                {
                    m = 23;
                    n = 4;
                }
                if (1999 >= datum.Year && datum.Year >= 1900)
                {
                    m = 24;
                    n = 5;
                }
                if (2099 >= datum.Year && datum.Year >= 2000)
                {
                    m = 24;
                    n = 5;
                }
                if (2199 >= datum.Year && datum.Year >= 2100)
                {
                    m = 24;
                    n = 6;
                }

                var a = datum.Year % 19; // Po 19 letech se měsíční cyklus opakuje ve stejné dny
                var b = datum.Year % 4; // Cyklus opakování přestupných let
                var c = datum.Year % 7; // Dorovnání dne v týdnu
                var d = (m + 19 * a) % 30;
                var e = (n + 2 * b + 4 * c + 6 * d) % 7;

                int den;
                int mesic;

                var f = 22 + d + e;
                if (f < 31)
                {
                    mesic = 3;
                    den = f + 1;
                }
                else
                {
                    f = d + e - 9;
                    f = (f > 25) ? f - 7 : f;
                    mesic = 4;
                    den = f + 1;
                }

                if (datum.Month == mesic && datum.Day == den)
                {
                    svatek = "Velikonoční pondělí";
                }

                if (datum.Month == (mesic) && datum.Day == (den - 3) && datum.Year >= 2016)
                {
                    svatek = "Velký pátek";
                }
            }
        }

        // Statické svátky
        if (datum.Month == 1 && datum.Day == 1)
        {
            svatek = "Nový rok";
        }
        if (datum.Month == 5 && datum.Day == 1)
        {
            svatek = "Svátek práce";
        }
        if (datum.Month == 5 && datum.Day == 8)
        {
            svatek = "Den vítězství";
        }
        if (datum.Month == 7 && datum.Day == 5)
        {
            svatek = "Den slovanských věrozvěstů Cyrila a Metoděje";
        }
        if (datum.Month == 7 && datum.Day == 6)
        {
            svatek = "Den upálení mistra Jana Husa";
        }
        if (datum.Month == 9 && datum.Day == 28)
        {
            svatek = "Den české státnosti";
        }
        if (datum.Month == 10 && datum.Day == 28)
        {
            svatek = "Den vzniku samostatného československého státu";
        }
        if (datum.Month == 11 && datum.Day == 17)
        {
            svatek = "Den boje za svobodu a demokracii";
        }
        if (datum.Month == 12 && datum.Day == 24)
        {
            svatek = "Štědrý den";
        }
        if (datum.Month == 12 && datum.Day == 25)
        {
            svatek = "1. Svátek vánoční";
        }
        if (datum.Month == 12 && datum.Day == 26)
        {
            svatek = "2. Svátek vánoční";
        }
        return svatek;
    }
    }


}
