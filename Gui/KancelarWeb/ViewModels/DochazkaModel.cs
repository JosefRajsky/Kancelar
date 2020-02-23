using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.ViewModels
{
    [GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class DochazkaModel
    {
        [DisplayName("Id")]
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [DisplayName("Datum a čas")]
        [JsonProperty("datum", Required = Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public DateTime Datum { get; set; }

        [DisplayName("Uživatel Id")]
        [JsonProperty("uzivatelId", Required = Required.Always)]
        public int UzivatelId { get; set; }

        [DisplayName("Jméno a příjmení")]
        [JsonProperty("uzivatelCeleJmeno", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string UzivatelCeleJmeno { get; set; }
        
        [DisplayName("Typ vstupu")]
        [JsonProperty("prichod", Required = Required.Always)]
        public bool Prichod { get; set; }

        [DisplayName("Čtečka Id")]
        [JsonProperty("cteckaId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CteckaId { get; set; }


    }
}
