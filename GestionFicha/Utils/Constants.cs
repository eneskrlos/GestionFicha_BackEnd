using System.Configuration;
using System.Web.Configuration;

namespace GestionFicha.Utils
{
    public class Constants
    {
        public static bool ModoDebug()
        {
            var section = (CompilationSection)(ConfigurationManager.GetSection("system.web/compilation"));

            return section.Debug;
        }

        public static class MediaTypeNames
        {
            public const string ApplicationXml = "application/xml";
            public const string TextXml = "text/xml";
            public const string ApplicationJson = "application/json";
            public const string TextJson = "text/json";
        }

        public static class Roles
        {
            public const string Administrador = "administrador";

            public const string Gestor = "gestor";
        }

        public static class SqlErrorCodes
        {
            public const int ConflictoDeFK = 547;
            public const int ConflictoDeUnicidad = 2627;
        }

        public static readonly string MENSAJE_ERROR_GENERICO = "Lo sentimos, ha ocurrido un error. Por favor inténtalo de nuevo";

        public class CodigosErrorAPI
        {
            #region Constantes para los errores

            // Error code que se utiliza cuando no se ha podido determinar (o no se quiere exponer) la causa del fallo
            public const int ERROR_GENERICO = -1;

            // Estas tipos de errores son los más generales y se utilizan en casi todos los controllers
            public const int ERROR_DE_VALIDACION = -2;

            public const int ELEMENTO_YA_EXISTE = -3;
            public const int ELEMENTO_NO_ENCONTRADO = -4;

            // Estos tipos de errores solo se utilizan en los controllers de Administradores y Gestores
            public const int USUARIO_YA_TIENE_EL_PERMISO = -5;

            public const int USUARIO_NO_TIENE_EL_PERMISO = -6;

            // Este tipo de error indica que se ha producido un error en la conexión con el backend de Competencias
            public const int ERROR_DE_RED = -7;

            // Este elemento no se puede borrar
            public const int ELEMENTO_NO_SE_PUEDE_BORRAR = -11;

            // Este error indica que el mantenimiento maestro ya está inactivo
            public const int ELEMENTO_YA_ESTA_INACTIVO = -12;

            // Ocurre cuando se intenta hacer un cambio de estado inválido
            public const int CAMBIO_DE_ESTADO_NO_PERMITIDO = -13;

            public const int ERROR_DE_CONCURRENCIA = -14;

            // El StoredProcedure al que se hace referencia no existe
            public const int PROCEDURE_NO_IMPLEMENTADO = -15;

            // Ocurre cuando se intenta modificar una propiedad que ya no es editable
            public const int PROPIEDAD_NO_EDITABLE = -16;

            // Ocurre cuando se intenta crear una petición para un proceso que no tiene actividades
            public const int PROCESO_NO_TIENE_ACTIVIDADES = -17;

            // Ocurre cuando se presentan errores en la edición/inserción por lotes
            public const int ERRORES_MULTIPLES = -18;

            // Ocurre cuando se intenta editar una elemento que ya no es editable
            public const int ELEMENTO_NO_EDITABLE = -23;


            #endregion Constantes para los errores
        }

        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}