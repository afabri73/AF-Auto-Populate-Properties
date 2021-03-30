using AF.AutoPopulateProperties.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;
using Umbraco.Web;

namespace AF.AutoPopulateProperties.Handlers
{
    /// <summary>
    /// APPRegisterEventsComposer
    /// </summary>
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class APPRegisterEventsComposer : IUserComposer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="composition"></param>
        public void Compose(Composition composition)
        {
            composition.Components().Append<APPRegisterEventComponent>();
            //composition.Register<APPRegisterEventComponent, APPRegisterEventComponent>();
        }
    }

    /// <summary>
    /// AutoPopulatePropertiesEventComponent : IApplicationEventHandler
    /// This Handler allows to update the properties specified in ~/config/AFUmbracoLibrary-APF.config.json
    /// </summary>
    public class APPRegisterEventComponent : IComponent
    {
        /// <summary>
        /// APP Configuration File
        /// </summary>
        protected string AF_AutoPopulateProperties_ConfigFile = "~/App_Plugins/AFAutoPopulateProperties/afapp.config.json";

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            //subscribe on start
            ContentService.Saving += this.ContentService_Saving;
            MediaService.Saving += this.MediaService_Saving;
        }

        /// <summary>
        /// Terminate
        /// </summary>
        public void Terminate()
        {
            //unsubscribe during shutdown
            ContentService.Saving -= this.ContentService_Saving;
            MediaService.Saving -= this.MediaService_Saving;
        }

        /// <summary>
        /// APPRegisterEvents
        /// </summary>
        public APPRegisterEventComponent() { }

        /// <summary>
        /// ContentService_Saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="contentEventArgs"></param>
        void ContentService_Saving(IContentService sender, SaveEventArgs<IContent> contentEventArgs)
        {
            try
            {
                List<AutoPopulatePropertiesModel> APP_JSONConfiguration = JsonConvert.DeserializeObject<List<AutoPopulatePropertiesModel>>(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(AF_AutoPopulateProperties_ConfigFile)));

                AF.AutoPopulateProperties.Models.Section APPContentModel = APP_JSONConfiguration.First().Tabs.Find(f => f.SectionName == "content");

                if (APPContentModel != null)
                {
                    AF.AutoPopulateProperties.Models.Action APPSavingAction = APPContentModel.Actions.Find(f => f.ActionName == "saving");

                    if ((APPSavingAction != null) && (APPSavingAction.Doctypes.Count > 0))
                    {
                        foreach (var content in contentEventArgs.SavedEntities)
                        {
                            AF.AutoPopulateProperties.Models.Doctype APPDoctype = APPSavingAction.Doctypes.Find(f => f.DoctypeAlias == content.ContentType.Alias || f.DoctypeAlias == "all" || f.DoctypeAlias == string.Empty);

                            if (APPDoctype != null && APPDoctype.Properties.Count > 0)
                            {
                                foreach (var APPProperty in APPDoctype.Properties)
                                {
                                    if (((content.HasProperty(APPProperty.PropertyAlias)) && (APPProperty.Config.PropertyType == "bool")) || ((content.HasProperty(APPProperty.PropertyAlias)) && (String.IsNullOrEmpty(content.GetValue<string>(APPProperty.PropertyAlias)))))
                                    {
                                        switch (APPProperty.Config.PropertyType)
                                        {
                                            case "bool":
                                                if (APPProperty.Config.DefaultValue == "true")
                                                {
                                                    content.SetValue(APPProperty.PropertyAlias, "1");
                                                }
                                                break;
                                            case "datetime":
                                                if (APPProperty.Config.DefaultValue == "now")
                                                {
                                                    content.SetValue(APPProperty.PropertyAlias, DateTime.Now);
                                                }
                                                else if (APPProperty.Config.PropertyAliasToCopyValue == "CreateDate")
                                                {
                                                    if (content.CreateDate.ToString().Contains("1/1/0001"))
                                                        content.SetValue(APPProperty.PropertyAlias, DateTime.Now.ToShortDateString());
                                                    else
                                                        content.SetValue(APPProperty.PropertyAlias, DateTime.Parse(content.CreateDate.ToString()));
                                                }
                                                else
                                                {
                                                    if (!String.IsNullOrEmpty(APPProperty.Config.DefaultValue))
                                                    {
                                                        // the date value must be in this format: yyyy,mm,dd,hh,mm,ss
                                                        string ConfigDefaultValue = APPProperty.Config.DefaultValue;

                                                        string DateTimeFormat = "yyyy,mm,dd,hh,mm,ss";

                                                        Regex regex = new Regex(@"^\d{4},\d{2},\d{2},\d{2},\d{2},\d{2}$", RegexOptions.IgnorePatternWhitespace);

                                                        Match result = regex.Match(ConfigDefaultValue);

                                                        if (result.Success)
                                                        {
                                                            DateTime formattedDate = DateTime.ParseExact(ConfigDefaultValue, DateTimeFormat, CultureInfo.InvariantCulture);

                                                            content.SetValue(APPProperty.PropertyAlias, formattedDate);
                                                        }
                                                    }
                                                }
                                                break;
                                            case "string":
                                                if (APPProperty.Config.PropertyAliasToCopyValue == "Name")
                                                {
                                                    content.SetValue(APPProperty.PropertyAlias, content.Name);
                                                }
                                                else if (!String.IsNullOrEmpty(APPProperty.Config.PropertyAliasToCopyValue))
                                                {
                                                    content.SetValue(APPProperty.PropertyAlias, content.Properties[APPProperty.Config.PropertyAliasToCopyValue].GetValue());
                                                }
                                                else
                                                {
                                                    content.SetValue(APPProperty.PropertyAlias, APPProperty.Config.DefaultValue);
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// MediaService_Saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mediaEventArgs"></param>
        void MediaService_Saving(IMediaService sender, SaveEventArgs<IMedia> mediaEventArgs)
        {
            try
            {
                List<AutoPopulatePropertiesModel> APP_JSONConfiguration = JsonConvert.DeserializeObject<List<AutoPopulatePropertiesModel>>(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(AF_AutoPopulateProperties_ConfigFile)));

                AF.AutoPopulateProperties.Models.Section APPContentModel = APP_JSONConfiguration.First().Tabs.Find(f => f.SectionName == "media");

                if (APPContentModel != null)
                {
                    AF.AutoPopulateProperties.Models.Action APPSavingAction = APPContentModel.Actions.Find(f => f.ActionName == "saving");

                    if ((APPSavingAction != null) && (APPSavingAction.Doctypes.Count > 0))
                    {
                        foreach (var media in mediaEventArgs.SavedEntities)
                        {
                            AF.AutoPopulateProperties.Models.Doctype APPDoctype = APPSavingAction.Doctypes.Find(f => f.DoctypeAlias == media.ContentType.Alias || f.DoctypeAlias == "all" || f.DoctypeAlias == string.Empty);

                            if (APPDoctype != null && APPDoctype.Properties.Count > 0)
                            {
                                foreach (var APPProperty in APPDoctype.Properties)
                                {
                                    if (((media.HasProperty(APPProperty.PropertyAlias)) && (APPProperty.Config.PropertyType == "bool")) || ((media.HasProperty(APPProperty.PropertyAlias)) && (String.IsNullOrEmpty(media.GetValue<string>(APPProperty.PropertyAlias)))))
                                    {
                                        switch (APPProperty.Config.PropertyType)
                                        {
                                            case "bool":
                                                if (APPProperty.Config.DefaultValue == "true")
                                                {
                                                    media.SetValue(APPProperty.PropertyAlias, "1");
                                                }
                                                break;
                                            case "datetime":
                                                if (APPProperty.Config.DefaultValue == "now")
                                                {
                                                    media.SetValue(APPProperty.PropertyAlias, DateTime.Now);
                                                }
                                                else if (APPProperty.Config.PropertyAliasToCopyValue == "CreateDate")
                                                {
                                                    if (media.CreateDate.ToString().Contains("1/1/0001"))
                                                        media.SetValue(APPProperty.PropertyAlias, DateTime.Now.ToShortDateString());
                                                    else
                                                        media.SetValue(APPProperty.PropertyAlias, DateTime.Parse(media.CreateDate.ToString()));
                                                }
                                                else
                                                {
                                                    if (!String.IsNullOrEmpty(APPProperty.Config.DefaultValue))
                                                    {
                                                        // the date value must be in this format: yyyy,mm,dd,hh,mm,ss
                                                        string ConfigDefaultValue = APPProperty.Config.DefaultValue;

                                                        string DateTimeFormat = "yyyy,mm,dd,hh,mm,ss";

                                                        Regex regex = new Regex(@"^\d{4},\d{2},\d{2},\d{2},\d{2},\d{2}$", RegexOptions.IgnorePatternWhitespace);

                                                        Match result = regex.Match(ConfigDefaultValue);

                                                        if (result.Success)
                                                        {
                                                            DateTime formattedDate = DateTime.ParseExact(ConfigDefaultValue, DateTimeFormat, CultureInfo.InvariantCulture);

                                                            media.SetValue(APPProperty.PropertyAlias, formattedDate);
                                                        }
                                                    }
                                                }
                                                break;
                                            case "string":
                                                if (APPProperty.Config.PropertyAliasToCopyValue == "Name")
                                                {
                                                    media.SetValue(APPProperty.PropertyAlias, media.Name);
                                                }
                                                else if (!String.IsNullOrEmpty(APPProperty.Config.PropertyAliasToCopyValue))
                                                {
                                                    media.SetValue(APPProperty.PropertyAlias, media.Properties[APPProperty.Config.PropertyAliasToCopyValue].GetValue());
                                                }
                                                else
                                                {
                                                    media.SetValue(APPProperty.PropertyAlias, APPProperty.Config.DefaultValue);
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}