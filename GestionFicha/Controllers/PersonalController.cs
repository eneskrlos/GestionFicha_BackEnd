using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GestionFicha.Models.DTO;
using GestionFicha.Models.Repositorios;
using GestionFicha.Utils;

namespace GestionFicha.Controllers
{
    /// <summary>
    /// Controller de Personas.
    /// La ruta es api/personas
    /// </summary>
    /// <seealso cref="GestionEvaluaciones.Controllers.BaseController" />
    [RoutePrefix("api/personas")]
    public class PersonalController : BaseController
    {
        private IPersonalRepository _repository;

        public PersonalController(IPersonalRepository repository)
        {
            _repository = repository;
        }

        #region Métodos Abstractos de BaseController

        /// <summary>
        /// Devuelve el repositorio a este controller
        /// </summary>
        /// <returns></returns>
        public override IBasicBaseRepository GetRepository()
        {
            return _repository;
        }

        #endregion Métodos Abstractos de BaseController

        #region Métodos CRUD

        // POST: api/personas
        [Authorize(Roles = Constants.Roles.Administrador)]
        [Route("")]
        [ResponseType(typeof(PersonalDTO))]
        public IHttpActionResult PostPersona([FromBody]PersonalDTO objDTO)
        {
            return StatusCode(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        // GET: api/personas
        [Route("")]
        public async Task<IHttpActionResult> GetPersonas([FromUri]ParametrosPaginadorDTO parametrosPaginador, [FromUri]ParametrosFiltroPersonalDTO parametrosFiltro)
        {
            try
            {
                return Ok(await _repository.GetAll(parametrosPaginador != null ? parametrosPaginador : new ParametrosPaginadorDTO(), parametrosFiltro));
            }
            catch (UnauthorizedAccess)
            {
                return Unauthorized();
            }
            catch (InvalidParameter)
            {
                return BadRequest();
            }
        }

        // GET: api/personas?incluirRol
        [Authorize(Roles = Constants.Roles.Administrador)]
        [Route("")]
        public async Task<IHttpActionResult> GetPersonas(bool? incluirRol, [FromUri]ParametrosPaginadorDTO parametrosPaginador, [FromUri]ParametrosFiltroPersonalDTO parametrosFiltro)
        {
            try
            {
                return Ok(await _repository.GetAllConRoles(parametrosPaginador != null ? parametrosPaginador : new ParametrosPaginadorDTO(), parametrosFiltro));
            }
            catch (InvalidParameter)
            {
                return BadRequest();
            }
        }

        //GET: api/personas/id
        [Route("{id:int}", Name = "GetPersonalById")]
        [ResponseType(typeof(PersonalConInfodeRolesDTO))]
        public async Task<IHttpActionResult> GetPersona(int id)
        {
            if (ObtenerUsuarioLogueado().nInterno != id && !UsuarioLogueadoEsGestor())
            {
                return Unauthorized();
            }
            return await Get(id);
        }

        // PUT: api/personas/5
        [Authorize(Roles = Constants.Roles.Administrador)]
        [Route("{id:int}")]
        [ResponseType(typeof(PersonalDTO))]
        public IHttpActionResult PutPersona(int id, [FromBody]PersonalDTO objDTO)
        {
            return StatusCode(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        // DELETE: api/personas/5
        [Authorize(Roles = Constants.Roles.Administrador)]
        [Route("{id:int}")]
        [ResponseType(typeof(PersonalDTO))]
        public IHttpActionResult DeletePersona(int id)
        {
            return StatusCode(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        #endregion Métodos CRUD
    }
}