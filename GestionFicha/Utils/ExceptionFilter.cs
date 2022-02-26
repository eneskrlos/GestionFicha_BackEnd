using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using static GestionFicha.Utils.Constants;
using static GestionFicha.Utils.Constants.CodigosErrorAPI;

namespace GestionFicha.Utils
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Task.Run(() => ExceptionLogger.Log(context)); // Loguea la excepción en un thread aparte

            if (context.Response == null)
            {
                context.Response = new HttpResponseMessage();
            }

            if (context.Exception is ApiException _exception2)
            {
                context.Response = context.Request.CreateResponse(_exception2.httpStatusCode, new ApiError(_exception2));
            }
            else
            {
                // Las excepciones que no están manejadas devuelven al cliente un StatusCode 500 y un mensaje genérico
                context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, new ApiError());
            }

            base.OnException(context);
        }
    }

    public class ApiError
    {
        public string message { get; set; }
        public int errorCode { get; set; }

        public ApiError(ApiException apiException)
        {
            message = apiException.Message;
            errorCode = apiException.errorCode;
        }

        public ApiError(string message, int errorCode = ERROR_GENERICO)
        {
            this.message = message;
            this.errorCode = errorCode;
        }

        public ApiError()
        {
            var _temp = new ApiError(message = MENSAJE_ERROR_GENERICO);
            message = _temp.message;
            errorCode = _temp.errorCode;
        }
    }
}