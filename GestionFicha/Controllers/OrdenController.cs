using GestionFicha.Models.DTO;
using GestionFicha.Models.Interfaces;
using GestionFicha.Models.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace GestionFicha.Controllers
{
    [System.Web.Http.RoutePrefix("api/ordenes")]
    public class OrdenController : BaseController
    {
        private readonly IOrdenRepository _repository;


        public OrdenController(IOrdenRepository repository)
        {
            _repository = repository;   
        }

        public override IBasicBaseRepository GetRepository()
        {
            return _repository;
        }

        // GET: api/ordenes
        [System.Web.Http.Route("")]
        public async Task<IHttpActionResult> GetTodasOrdenes([FromUri] ParametrosPaginadorDTO parametrosPaginador, [FromUri] ParametrosFiltroOrdenDTO parametrosFiltro)
        {
            try
            {
                decimal nInterno = (UsuarioLogueadoEsAdmin()) ? 0 : ObtenerUsuarioLogueado().nInterno;
                return Ok(await _repository.ObtenerTodasOrdenes(parametrosPaginador,parametrosFiltro, nInterno));
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

        // GET: api/ordenes/id_orden
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("{id_orden:int}", Name = "GetOrdenById")]
        [ResponseType(typeof(OrdenDTO))]
        public async Task<IHttpActionResult> GetOrden(int id_orden)
        {
            try
            {
                return Ok(await _repository.Obtener(id_orden));
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

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("CrearOrden")]
        [ResponseType(typeof(OrdenDTO))]
        public async Task<IHttpActionResult> CrearOrden([FromBody] OrdenDTO ordenDTO)
        {
            try
            {
                var personal = ObtenerUsuarioLogueado();
                OrdenDTO ordenTemp = new OrdenDTO();
                if (UsuarioLogueadoEsGestor())
                {
                    ordenTemp.fecha = ordenDTO.fecha;
                    ordenTemp.direccion = ordenDTO.direccion;
                    ordenTemp.cantidad = ordenDTO.cantidad;
                    ordenTemp.es_gestor = true;
                    ordenTemp.id_producto = ordenDTO.id_producto;
                    ordenTemp.nInterno = personal.nInterno;
                }
                else
                {
                    ordenTemp.fecha = ordenDTO.fecha;
                    ordenTemp.direccion = ordenDTO.direccion;
                    ordenTemp.cantidad = ordenDTO.cantidad;
                    ordenTemp.es_gestor = false;
                    ordenTemp.id_producto = ordenDTO.id_producto;
                    ordenTemp.nInterno = personal.nInterno;
                }
                return Ok( await _repository.InsertarOrden(ordenTemp));
            }
            catch (InvalidParameter)
            {
                return BadRequest();
            }
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("{id_orden:int}")]
        [ResponseType(typeof(OrdenDTO))]
        public async Task<IHttpActionResult> ActualizarOrden(int id_orden, [FromBody] OrdenDTO ordenDTO)
        {
            try
            {
                var actualizado = await _repository.ActualizarOrden(id_orden,ordenDTO);
                return Ok(actualizado);
            }
            catch (InvalidParameter)
            {
                return BadRequest();
            }
        }

        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("{id_orden:int}")]
        [ResponseType(typeof(OrdenDTO))]
        public async Task<IHttpActionResult> EliminarOrden(int id_orden)
        {
            try
            {
                return Ok(await _repository.EliminarOrden(id_orden));
            }
            catch (InvalidParameter)
            {
                return BadRequest();
            }
        }
    }
}