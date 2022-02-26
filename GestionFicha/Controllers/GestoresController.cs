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
    /// Controller de Gestores.
    /// La ruta es api/gestores
    /// </summary>
    /// <seealso cref="BaseController" />
    [Authorize(Roles = Constants.Roles.Administrador)]
    [RoutePrefix("api/gestores")]
    public class GestoresController : BaseController
    {
        private IGestoresRepository _repository;

        public GestoresController(IGestoresRepository repository)
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

        // POST: api/gestores
        [Authorize(Roles = Constants.Roles.Administrador)]
        [Route("")]
        public async Task<IHttpActionResult> PostGestor([FromBody]GestorDTO objDTO)
        {
            if (objDTO.gestor == false)
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

            try
            {
                return Ok(PrepareData(await _repository.Create(objDTO)));
            }
            catch (ElementAlreadyExists e)
            {
                throw new ApiException(e.Message, e, USUARIO_YA_TIENE_EL_PERMISO);
            }
        }

        // GET: api/gestores
        [Route("")]
        public IHttpActionResult GetGestores()
        {
            return StatusCode(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        //GET: api/gestores/X
        [Route("{id}", Name = "GetGestorById")]
        [ResponseType(typeof(GestorDTO))]
        public IHttpActionResult GetGestor(string id)
        {
            return StatusCode(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        // PUT: api/gestores/5
        [Route("{id:int}")]
        [ResponseType(typeof(GestorDTO))]
        public IHttpActionResult PutGestor(int id, [FromBody]GestorDTO objDTO)
        {
            return StatusCode(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        // DELETE: api/gestores/5
        [Route("{id:int}")]
        [ResponseType(typeof(GestorDTO))]
        public IHttpActionResult DeleteGestor(int id)
        {
            return StatusCode(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        #endregion Métodos CRUD
    }
}