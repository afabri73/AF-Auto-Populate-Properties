using Newtonsoft.Json;
using System.Collections.Generic;

namespace AF.AutoPopulateProperties.Models
{
    /// <summary>Config Model</summary>
    public class Config
    {
        /// <summary>PropertyType</summary>
        [JsonProperty("propertyType")]
        public string PropertyType { get; set; }
        /// <summary>PropertyToCopy</summary>
        [JsonProperty("propertyAliasToCopyValue")]
        public string PropertyAliasToCopyValue { get; set; }
        /// <summary>
        /// DefaultValue
        /// For DateTime property type, the default value must be in this format: yyyy,mm,dd,hh,mm,ss
        /// </summary>
        [JsonProperty("defaultValue")]
        public string DefaultValue { get; set; }
    }

    /// <summary>Property Model</summary>
    public class Property
    {
        /// <summary>PropertyName</summary>
        [JsonProperty("propertyAlias")]
        public string PropertyAlias { get; set; }
        /// <summary>Config</summary>
        [JsonProperty("config")]
        public Config Config { get; set; }
    }

    /// <summary>Doctype Model</summary>
    public class Doctype
    {
        /// <summary>DocTypeAlias</summary>
        [JsonProperty("doctypeAlias")]
        public string DoctypeAlias { get; set; }
        /// <summary>Properties</summary>
        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }
    }

    /// <summary>Action Model</summary>
    public class Action
    {
        /// <summary>ActionName</summary>
        [JsonProperty("action")]
        public string ActionName { get; set; }
        /// <summary>DocTypes</summary>
        [JsonProperty("doctypes")]
        public List<Doctype> Doctypes { get; set; }
    }

    /// <summary>AutoPopulateProperties Model</summary>
    public class AutoPopulatePropertiesModel
    {
        /// <summary>SectionName</summary>
        [JsonProperty("section")]
        public string SectionName { get; set; }
        /// <summary>Actions</summary>
        [JsonProperty("actions")]
        public List<Action> Actions { get; set; }
    }
}