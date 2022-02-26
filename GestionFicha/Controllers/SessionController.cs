using System.Threading.Tasks;
using System.Web.Http;
using GestionFicha.Models.Repositorios;

namespace GestionFicha.Controllers
{
    [RoutePrefix("api/sesion")]
    public class SesionController : BaseController
    {
        private IPersonalRepository _repository;

        public SesionController(IPersonalRepository repository)
        {
            _repository = repository;
        }

        #region Métodos CRUD

        // GET: api/sesion
        [Route("")]
        public async Task<IHttpActionResult> GetSession()
        {
            var user = ObtenerUsuarioLogueado();

            return Ok(PrepareData(await _repository.DBOtoDTOconRoles(user)));
        }

        public override IBasicBaseRepository GetRepository()
        {
            return _repository;
        }

        #endregion Métodos CRUD
    }
}