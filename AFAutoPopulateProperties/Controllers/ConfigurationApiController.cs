﻿// AFUmbracoLibrary
using AF.AutoPopulateProperties.Models;
//SYSTEM
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Logging;
//UMBRACO
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace AF.AutoPopulateProperties.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [PluginController("AFAutoPopulateProperties")]
    public class ConfigurationApiController : UmbracoApiController
    {
        /// <summary>
        /// AF Auto Populate Properties config file path (relative)
        /// </summary>
        protected string jsonConfigFileURL = "~/config/AF-AutoPopulateProperties.config.json";

        /// <summary>
        /// GetJSONConfiguration
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<AutoPopulatePropertiesModel> GetJSONConfiguration()
        {
            try
            {
                var jsonConfigFilePath = HttpContext.Current.Server.MapPath(jsonConfigFileURL);
                var jsonConfigFile = File.ReadAllText(jsonConfigFilePath);
                List<AutoPopulatePropertiesModel> jsonStructure = JsonConvert.DeserializeObject<List<AutoPopulatePropertiesModel>>(jsonConfigFile);

                foreach (var section in jsonStructure)
                {
                    foreach (var action in section.Actions)
                    {
                        foreach (var doctype in action.Doctypes)
                        {
                            if (doctype.DoctypeAlias == "all")
                            {
                                doctype.DoctypeAlias = "";
                            }
                        }
                    }
                }

                return jsonStructure;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        /// <summary>
        /// GetAllContentTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<string> GetAllContentTypes()
        {
            var contentTypeService = Services.ContentTypeService;
            var documentTypes = new List<string>();

            foreach (var documentType in contentTypeService.GetAllContentTypes().OrderBy(d => d.Name))
            {
                documentTypes.Add(documentType.Alias);
            }

            return documentTypes;
        }

        /// <summary>
        /// GetAllMediaTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<string> GetAllMediaTypes()
        {
            var contentTypeService = Services.ContentTypeService;
            var mediaTypes = new List<string>();

            foreach (var mediaType in contentTypeService.GetAllMediaTypes().OrderBy(d => d.Name))
            {
                mediaTypes.Add(mediaType.Alias);
            }

            return mediaTypes;
        }

        /// <summary>
        /// GetAllProperties
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        [HttpGet]
        public List<string> GetAllProperties(string section)
        {
            var contentTypeService = Services.ContentTypeService;
            var allProperties = new List<string>();

            switch (section)
            {
                case "content":
                    foreach (var contentItem in contentTypeService.GetAllContentTypes().OrderBy(d => d.Name))
                    {
                        var allContentTypeProperties = GetPropertiesByContentType(section, contentItem.Id);

                        allProperties.AddRange(allContentTypeProperties.Where(x => allProperties.All(y => y != x)));
                    }
                    break;

                case "media":
                    foreach (var mediaItem in contentTypeService.GetAllMediaTypes().OrderBy(d => d.Name))
                    {
                        var allContentTypeProperties = GetPropertiesByContentType(section, mediaItem.Id);

                        allProperties.AddRange(allContentTypeProperties.Where(x => allProperties.All(y => y != x)));
                    }
                    break;
            }

            return new List<string>(allProperties.OrderBy(o => o));
        }

        /// <summary>
        /// GetPropertiesByContentType
        /// </summary>
        /// <param name="section"></param>
        /// <param name="contentTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<string> GetPropertiesByContentType(string section, int contentTypeId)
        {
            var propertyTypes = section == "content" ? Services.ContentTypeService.GetContentType(contentTypeId).PropertyTypes : Services.ContentTypeService.GetMediaType(contentTypeId).PropertyTypes;

            var properties = new List<string>();

            foreach (var property in propertyTypes)
            {
                properties.Add(property.Alias);
            }

            return properties;
        }

        /// <summary>
        /// PostGenConf
        /// </summary>
        /// <param name="customJsonConfiguration"></param>
        /// <returns></returns>
        [HttpPost]
        public bool PostGenConf(object customJsonConfiguration)
        {
            try
            {
                var jsonString = customJsonConfiguration.ToString();

                var jsonConfigFilePath = HttpContext.Current.Server.MapPath(jsonConfigFileURL);

                var jsonSerialized = JsonConvert.SerializeObject(customJsonConfiguration);

                File.WriteAllText(jsonConfigFilePath, jsonSerialized);

                return true;

            }
            catch (Exception ex)
            {
                LogHelper.Error<Exception>(ex.Message, ex);
                return false;
            }
}
    }
}