using GestionFicha.Entity.RH;
using GestionFicha.Models.DTO;
using GestionFicha.Models.Repositorios;
using GestionFicha.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFicha.Models.Interfaces
{
    public interface IProductosRepository : IBaseRepository<Producto, ProductoDTO, IProductoService>
    {
        Task<PaginadorDTO> GetAll(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroProductoDTO parametrosFiltro);
        Task<ProductoDTO> ObtenerProductoById(int id_producto);
        Task<ProductoDTO> CrearProducto(ProductoDTO productoDTO);
        Task<ProductoDTO> UpdateProducto(int id_producto,ProductoDTO productoDTO);
        Task<ProductoDTO> EliminarProducto(int id_producto);

    }
}
