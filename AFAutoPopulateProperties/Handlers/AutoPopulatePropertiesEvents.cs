using AFUmbracoLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace AFUmbracoLibrary.Handlers
{
    /// <summary>
    /// AutoPopulatePropertiesEvents : IApplicationEventHandler
    /// This Handler allows to update the properties specified in ~/config/AFUmbracoLibrary-APF.config.json
    /// </summary>
    public class AutoPopulatePropertiesEvents : IApplicationEventHandler
    {
        /// <summary>
        /// OnApplicationInitialized
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }

        /// <summary>
        /// OnApplicationStarted
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }

        /// <summary>
        /// OnApplicationStarting
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }

        /// <summary>
        /// APFConfiguration
        /// </summary>
        protected List<AutoPopulateProperties> APP_JSONConfiguration = JsonConvert.DeserializeObject<List<AutoPopulateProperties>>(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/config/AF-AutoPopulateProperties.config.json")));

        /// <summary>
        /// AutoPopulatePropertiesEvents
        /// </summary>
        public AutoPopulatePropertiesEvents()
        {
            // Handlers for Umbraco Content Section
            ContentService.Created += new TypedEventHandler<IContentService, NewEventArgs<IContent>>(ContentService_Created);
            ContentService.Saving += new TypedEventHandler<IContentService, SaveEventArgs<IContent>>(ContentService_Saving);

            // Handlers for Umbraco Media Section
            MediaService.Created += new TypedEventHandler<IMediaService, NewEventArgs<IMedia>>(MediaService_Created);
            MediaService.Saving += new TypedEventHandler<IMediaService, SaveEventArgs<IMedia>>(MediaService_Saving);
        }

        /// <summary>
        /// ContentService_Created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="contentEventArgs"></param>
        void ContentService_Created(IContentService sender, NewEventArgs<IContent> contentEventArgs)
        {
            try
            {
                var APP_CreatedAction = APP_JSONConfiguration.Find(apfconfig => apfconfig.SectionName == "content").Actions.Find(apfsection => apfsection.ActionName == "Created");

                if ((APP_CreatedAction != null) && (APP_CreatedAction.Properties.Count > 0))
                {
                    foreach (var APP_Property in APP_CreatedAction.Properties)
                    {
                        if (contentEventArgs.Entity.HasProperty(APP_Property.PropertyAlias))
                        {
                            switch (APP_Property.Config.PropertyType)
                            {
                                case "bool":
                                    if (APP_Property.Config.DefaultValue == "true")
                                    {
                                        contentEventArgs.Entity.SetValue(APP_Property.PropertyAlias, true);
                                    }
                                    break;
                                case "datetime":
                                    if (APP_Property.Config.DefaultValue == "now")
                                    {
                                        contentEventArgs.Entity.SetValue(APP_Property.PropertyAlias, DateTime.Now);
                                    }
                                    else
                                    {
                                        // the date value must be in this format: yyyy,mm,dd,hh,mm,ss
                                        int[] dateValue = APP_Property.Config.DefaultValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                                        if (dateValue.Length > 0)
                                        {
                                            DateTime formattedDate = new DateTime(dateValue[0], dateValue[1], dateValue[2], dateValue[3], dateValue[4], dateValue[5]);

                                            contentEventArgs.Entity.SetValue(APP_Property.PropertyAlias, formattedDate);
                                        }
                                    }
                                    break;
                                case "string":
                                default:
                                    contentEventArgs.Entity.SetValue(APP_Property.PropertyAlias, APP_Property.Config.DefaultValue);
                                    break;
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
        /// ContentService_Saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="contentEventArgs"></param>
        void ContentService_Saving(IContentService sender, SaveEventArgs<IContent> contentEventArgs)
        {
            try
            {
                var APP_SavingAction = APP_JSONConfiguration.Find(apfconfig => apfconfig.SectionName == "content").Actions.Find(apfsection => apfsection.ActionName == "Saving");

                if ((APP_SavingAction != null) && (APP_SavingAction.Properties.Count > 0))
                {
                    foreach (var content in contentEventArgs.SavedEntities)
                    {
                        foreach (var APP_Property in APP_SavingAction.Properties)
                        {
                            if (((content.HasProperty(APP_Property.PropertyAlias)) && (APP_Property.Config.PropertyType == "bool")) || ((content.HasProperty(APP_Property.PropertyAlias)) && (String.IsNullOrEmpty(content.GetValue<string>(APP_Property.PropertyAlias)))))
                            {
                                switch (APP_Property.Config.PropertyType)
                                {
                                    case "bool":
                                        if (APP_Property.Config.DefaultValue == "true")
                                        {
                                            content.SetValue(APP_Property.PropertyAlias, "1");
                                        }
                                        break;
                                    case "datetime":
                                        if (APP_Property.Config.DefaultValue == "now")
                                        {
                                            content.SetValue(APP_Property.PropertyAlias, DateTime.Now);
                                        }
                                        else if (APP_Property.Config.PropertyAliasToCopyValue == "CreateDate")
                                        {
                                            content.SetValue(APP_Property.PropertyAlias, content.CreateDate);
                                        }
                                        else
                                        {
                                            if (!String.IsNullOrEmpty(APP_Property.Config.DefaultValue))
                                            {
                                                // the date value must be in this format: yyyy,mm,dd,hh,mm,ss
                                                int[] dateValue = APP_Property.Config.DefaultValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                                                if (dateValue.Length > 0)
                                                {
                                                    DateTime formattedDate = new DateTime(dateValue[0], dateValue[1], dateValue[2], dateValue[3], dateValue[4], dateValue[5]);

                                                    content.SetValue(APP_Property.PropertyAlias, formattedDate);
                                                }
                                            }
                                        }
                                        break;
                                    case "string":
                                        if (APP_Property.Config.PropertyAliasToCopyValue == "Name")
                                        {
                                            content.SetValue(APP_Property.PropertyAlias, content.Name);
                                        }
                                        else if (!String.IsNullOrEmpty(APP_Property.Config.PropertyAliasToCopyValue))
                                        {
                                            content.SetValue(APP_Property.PropertyAlias, content.Properties[APP_Property.Config.PropertyAliasToCopyValue].Value);
                                        }
                                        else
                                        {
                                            content.SetValue(APP_Property.PropertyAlias, APP_Property.Config.DefaultValue);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// MediaService_Created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mediaEventArgs"></param>
        void MediaService_Created(IMediaService sender, NewEventArgs<IMedia> mediaEventArgs)
        {
            try
            {
                var APP_CreatedAction = APP_JSONConfiguration.Find(apfconfig => apfconfig.SectionName == "media").Actions.Find(apfsection => apfsection.ActionName == "Created");

                if ((APP_CreatedAction != null) && (APP_CreatedAction.Properties.Count > 0))
                {
                    foreach (var APP_Property in APP_CreatedAction.Properties)
                    {
                        if (mediaEventArgs.Entity.HasProperty(APP_Property.PropertyAlias))
                        {
                            switch (APP_Property.Config.PropertyType)
                            {
                                case "bool":
                                    if (APP_Property.Config.DefaultValue == "true")
                                    {
                                        mediaEventArgs.Entity.SetValue(APP_Property.PropertyAlias, true);
                                    }
                                    break;
                                case "datetime":
                                    if (APP_Property.Config.DefaultValue == "now")
                                    {
                                        mediaEventArgs.Entity.SetValue(APP_Property.PropertyAlias, DateTime.Now);
                                    }
                                    else
                                    {
                                        // the date value must be in this format: yyyy,mm,dd,hh,mm,ss
                                        int[] dateValue = APP_Property.Config.DefaultValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                                        if (dateValue.Length > 0)
                                        {
                                            DateTime formattedDate = new DateTime(dateValue[0], dateValue[1], dateValue[2], dateValue[3], dateValue[4], dateValue[5]);

                                            mediaEventArgs.Entity.SetValue(APP_Property.PropertyAlias, formattedDate);
                                        }
                                    }
                                    break;
                                case "string":
                                default:
                                    mediaEventArgs.Entity.SetValue(APP_Property.PropertyAlias, APP_Property.Config.DefaultValue);
                                    break;
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
                var APP_SavingAction = APP_JSONConfiguration.Find(apfconfig => apfconfig.SectionName == "media").Actions.Find(apfsection => apfsection.ActionName == "Saving");

                if ((APP_SavingAction != null) && (APP_SavingAction.Properties.Count > 0))
                {
                    foreach (var media in mediaEventArgs.SavedEntities)
                    {
                        foreach (var APP_Property in APP_SavingAction.Properties)
                        {
                            if (((media.HasProperty(APP_Property.PropertyAlias)) && (APP_Property.Config.PropertyType == "bool")) || ((media.HasProperty(APP_Property.PropertyAlias)) && (String.IsNullOrEmpty(media.GetValue<string>(APP_Property.PropertyAlias)))))
                            {
                                switch (APP_Property.Config.PropertyType)
                                {
                                    case "bool":
                                        if (APP_Property.Config.DefaultValue == "true")
                                        {
                                            media.SetValue(APP_Property.PropertyAlias, true);
                                        }
                                        break;
                                    case "datetime":
                                        if (APP_Property.Config.DefaultValue == "now")
                                        {
                                            media.SetValue(APP_Property.PropertyAlias, DateTime.Now);
                                        }
                                        else if (APP_Property.Config.PropertyAliasToCopyValue == "CreateDate")
                                        {
                                            media.SetValue(APP_Property.PropertyAlias, media.CreateDate);
                                        }
                                        else
                                        {
                                            if (!String.IsNullOrEmpty(APP_Property.Config.DefaultValue))
                                            {
                                                // the date value must be in this format: yyyy,mm,dd,hh,mm,ss
                                                int[] dateValue = APP_Property.Config.DefaultValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                                                if (dateValue.Length > 0)
                                                {
                                                    DateTime formattedDate = new DateTime(dateValue[0], dateValue[1], dateValue[2], dateValue[3], dateValue[4], dateValue[5]);

                                                    media.SetValue(APP_Property.PropertyAlias, formattedDate);
                                                }
                                            }
                                        }
                                        break;
                                    case "string":
                                        if (APP_Property.Config.PropertyAliasToCopyValue == "Name")
                                        {
                                            media.SetValue(APP_Property.PropertyAlias, media.Name);
                                        }
                                        else if (!String.IsNullOrEmpty(APP_Property.Config.PropertyAliasToCopyValue))
                                        {
                                            media.SetValue(APP_Property.PropertyAlias, media.Properties[APP_Property.Config.PropertyAliasToCopyValue].Value);
                                        }
                                        else
                                        {
                                            media.SetValue(APP_Property.PropertyAlias, APP_Property.Config.DefaultValue);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}