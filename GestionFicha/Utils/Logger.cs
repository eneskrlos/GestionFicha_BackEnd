using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace GestionFicha.Utils
{
    /// <summary>
    /// Clase que se encarga del logging (excepciones, mensajes, etc)
    /// </summary>
    public class ExceptionLogger
    {
        /// <summary>
        /// Loguea una excepción
        /// </summary>
        /// <param name="ex">La excepción.</param>
        /// <param name="request">El request.</param>
        private static void Log(Exception ex, HttpRequestMessage request)
        {
            var mensaje = String.Format("{0} - {1} raised", request.Method, request.RequestUri);
            Constants.log.Error(mensaje, ex);
        }

        /// <summary>
        /// Loguea una excepción
        /// </summary>
        /// <param name="ex">La excepción.</param>
        public static void Log(Exception ex)
        {
            if (HttpContext.Current != null)
            {
                var mensaje = String.Format("{0} - {1} raised", HttpContext.Current.Request.HttpMethod, HttpContext.Current.Request.Url.AbsoluteUri);
                Constants.log.Error(mensaje, ex);
            }
            else
            {
                Constants.log.Error("Background task task raised", ex);
            }
        }

        /// <summary>
        /// Dado un HttpActionExecutedContext loguea la excepción contenida en él
        /// </summary>
        /// <param name="context">The context.</param>
        public static void Log(HttpActionExecutedContext context)
        {
            Log(context.Exception, context.Request);
        }
    }
}