using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using GestionFicha.Utils;

namespace GestionFicha
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            var cors = new EnableCorsAttribute(ConfigurationManager.AppSettings["origin"], "GET, POST, PUT, DELETE, PATCH, OPTIONS", "Content-Type, Accept, X-User") { SupportsCredentials = true };
            config.EnableCors(cors);

            config.Filters.Add(new ExceptionFilter());
        }
    }
}
