using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.CommandsModels
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class CommandDochazkaCreate
    {
        [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
        public Guid UzivatelId { get; set; }

        [Newtonsoft.Json.JsonProperty("prichod", Required = Newtonsoft.Json.Required.Always)]
        public bool Prichod { get; set; }

        [Newtonsoft.Json.JsonProperty("datum", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTimeOffset Datum { get; set; }

        [Newtonsoft.Json.JsonProperty("cteckaId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CteckaId { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class CommandDochazkaRemove
    {
        [Newtonsoft.Json.JsonProperty("dochazkaId", Required = Newtonsoft.Json.Required.Always)]
        public Guid DochazkaId { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v12.0.0.0)")]
    public partial class CommandDochazkaUpdate
    {
        [Newtonsoft.Json.JsonProperty("dochazkaId", Required = Newtonsoft.Json.Required.Always)]
        public Guid DochazkaId { get; set; }

        [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
        public Guid UzivatelId { get; set; }

        [Newtonsoft.Json.JsonProperty("prichod", Required = Newtonsoft.Json.Required.Always)]
        public bool Prichod { get; set; }

        [Newtonsoft.Json.JsonProperty("datum", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTimeOffset Datum { get; set; }

        [Newtonsoft.Json.JsonProperty("cteckaId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CteckaId { get; set; }


    }
}
