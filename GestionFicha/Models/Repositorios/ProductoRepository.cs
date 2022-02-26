using GestionFicha.Entity.RH;
using GestionFicha.Models.DTO;
using GestionFicha.Models.Interfaces;
using GestionFicha.Services;
using GestionFicha.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GestionFicha.Models.Repositorios
{
    public class ProductoRepository : NewBaseRepository<Producto, ProductoDTO, IProductoService>, IProductosRepository
    {

        public ProductoRepository()
        {
            Service = new ProductoService();
        }

        public ProductoRepository(IProductoService service)
        {
            Service = service;
        }

        public override ProductoDTO DBOtoDTO(Producto entity)
        {
            return new ProductoDTO()
            {
                id_producto = (int)entity.id_producto,
                nombre = (string)entity.nombre,
                descripcion = (string)entity.descripcion,
                precio = (decimal)entity.precio,
            };
        }

        public override Producto DTOtoDBO(ProductoDTO entityDTO)
        {
            return new Producto()
            {
                id_producto = entityDTO.id_producto,
                nombre = entityDTO.nombre,
                descripcion = entityDTO.descripcion,
                precio = entityDTO.precio
            };
        }

        public async Task<PaginadorDTO> GetAll(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroProductoDTO parametrosFiltro)
        {
            return await Service.ObtenerTodosProductos(parametrosPaginador, parametrosFiltro, DBOtoDTO);
        }

        public async Task<ProductoDTO> ObtenerProductoById(int id_producto)
        {
            return DBOtoDTO(await Service.ObtenerProductoById(id_producto));
        }

        
        public async Task<ProductoDTO> CrearProducto(ProductoDTO productoDTO)
        {
            return DBOtoDTO(await Service.InsertarProducto(DTOtoDBO(productoDTO)));
        }

        public async Task<ProductoDTO> UpdateProducto(int id_producto ,ProductoDTO productoDTO)
        {
            return DBOtoDTO(await Service.ActualizarProducto(id_producto,DTOtoDBO(productoDTO)));
        }

        public async Task<ProductoDTO> EliminarProducto(int id_producto)
        {
            return DBOtoDTO(await  Service.Borrar(id_producto));
        }

         
    }
}