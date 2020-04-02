﻿//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.2.3.0 (NJsonSchema v10.1.5.0 (Newtonsoft.Json v11.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

#pragma warning disable 108 // Disable "CS0108 '{derivedDto}.ToJson()' hides inherited member '{dtoBase}.ToJson()'. Use the new keyword if hiding was intended."
#pragma warning disable 114 // Disable "CS0114 '{derivedDto}.RaisePropertyChanged(String)' hides inherited member 'dtoBase.RaisePropertyChanged(String)'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword."
#pragma warning disable 472 // Disable "CS0472 The result of the expression is always 'false' since a value of type 'Int32' is never equal to 'null' of type 'Int32?'
#pragma warning disable 1573 // Disable "CS1573 Parameter '...' has no matching param tag in the XML comment for ...
#pragma warning disable 1591 // Disable "CS1591 Missing XML comment for publicly visible type or member ..."

namespace KancelarWeb.CommandsModels
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class CommandKalendarCreate
    {
        [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
        public int UzivatelId { get; set; }

        [Newtonsoft.Json.JsonProperty("celeJmeno", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CeleJmeno { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class CommandKalendarUpdate
    {
        [Newtonsoft.Json.JsonProperty("kalendarId", Required = Newtonsoft.Json.Required.Always)]
        public int KalendarId { get; set; }

        [Newtonsoft.Json.JsonProperty("uzivatelId", Required = Newtonsoft.Json.Required.Always)]
        public int UzivatelId { get; set; }


    }

    //[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v11.0.0.0)")]
    //public partial class Message
    //{
    //    [Newtonsoft.Json.JsonProperty("guid", Required = Newtonsoft.Json.Required.Always)]
    //    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    //    public System.Guid Guid { get; set; }

    //    [Newtonsoft.Json.JsonProperty("currentGuid", Required = Newtonsoft.Json.Required.Always)]
    //    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    //    public System.Guid CurrentGuid { get; set; }

    //    [Newtonsoft.Json.JsonProperty("parentGuid", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    //    public System.Guid? ParentGuid { get; set; }

    //    [Newtonsoft.Json.JsonProperty("topicId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    //    public int? TopicId { get; set; }

    //    [Newtonsoft.Json.JsonProperty("messageType", Required = Newtonsoft.Json.Required.Always)]
    //    public MessageType MessageType { get; set; }

    //    [Newtonsoft.Json.JsonProperty("messageTypeText", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    //    public string MessageTypeText { get; set; }

    //    [Newtonsoft.Json.JsonProperty("version", Required = Newtonsoft.Json.Required.Always)]
    //    public int Version { get; set; }

    //    [Newtonsoft.Json.JsonProperty("created", Required = Newtonsoft.Json.Required.Always)]
    //    [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
    //    public System.DateTimeOffset Created { get; set; }

    //    [Newtonsoft.Json.JsonProperty("body", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
    //    public string Body { get; set; }


    //}

    //[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.5.0 (Newtonsoft.Json v11.0.0.0)")]
    //public enum MessageType
    //{
    //    DochazkaCreate = 0,

    //    DochazkaRemove = 1,

    //    DochazkaUpdate = 2,

    //    DochazkaCreated = 3,

    //    DochazkaRemoved = 4,

    //    DochazkaUpdated = 5,

    //    UdalostCreate = 6,

    //    UdalostRemove = 7,

    //    UdalostUpdate = 8,

    //    UdalostCreated = 9,

    //    UdalostRemoved = 10,

    //    UdalostUpdated = 11,

    //    UzivatelCreate = 12,

    //    UzivatelRemove = 13,

    //    UzivatelUpdate = 14,

    //    UzivatelCreated = 15,

    //    UzivatelRemoved = 16,

    //    UzivatelUpdated = 17,

    //    KalendarCreate = 18,

    //    KalendarUpdate = 19,

    //    KalendarCreated = 20,

    //    KalendarUpdated = 21,

    //}

}

#pragma warning restore 1591
#pragma warning restore 1573
#pragma warning restore 472
#pragma warning restore 114
#pragma warning restore 108