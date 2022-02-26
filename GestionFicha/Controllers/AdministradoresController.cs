using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GestionFicha.Models.DTO;
using GestionFicha.Models.Repositorios;
using GestionFicha.Utils;
using static GestionFicha.Utils.Constants.CodigosErrorAPI;

namespace GestionFicha.Controllers
{
    /// <summary>
    /// Controller de Administradores.
    /// La ruta es api/permisos
    /// </summary>
    /// <seealso cref="BaseController" />
    [Authorize(Roles = Constants.Roles.Administrador)]
    [RoutePrefix("api/permisos")]
    public class AdministradoresController : BaseController
    {
        private IAdministradoresRepository _repository;

        public AdministradoresController(IAdministradoresRepository repository)
        {
            _repository = repository;
        }

        #region Métodos Abstractos de BaseController

        public override IBasicBaseRepository GetRepository()
        {
            return _repository;
        }

        #endregion Métodos Abstractos de BaseController

        #region Métodos CRUD

        // POST: api/permisos
        [Authorize(Roles = Constants.Roles.Administrador)]
        [Route("")]
        public async Task<IHttpActionResult> PostAdministrador([FromBody]AdministradorDTO objDTO)
        {
            if (objDTO.administrador == false)
            {
                try
                {
                    await _repository.Delete(objDTO.nInterno);
                    return Ok(PrepareData(objDTO));
                }
                catch (ElementNotFound e)
                {
                    throw new ApiException(e.Message, e, USUARIO_NO_TIENE_EL_PERMISO);
                }
            }
            else
            {
                try
                {
                    return Ok(PrepareData(await _repository.Create(objDTO)));
                }
                catch (ElementAlreadyExists e)
                {
                    throw new ApiException(e.Message, e, USUARIO_YA_TIENE_EL_PERMISO);
                }
            }
        }

        // GET: api/permisos
        [Route("")]
        public IHttpActionResult GetAdministradores()
        {
            return StatusCode(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        //GET: api/permisos/X
        [Route("{id}", Name = "GetAdministradorById")]
        [ResponseType(typeof(AdministradorDTO))]
        public IHttpActionResult GetAdministrador(string id)
        {
            return StatusCode(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        // PUT: api/permisos/5
        [Route("{id:int}")]
        [ResponseType(typeof(AdministradorDTO))]
        public IHttpActionResult PutAdministrador(int id, [FromBody]AdministradorDTO objDTO)
        {
            return StatusCode(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        // DELETE: api/permisos/5
        [Route("{id:int}")]
        [ResponseType(typeof(AdministradorDTO))]
        public IHttpActionResult DeleteAdministrador(int id)
        {
            return StatusCode(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        #endregion Métodos CRUD
    }
}