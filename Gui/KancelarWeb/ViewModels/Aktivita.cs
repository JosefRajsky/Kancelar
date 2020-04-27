using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.ViewModels
{
    public partial class Aktivita
    {
        [Key]
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid Id { get; set; }

        [Newtonsoft.Json.JsonProperty("AktivitaId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid AktivitaId { get; set; }

        [Newtonsoft.Json.JsonProperty("AktivitaTypId", Required = Newtonsoft.Json.Required.Always)]
        public int AktivitaTypId { get; set; }

        [Newtonsoft.Json.JsonProperty("popis", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Popis { get; set; }

        [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid UzivatelId { get; set; }

        [Newtonsoft.Json.JsonProperty("datumOd", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTimeOffset DatumOd { get; set; }

        [Newtonsoft.Json.JsonProperty("datumDo", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTimeOffset DatumDo { get; set; }

        [Newtonsoft.Json.JsonProperty("datumZadal", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTimeOffset DatumZadal { get; set; }

        [Newtonsoft.Json.JsonProperty("nazev", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Nazev { get; set; }

        [Newtonsoft.Json.JsonProperty("uzivatelCeleJmeno", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UzivatelCeleJmeno { get; set; }

        [Newtonsoft.Json.JsonProperty("eventGuid", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid? EventGuid { get; set; }

        [Newtonsoft.Json.JsonProperty("generation", Required = Newtonsoft.Json.Required.Always)]
        public int Generation { get; set; }


    }
}
