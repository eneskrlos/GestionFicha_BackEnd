using GestionFicha.Entity.RH;
using GestionFicha.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFicha.Services.Interfaces
{
    public interface IProductoService : IBaseService<Producto>
    {
        Task<PaginadorDTO> ObtenerTodosProductos(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroProductoDTO parametrosFiltro, Func<Producto, ProductoDTO> func);

        Task<Producto> ObtenerProductoById(int id_producto);

        Task<Producto> InsertarProducto(Producto prod);

        Task<Producto> ActualizarProducto(int id_producto, Producto producto);

        Task<Producto> Borrar(int id_producto);

    }
}
