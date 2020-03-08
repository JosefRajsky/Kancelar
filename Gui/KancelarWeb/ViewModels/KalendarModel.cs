using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.ViewModels
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class KalendarModel
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
        public int UzivatelId { get; set; }

        [Newtonsoft.Json.JsonProperty("rok", Required = Newtonsoft.Json.Required.Always)]
        public int Rok { get; set; }
        public virtual string CeleJmeno { get; set; }

        [Newtonsoft.Json.JsonProperty("kalendar", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Year Kalendar { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Year
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("months", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Month> Months { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Month
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("days", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.IList<Day> Days { get; set; }

        [Newtonsoft.Json.JsonProperty("dayCount", Required = Newtonsoft.Json.Required.Always)]
        public int DayCount { get; set; }
        public virtual string MonthName {get{return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Id); } }
        public virtual string CeleJmeno { get; set; }

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v11.0.0.0)")]
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

        [Newtonsoft.Json.JsonProperty("udalostCount", Required = Newtonsoft.Json.Required.Always)]
        public int UdalostCount { get; set; }

        [Newtonsoft.Json.JsonProperty("polozky", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Polozka> Polozky { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Polozka
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("nazev", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Nazev { get; set; }

        [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
        public int UzivatelId { get; set; }

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
