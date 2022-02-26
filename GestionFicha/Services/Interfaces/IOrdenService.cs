using GestionFicha.Entity.RH;
using GestionFicha.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFicha.Services.Interfaces
{
    public interface IOrdenService : IBaseService<Orden>
    {
        Task<PaginadorDTO> ObtenerTodasOrdenes(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroOrdenDTO parametrosFiltro, decimal nInterno, Func<Orden, OrdenDTO> func);
        Task<Orden> Obtener(int id_orden);
        Task<Orden> InsertarOrden(Orden orden);
        Task<Orden> ActualizarOrden(int id_orden, Orden orden);
        Task<Orden> EliminarOrden(int id_orden);

    }
}
