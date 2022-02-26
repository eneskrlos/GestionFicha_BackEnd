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
    public interface IOrdenRepository : IBaseRepository<Orden, OrdenDTO, IOrdenService>
    {
        Task<PaginadorDTO> ObtenerTodasOrdenes(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroOrdenDTO parametrosFiltro, decimal nInterno);
        Task<OrdenDTO> Obtener(int id_orden);
        Task<OrdenDTO> InsertarOrden(OrdenDTO ordendto);
        Task<OrdenDTO> ActualizarOrden(int id_orden, OrdenDTO ordendto);
        Task<OrdenDTO> EliminarOrden(int id_orden);
    }
}
