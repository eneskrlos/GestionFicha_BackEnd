using GestionFicha.Models.DTO;
using GestionFicha.Models.Interfaces;
using GestionFicha.Models.Repositorios;
using GestionFicha.Utils;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestionFicha.Controllers
{
    [RoutePrefix("api/productos")]
    public class ProductoController : BaseController
    {
        private readonly IProductosRepository _repository;
        

        public ProductoController(IProductosRepository repository)
        {
            _repository = repository;
        }

        public override IBasicBaseRepository GetRepository()
        {
            return _repository;
        }

        // GET: api/productos
        [Route("")]
        public async Task<IHttpActionResult> GetTodosProducto([FromUri] ParametrosPaginadorDTO parametrosPaginador, [FromUri] ParametrosFiltroProductoDTO parametrosFiltro)
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


        //GET: api/producto/id
        [HttpGet]
        [Route("{id_producto:int}", Name = "GetProductoById")]
        [ResponseType(typeof(ProductoDTO))]
        public async Task<IHttpActionResult> GetProducto(int id_producto)
        {
            try
            {   
                return Ok(await _repository.ObtenerProductoById(id_producto));
            }
            catch (ElementNotFound)
            {
                return NotFound();
            }
            catch (InvalidParameter)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = Constants.Roles.Administrador)]
        [HttpPost]
        [Route("CrearProducto")]
        [ResponseType(typeof(ProductoDTO))]
        public async Task<IHttpActionResult> CrearProducto([FromBody] ProductoDTO productoDTO)
        {
            try
            {   
                var creado = await _repository.CrearProducto(productoDTO);
                return Ok(creado);
            }
            catch (InvalidParameter)
            {
                return BadRequest() ;
            }
        }

        // PUT: api/productos/id_producto
        [Authorize(Roles = Constants.Roles.Administrador)]
        [HttpPut]
        [Route("{id_producto:int}")]
        [ResponseType(typeof(ProductoDTO))]
        public async Task<IHttpActionResult> ActualizarProducto(int id_producto,[FromBody] ProductoDTO productoDTO)
        {
            try
            {
                var actualizado = await  _repository.UpdateProducto(id_producto,productoDTO);
                return Ok(actualizado);
            }
            catch (InvalidParameter)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = Constants.Roles.Administrador)]
        [HttpDelete]
        [Route("{id_producto:int}")]
        [ResponseType(typeof(ProductoDTO))]
        public async Task<IHttpActionResult> EliminarProducto(int id_producto)
        {
            try
            {
                var eliminado = await _repository.EliminarProducto(id_producto);
                return Ok(eliminado);
            }
            catch (InvalidParameter)
            {
                return BadRequest();
            }


        }



    }
}