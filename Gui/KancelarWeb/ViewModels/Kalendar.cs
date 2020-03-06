using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KancelarWeb.ViewModels
{

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class Kalendar
    {
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
        public int UzivatelId { get; set; }

        [Newtonsoft.Json.JsonProperty("rok", Required = Newtonsoft.Json.Required.Always)]
        public int Rok { get; set; }

        [Newtonsoft.Json.JsonProperty("body", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Body { get; set; }


    }
}
