using System;
using System.Net;
using static GestionFicha.Utils.Constants.CodigosErrorAPI;

namespace GestionFicha.Utils
{
    // Clase base para todas nuestras excepciones
    public abstract class BaseException : Exception
    {
        public BaseException()
        {
        }

        public BaseException(string message) : base(message)
        {
        }

        public BaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Clase base para todas las excepciones del API
    /// </summary>
    /// <seealso cref="BaseException" />
    public class ApiException : BaseException
    {
        public HttpStatusCode httpStatusCode { get; }

        public int errorCode { get; }

        public ApiException(string message, Exception innerException, int errorCode = ERROR_GENERICO, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
            : base(message, innerException)
        {
            this.httpStatusCode = httpStatusCode;
            this.errorCode = errorCode;
        }

        public ApiException(string message, int errorCode = -1, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            this.httpStatusCode = httpStatusCode;
            this.errorCode = errorCode;
        }
    }
}