using GestionFicha.Entity.RH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using GestionFicha.Entity;
using GestionFicha.Services.Interfaces;
using GestionFicha.Models.DTO;
using System.Threading.Tasks;
using GestionFicha.Utils;
using GestionFicha.Models.Repositorios;

namespace GestionFicha.Services
{
    public class ProductoService : BaseService<Producto>, IProductoService
    {
        public override DbSet<Producto> ObtenerDBSet()
        {
            return db.Productos;
        }


        public async Task<PaginadorDTO> ObtenerTodosProductos(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroProductoDTO parametrosFiltro, Func<Producto, ProductoDTO> func)
        {
            var productosQuery = ObtenerDBSet().AsQueryable();

            // Si tenemos filtros, los procesamos
            if (parametrosFiltro != null)
            {
                productosQuery = ProcesarFiltro(productosQuery, parametrosFiltro);
            }
            else
            {
                // De no haber filtro, utilizamos el orden por defecto
                productosQuery = productosQuery.OrderBy(x => x.nombre);
            }

            return await Paginador.ProcesarPaginador(productosQuery, parametrosPaginador, func);
        }

        public IQueryable<Producto> ProcesarFiltro(IQueryable<Producto> query, ParametrosFiltroProductoDTO parametrosFiltro)
        {
            var ordenDescendente = parametrosFiltro.ordenarPor.StartsWith("-");
            parametrosFiltro.ordenarPor = parametrosFiltro.ordenarPor.Split('-').Last();
            var ordenesPermitidos = new string[]
            {
                "nombre", "descripcion", "precio"
            };
            if (!ordenesPermitidos.Contains(parametrosFiltro.ordenarPor))
            {
                throw new InvalidParameter(String.Format("No es posible ordenar por {0}", parametrosFiltro.ordenarPor));
            }

            if (parametrosFiltro.busquedaGeneral != null)
            {
                var param = parametrosFiltro.busquedaGeneral.Trim();
                query = query.Where(x => (x.nombre).ToUpper().Contains(param.ToUpper()) || x.descripcion.ToUpper().Contains(param.ToUpper()) || x.precio.ToString().Contains(param));
            }

            if (parametrosFiltro.nombre != null)
            {
                query = query.Where(x => x.nombre.ToUpper().Contains(parametrosFiltro.nombre.ToUpper()));
            }
            if (parametrosFiltro.descripcion != null)
            {
                query = query.Where(x => x.descripcion.ToUpper().Contains(parametrosFiltro.descripcion.ToUpper()));
            }
            if (parametrosFiltro.precio != null)
            {   
                query = query.Where(x => x.precio == parametrosFiltro.precio);
            }
            
            if (parametrosFiltro.ordenarPor == "nombre")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.nombre) : query.OrderBy(x => x.nombre);
            }
            if (parametrosFiltro.ordenarPor == "descripcion")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.descripcion) : query.OrderBy(x => x.descripcion);
            }
            if (parametrosFiltro.ordenarPor == "precio")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.precio) : query.OrderBy(x => x.precio);
            }
            return query;
        }
        //obtengo el producto por su id
        public async Task<Producto> ObtenerProductoById(int id_producto)
        {
            return await db.Productos.FirstAsync(x => x.id_producto == id_producto);
        }

        public async Task<Producto> InsertarProducto(Producto prod)
        {
            if (await EntidadExiste(prod))
            {
                throw new ElementAlreadyExists(String.Format("El producto {0} ya existen en BD", prod.nombre));
            }
            return await base.Insertar(prod);
        }

        public async Task<Producto> ActualizarProducto(int id_producto, Producto producto)
        {
            if(await EntidadExiste(id_producto))
            {
                return await base.Actualizar(producto);
            }
            else
            {
                throw new ElementNotFound(String.Format("Error al modificar : El producto no se encuentra en la Base de datos."));
            }
            
        }

        public async Task<Producto> Borrar(int id_producto)
        {
            if (await EntidadExiste(id_producto))
            {
                Producto productoeliminado = await ObtenerProductoById(id_producto);
                if (await db.Orden.AnyAsync(x => x.producto.id_producto == id_producto))
                {
                    throw new ElementNotFound(String.Format("Error al eliminar : El producto {0} no puede ser eliminado.", productoeliminado.nombre));
                }
                else
                {
                    await base.Borrar(id_producto);
                    return productoeliminado;
                }
            }
            else
            {
                throw new ElementNotFound(String.Format("Error al modificar : El producto no se encuentra en la Base de datos."));
            }
            
        }

    }
}