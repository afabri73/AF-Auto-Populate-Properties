using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

namespace UmbracoExtensions
{
    /// <summary>
    /// Global
    /// </summary>
    public class Global : HttpApplication
    {
        /// <summary>
        /// Application_Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_Start(object sender, EventArgs e)
        {
            // Codice eseguito all'avvio dell'applicazione
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }
    }
}