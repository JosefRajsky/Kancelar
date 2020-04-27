using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.ViewModels
{
    public partial class Uzivatel
    {
        [Key]
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid Id { get; set; }

        [Newtonsoft.Json.JsonProperty("eventGuid", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid? EventGuid { get; set; }

        [Newtonsoft.Json.JsonProperty("generation", Required = Newtonsoft.Json.Required.Always)]
        public int Generation { get; set; }

        [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid UzivatelId { get; set; }

        [Newtonsoft.Json.JsonProperty("importedId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ImportedId { get; set; }

        [Newtonsoft.Json.JsonProperty("titulPred", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string TitulPred { get; set; }

        [Newtonsoft.Json.JsonProperty("jmeno", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Jmeno { get; set; }

        [Newtonsoft.Json.JsonProperty("prijmeni", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Prijmeni { get; set; }

        [Newtonsoft.Json.JsonProperty("titulZa", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string TitulZa { get; set; }

        [Newtonsoft.Json.JsonProperty("pohlavi", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Pohlavi { get; set; }

        [Newtonsoft.Json.JsonProperty("datumNarozeni", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTimeOffset DatumNarozeni { get; set; }

        [Newtonsoft.Json.JsonProperty("email", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Email { get; set; }

        [Newtonsoft.Json.JsonProperty("telefon", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Telefon { get; set; }


    }
}
