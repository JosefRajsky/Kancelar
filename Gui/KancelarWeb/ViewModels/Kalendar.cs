using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.ViewModels
{
    public partial class Kalendar
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid Id { get; set; }

        [Newtonsoft.Json.JsonProperty("kalendarId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid KalendarId { get; set; }

        [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid UzivatelId { get; set; }

        [Newtonsoft.Json.JsonProperty("uzivatelCeleJmeno", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UzivatelCeleJmeno { get; set; }

        [Newtonsoft.Json.JsonProperty("rok", Required = Newtonsoft.Json.Required.Always)]
        public int Rok { get; set; }

        [Newtonsoft.Json.JsonProperty("body", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Body { get; set; }

        [Newtonsoft.Json.JsonProperty("datumAktualizace", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTimeOffset DatumAktualizace { get; set; }

        [Newtonsoft.Json.JsonProperty("kalendarBody", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Year KalendarBody { get; set; }

        [Newtonsoft.Json.JsonProperty("eventGuid", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid? EventGuid { get; set; }

        [Newtonsoft.Json.JsonProperty("generation", Required = Newtonsoft.Json.Required.Always)]
        public int Generation { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.11.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class Year
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("months", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Month> Months { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.11.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class Month
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("days", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Day> Days { get; set; }

        [Newtonsoft.Json.JsonProperty("dayCount", Required = Newtonsoft.Json.Required.Always)]
        public int DayCount { get; set; }

        [Newtonsoft.Json.JsonProperty("monthName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string MonthName { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.11.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class Day
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("typId", Required = Newtonsoft.Json.Required.Always)]
        public int TypId { get; set; }

        [Newtonsoft.Json.JsonProperty("nazev", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Nazev { get; set; }

        [Newtonsoft.Json.JsonProperty("isSvatek", Required = Newtonsoft.Json.Required.Always)]
        public bool IsSvatek { get; set; }

        [Newtonsoft.Json.JsonProperty("aktivitaCount", Required = Newtonsoft.Json.Required.Always)]
        public int AktivitaCount { get; set; }

        [Newtonsoft.Json.JsonProperty("polozky", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Polozka> Polozky { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.11.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class Polozka
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid Id { get; set; }

        [Newtonsoft.Json.JsonProperty("aktivitaTypId", Required = Newtonsoft.Json.Required.Always)]
        public int AktivitaTypId { get; set; }

        [Newtonsoft.Json.JsonProperty("nazev", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Nazev { get; set; }

        [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid UzivatelId { get; set; }

        [Newtonsoft.Json.JsonProperty("celeJmeno", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CeleJmeno { get; set; }

        [Newtonsoft.Json.JsonProperty("datumOd", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTimeOffset DatumOd { get; set; }

        [Newtonsoft.Json.JsonProperty("datumDo", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTimeOffset DatumDo { get; set; }


    }
}
