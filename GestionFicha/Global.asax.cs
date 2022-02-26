using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GestionFicha.Utils;
using GestionFicha.Utils.Security;

namespace GestionFicha
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RegisterHandlers();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", ConfigurationManager.AppSettings["origin"]);
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, PATCH");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-User, X-Version");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
                HttpContext.Current.Response.End();
            }
        }

        private void RegisterHandlers()
        {
            GlobalConfiguration.Configuration.MessageHandlers.Add(
            new AuthenticationMessageHandler());
        }

        public override void Init()
        {
            base.Init();
            BeginRequest += WebApiApplication_BeginRequest;
        }

        private void WebApiApplication_BeginRequest(object sender, EventArgs e)
        {
            Constants.log.Info("Request iniciado");
        }
    }
}
