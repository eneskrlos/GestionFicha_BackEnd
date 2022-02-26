using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using GestionFicha.Entity;
using GestionFicha.Models.DTO;
using GestionFicha.Models.Repositorios;
using GestionFicha.Utils;
using GestionFicha.Utils.Security;
using static GestionFicha.Utils.Constants.CodigosErrorAPI;

namespace GestionFicha.Controllers
{
    /// <summary>
    /// Clase base para todos los controllers
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public abstract class BaseController : ApiController
    {
        #region Métodos abstractors

        public abstract IBasicBaseRepository GetRepository();

        #endregion Métodos abstractors

        #region Métodos CRUD

        public virtual async Task<IHttpActionResult> Post(BaseDTO objDTO, string routeName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var new_objDTO = await GetRepository().Create(objDTO);
                return CreatedAtRoute(routeName, new { id = new_objDTO.GetPk() }, PrepareData(new_objDTO));
            }
            catch (ElementAlreadyExists e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_YA_EXISTE);
            }
            catch (EntidadRelacionadaNoEncontrada e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_NO_ENCONTRADO);
            }
            catch (ValidationError e)
            {
                throw new ApiException(e.Message, e, ERROR_DE_VALIDACION);
            }
            catch (ElementNotFound e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_NO_ENCONTRADO);
            }
            catch (UnauthorizedAccess)
            {
                return Unauthorized();
            }
            catch (BaseException e)
            {
                throw new ApiException(e.Message, e);
            }
        }

        public virtual async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(PrepareData(await GetRepository().GetAll()));
            }
            catch (WebException e)
            {
                return ManejarErrorDeRed(e);
            }
            catch (UnauthorizedAccess)
            {
                return Unauthorized();
            }
        }

        public virtual async Task<IHttpActionResult> Get(object id)
        {
            BaseDTO entityDTO;
            try
            {
                entityDTO = await GetRepository().GetByPk(id);
            }
            catch (WebException e)
            {
                return ManejarErrorDeRed(e);
            }
            catch (UnauthorizedAccess)
            {
                return Unauthorized();
            }
            catch (ElementNotFound)
            {
                return NotFound();
            }
            if (entityDTO == null)
            {
                return NotFound();
            }

            return Ok(PrepareData(entityDTO));
        }

        public virtual async Task<IHttpActionResult> Put(int id, BaseDTO objDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != (int)objDTO.GetPk())
            {
                return BadRequest();
            }

            try
            {
                var updatedDTO = await GetRepository().Update(objDTO);
                return Ok(PrepareData(updatedDTO));
            }
            catch (UnauthorizedAccess)
            {
                return Unauthorized();
            }
            catch (ElementNotFound)
            {
                return NotFound();
            }
            catch (ElementoNoSePuedeBorrar e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_NO_SE_PUEDE_BORRAR);
            }
            catch (ElementoNoEditable e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_NO_EDITABLE);
            }
            catch (PropiedadNoEditable e)
            {
                throw new ApiException(e.Message, e, PROPIEDAD_NO_EDITABLE);
            }
            catch (ElementAlreadyExists e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_YA_EXISTE);
            }
            catch (EntidadRelacionadaNoEncontrada e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_NO_ENCONTRADO);
            }
            catch (ErrorDeConcurrencia e)
            {
                throw new ApiException(e.Message, e, ERROR_DE_CONCURRENCIA);
            }
            catch (ValidationError e)
            {
                throw new ApiException(e.Message, e, ERROR_DE_VALIDACION);
            }
            catch (BaseException e)
            {
                throw new ApiException(e.Message, e);
            }
        }

        public virtual async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                // TODO: Decidir qué data vamos a devolver en los DELETE exitosos
                await GetRepository().Delete(id);
                return Ok();
            }
            catch (UnauthorizedAccess)
            {
                return Unauthorized();
            }
            catch (ElementNotFound)
            {
                return NotFound();
            }
            catch (ElementoNoSePuedeBorrar e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_NO_SE_PUEDE_BORRAR);
            }
            catch (ElementoYaEstaInactivo e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_YA_ESTA_INACTIVO);
            }
            catch (BaseException e)
            {
                throw new ApiException(e.Message, e);
            }
        }

        #endregion Métodos CRUD

        #region Otros

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var repository = GetRepository();
                if (repository != null)
                {
                    repository.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public static BaseResponse PrepareData(object obj)
        {
            return new BaseResponse(obj);
        }

        // Devuelve el usuario que está logueado
        public Personal ObtenerUsuarioLogueado()
        {
            var usuario = (Usuario)(User.Identity);
            return usuario.Persona;
        }

        public bool UsuarioLogueadoEsAdmin()
        {
            return User.IsInRole(Constants.Roles.Administrador);
        }

        public bool UsuarioLogueadoEsGestor()
        {
            return User.IsInRole(Constants.Roles.Gestor);
        }

        /// <summary>
        /// Maneja los errores de red al enviar los request a autorizaciones
        /// </summary>
        /// <param name="e">La excepción</param>
        /// <returns></returns>
        /// <exception cref="ApiException">No ha podido establecerse una conexión con el backend de autorizaciones</exception>
        public virtual IHttpActionResult ManejarErrorDeRed(WebException e)
        {
            // Si el Response es null es porque no se pudo establecer la conexión
            if (e.Response == null)
            {
                throw new ApiException("No ha podido establecerse una conexión", e, ERROR_DE_RED);
            }
            Constants.log.Error("Error al enviar request", e);
            var response = (HttpWebResponse)(e.Response);
            HttpResponseMessage result = new HttpResponseMessage(response.StatusCode)
            {
                Content = new StreamContent(e.Response.GetResponseStream())
            };
            return ResponseMessage(result);
        }

        #endregion Otros
    }

    /// <summary>
    /// Clase para los responses.
    ///
    /// Representa un JSON con la siguiente estructura
    /// {
    ///     data: <reponse_data>
    /// }
    /// </summary>
    public class BaseResponse
    {
        public object data;

        public BaseResponse(object data)
        {
            this.data = data;
        }
    }

    /// <summary>
    /// Controller utilizado para pasar el usuario logueado a los repositorios
    /// </summary>
    /// <seealso cref="BaseController" />
    public abstract class BaseControllerConUsuario : BaseController
    {
        #region Métodos CRUD

        private IBasicRepositoryConUsuario _GetRepository()
        {
            return (IBasicRepositoryConUsuario)GetRepository();
        }

        public override async Task<IHttpActionResult> Post(BaseDTO objDTO, string routeName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var new_objDTO = await _GetRepository().Create(objDTO, ObtenerUsuarioLogueado(), UsuarioLogueadoEsGestor());
                return CreatedAtRoute(routeName, new { id = new_objDTO.GetPk() }, PrepareData(new_objDTO));
            }
            catch (WebException e)
            {
                return ManejarErrorDeRed(e);
            }
            catch (ElementAlreadyExists e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_YA_EXISTE);
            }
            catch (EntidadRelacionadaNoEncontrada e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_NO_ENCONTRADO);
            }
            catch (ValidationError e)
            {
                throw new ApiException(e.Message, e, ERROR_DE_VALIDACION);
            }
            catch (ElementNotFound e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_NO_ENCONTRADO);
            }
            catch (UnauthorizedAccess)
            {
                return Unauthorized();
            }
            catch (BaseException e)
            {
                throw new ApiException(e.Message, e);
            }
        }

        public override async Task<IHttpActionResult> Get()
        {
            try
            {
                return Ok(PrepareData(await _GetRepository().GetAll(ObtenerUsuarioLogueado(), UsuarioLogueadoEsGestor())));
            }
            catch (WebException e)
            {
                return ManejarErrorDeRed(e);
            }
            catch (UnauthorizedAccess)
            {
                return Unauthorized();
            }
        }

        public override async Task<IHttpActionResult> Get(object id)
        {
            BaseDTO entityDTO;
            try
            {
                entityDTO = await _GetRepository().GetByPk(id, ObtenerUsuarioLogueado(), UsuarioLogueadoEsGestor());
            }
            catch (WebException e)
            {
                throw new ApiException("Ha ocurrido un error de red", e, ERROR_DE_RED);
            }
            catch (UnauthorizedAccess)
            {
                return Unauthorized();
            }
            catch (ElementNotFound)
            {
                return NotFound();
            }
            if (entityDTO == null)
            {
                return NotFound();
            }

            return Ok(PrepareData(entityDTO));
        }

        public override async Task<IHttpActionResult> Put(int id, BaseDTO objDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != (int)objDTO.GetPk())
            {
                return BadRequest();
            }

            try
            {
                var updatedDTO = await _GetRepository().Update(objDTO, ObtenerUsuarioLogueado(), UsuarioLogueadoEsGestor());
                return Ok(PrepareData(updatedDTO));
            }
            catch (WebException e)
            {
                throw new ApiException("Ha ocurrido un error de red", e, ERROR_DE_RED);
            }
            catch (UnauthorizedAccess)
            {
                return Unauthorized();
            }
            catch (ElementNotFound)
            {
                return NotFound();
            }
            catch (ElementoNoSePuedeBorrar e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_NO_SE_PUEDE_BORRAR);
            }
            catch (PropiedadNoEditable e)
            {
                throw new ApiException(e.Message, e, PROPIEDAD_NO_EDITABLE);
            }
            catch (ElementAlreadyExists e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_YA_EXISTE);
            }
            catch (EntidadRelacionadaNoEncontrada e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_NO_ENCONTRADO);
            }
            catch (ErrorDeConcurrencia e)
            {
                throw new ApiException(e.Message, e, ERROR_DE_CONCURRENCIA);
            }
            catch (ValidationError e)
            {
                throw new ApiException(e.Message, e, ERROR_DE_VALIDACION);
            }
            catch (BaseException e)
            {
                throw new ApiException(e.Message, e);
            }
        }

        public override async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                // TODO: Decidir qué data vamos a devolver en los DELETE exitosos
                await _GetRepository().Delete(id, ObtenerUsuarioLogueado(), UsuarioLogueadoEsGestor());
                return Ok();
            }
            catch (WebException e)
            {
                throw new ApiException("Ha ocurrido un error de red", e, ERROR_DE_RED);
            }
            catch (UnauthorizedAccess)
            {
                return Unauthorized();
            }
            catch (ElementNotFound)
            {
                return NotFound();
            }
            catch (ElementoNoSePuedeBorrar e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_NO_SE_PUEDE_BORRAR);
            }
            catch (ElementoYaEstaInactivo e)
            {
                throw new ApiException(e.Message, e, ELEMENTO_YA_ESTA_INACTIVO);
            }
            catch (BaseException e)
            {
                throw new ApiException(e.Message, e);
            }
        }

        #endregion Métodos CRUD
    }
}