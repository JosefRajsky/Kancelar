using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.ViewModels
{
    public partial class Dochazka
    {
        [Key]
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid Id { get; set; }

        [Newtonsoft.Json.JsonProperty("eventGuid", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid? EventGuid { get; set; }

        [Newtonsoft.Json.JsonProperty("generation", Required = Newtonsoft.Json.Required.Always)]
        public int Generation { get; set; }

        [Newtonsoft.Json.JsonProperty("dochazkaId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid DochazkaId { get; set; }

        [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid UzivatelId { get; set; }

        [Newtonsoft.Json.JsonProperty("rok", Required = Newtonsoft.Json.Required.Always)]
        public int Rok { get; set; }

        [Newtonsoft.Json.JsonProperty("den", Required = Newtonsoft.Json.Required.Always)]
        public int Den { get; set; }

        [Newtonsoft.Json.JsonProperty("mesic", Required = Newtonsoft.Json.Required.Always)]
        public int Mesic { get; set; }

        [Newtonsoft.Json.JsonProperty("denTydne", Required = Newtonsoft.Json.Required.Always)]
        public int DenTydne { get; set; }

        [Newtonsoft.Json.JsonProperty("tick", Required = Newtonsoft.Json.Required.Always)]
        public long Tick { get; set; }

        [Newtonsoft.Json.JsonProperty("prichod", Required = Newtonsoft.Json.Required.Always)]
        public bool Prichod { get; set; }

        [Newtonsoft.Json.JsonProperty("cteckaId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CteckaId { get; set; }

        [Newtonsoft.Json.JsonProperty("datum", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTimeOffset Datum { get; set; }


    }
}
