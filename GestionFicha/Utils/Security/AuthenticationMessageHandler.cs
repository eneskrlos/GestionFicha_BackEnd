using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using GestionFicha.Entity;
using GestionFicha.Models.Repositorios;
using static GestionFicha.Utils.Constants;

namespace GestionFicha.Utils.Security
{
    public class AuthenticationMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var requestIdentifier = Guid.NewGuid();
            // Coloca un identificador único del request
            log4net.LogicalThreadContext.Properties["requestID"] = requestIdentifier;

            // En modo debug permitimos la autenticación con el X-User header
            if (ModoDebug())
            {
                if (await Authenticate(request, false, requestIdentifier))
                {
                    var response = await base.SendAsync(request, cancellationToken);
                    return response.StatusCode == HttpStatusCode.Unauthorized ? CreateUnauthorizedResponse() : response;
                }
            }

            var usuarioEstaAutenticadoConWindows = HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.Identity is WindowsIdentity;
            if (await Authenticate(request, usuarioEstaAutenticadoConWindows, requestIdentifier))
            {
                var response = await base.SendAsync(request, cancellationToken);
                return response.StatusCode == HttpStatusCode.Unauthorized ? CreateUnauthorizedResponse() : response;
            }
            return CreateUnauthorizedResponse();
        }

        public async Task<bool> Authenticate(HttpRequestMessage request, bool UsuarioAutenticadoConWindows, Guid requestIdentifier)
        {
            if (UsuarioAutenticadoConWindows)
            {
                try
                {
                    return await SetPrincipal((WindowsIdentity)HttpContext.Current.User.Identity, requestIdentifier, request);
                }
                catch (ElementNotFound)
                {
                    // Si estamos en modo debug, dejemos que se pueda utilizar al header
                    if (!HttpContext.Current.IsDebuggingEnabled)
                    {
                        return false;
                    }
                }
            }
            string nInternoStr;
            try
            {
                nInternoStr = request.Headers.GetValues("X-User").FirstOrDefault();
            }
            catch (InvalidOperationException)
            {
                Constants.log.Info(String.Format("Header X-User no fue encontrado en el request {0}", request));
                return false;
            }

            if (nInternoStr == null || !int.TryParse(nInternoStr, out int nInterno))
            {
                Constants.log.Info(String.Format("Imposible parsear X-User = {0}. Request {1}", nInternoStr, request));
                return false;
            }

            return await SetPrincipal(nInterno, requestIdentifier, request);
        }

        public HttpResponseMessage CreateUnauthorizedResponse()
        {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        public async Task<bool> SetPrincipal(int nInterno, Guid requestIdentifier, HttpRequestMessage request)
        {
            Tuple<Personal, bool, bool> userYRol;
            try
            {
                userYRol = await ObtenerUsuarioYRol(nInterno, request);
            }
            catch (ElementNotFound)
            {
                Constants.log.Info(String.Format("Usuario con nInterno {0} no fue encontrado en la BD", nInterno));
                return false;
            }

            var principal = new Principal(userYRol.Item1, userYRol.Item2, userYRol.Item3, requestIdentifier);

            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
            return true;
        }

        public async Task<bool> SetPrincipal(WindowsIdentity usuario, Guid requestIdentifier, HttpRequestMessage request)
        {
            Tuple<Personal, bool, bool> userYRol;
            try
            {
                userYRol = await ObtenerUsuarioYRolDesdeNinternoRed(usuario.Name.Split('\\').Last(), request);
            }
            catch (ElementNotFound)
            {
                Constants.log.Info(String.Format("Usuario con usuarioRed {0} no fue encontrado en la BD", usuario.Name));
                return false;
            }

            var principal = new Principal(userYRol.Item1, userYRol.Item2, userYRol.Item3, requestIdentifier);

            Thread.CurrentPrincipal = principal;

            usuario.Impersonate();

            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
            return true;
        }

        public async Task<Tuple<Personal, bool, bool>> ObtenerUsuarioYRol(int nInterno, HttpRequestMessage request)
        {
            var personalRepo = request.GetDependencyScope().GetService(typeof(IPersonalRepository)) as IPersonalRepository;
            return await personalRepo.ObtenerPersonaYRolGestor(nInterno);
        }

        public async Task<Tuple<Personal, bool, bool>> ObtenerUsuarioYRolDesdeNinternoRed(string usuarioRed, HttpRequestMessage request)
        {
            var personalRepo = request.GetDependencyScope().GetService(typeof(IPersonalRepository)) as IPersonalRepository;
            return await personalRepo.ObtenerPersonaYRolGestorDesdenUsuarioRed(usuarioRed);
        }
    }
}