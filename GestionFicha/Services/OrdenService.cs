using GestionFicha.Entity.RH;
using GestionFicha.Models.DTO;
using GestionFicha.Models.Repositorios;
using GestionFicha.Services.Interfaces;
using GestionFicha.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GestionFicha.Services
{
    public class OrdenService : BaseService<Orden>, IOrdenService
    {
        public override DbSet<Orden> ObtenerDBSet()
        {
            return db.Orden;
        }


        public async Task<PaginadorDTO> ObtenerTodasOrdenes(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroOrdenDTO parametrosFiltro, decimal nInterno, Func<Orden, OrdenDTO> func)
        {
            var ordenQuery = (nInterno != 0) ? ObtenerDBSet().AsQueryable().Include(x => x.producto)
                .Where(x => x.nInterno == nInterno)
                : ObtenerDBSet().AsQueryable().Include(x => x.producto);
            
            // Si tenemos filtros, los procesamos
            if (parametrosFiltro != null)
            {
                ordenQuery = ProcesarFiltro(ordenQuery, parametrosFiltro);
            }
            else
            {
                // De no haber filtro, utilizamos el orden por defecto
                ordenQuery = ordenQuery.OrderBy(x => x.fecha);
            }

            return await Paginador.ProcesarPaginador(ordenQuery, parametrosPaginador, func);
        }

        private IQueryable<Orden> ProcesarFiltro(IQueryable<Orden> query, ParametrosFiltroOrdenDTO parametrosFiltro)
        {
            var ordenDescendente = parametrosFiltro.ordenarPor.StartsWith("-");
            parametrosFiltro.ordenarPor = parametrosFiltro.ordenarPor.Split('-').Last();
            var ordenesPermitidos = new string[]
            {
                "fecha", "direccion", "cantidad","nombre"
            };
            if (!ordenesPermitidos.Contains(parametrosFiltro.ordenarPor))
            {
                throw new InvalidParameter(String.Format("No es posible ordenar por {0}", parametrosFiltro.ordenarPor));
            }

            if (parametrosFiltro.busquedaGeneral != null)
            {
                var param = parametrosFiltro.busquedaGeneral.Trim();
                query = query.Where(x => (x.fecha).ToString().Contains(param) || x.direccion.ToUpper().Contains(param.ToUpper()) 
                || x.cantidad.ToString().Contains(param) || x.producto.nombre.ToUpper().Contains(param.ToUpper()));
            }

            if (parametrosFiltro.fecha != null)
            {
                query = query.Where(x => x.fecha.ToString().Contains(parametrosFiltro.fecha.ToString()));
            }
            if (parametrosFiltro.direccion != null)
            {
                query = query.Where(x => x.direccion.ToUpper().Contains(parametrosFiltro.direccion.ToUpper()));
            }
            if (parametrosFiltro.cantidad != null)
            {
                query = query.Where(x => x.cantidad == parametrosFiltro.cantidad);
            }
            if (parametrosFiltro.nombre != null)
            {
                query = query.Where(x => x.producto.nombre == parametrosFiltro.nombre);
            }

            if (parametrosFiltro.ordenarPor == "fecha")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.fecha) : query.OrderBy(x => x.fecha);
            }
            if (parametrosFiltro.ordenarPor == "direccion")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.direccion) : query.OrderBy(x => x.direccion);
            }
            if (parametrosFiltro.ordenarPor == "cantidad")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.cantidad) : query.OrderBy(x => x.cantidad);
            }
            if (parametrosFiltro.ordenarPor == "nombre")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.producto.nombre) : query.OrderBy(x => x.producto.nombre);
            }
            return query;
        }

        public async Task<Orden> Obtener(int id_orden)
        {
            return await db.Orden.FirstAsync(x => x.id_orden == id_orden);
        }

        public async Task<Orden> InsertarOrden(Orden orden)
        {
            return await base.Insertar(orden);
        }

       

        public  async Task<Orden> ActualizarOrden(int id_orden,Orden orden)
        {
            if (await EntidadExiste(id_orden))
            {
                return await base.Actualizar(orden);
            }
            else
            {
                throw new ElementNotFound(String.Format("La orden con la descrpción {0} con fehca {1} no fue encontrada en la BD", orden.direccion, orden.fecha));
            }
            
        }

        public async Task<Orden> EliminarOrden(int id_orden)
        {
            if (await EntidadExiste(id_orden))
            {
                Orden ordeneliminar = await Obtener(id_orden);
                await base.Borrar(id_orden);
                return ordeneliminar;
            }
            else
            {
                throw new ElementNotFound(String.Format("La orden no fue encontrada en la BD"));

            }

        }

    }
}