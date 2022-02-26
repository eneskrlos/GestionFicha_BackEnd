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
    public class OrdenRepository : NewBaseRepository<Orden, OrdenDTO, IOrdenService>, IOrdenRepository
    {
        public OrdenRepository()
        {
            Service = new OrdenService();
        }

        public OrdenRepository(IOrdenService service)
        {
            Service = service;
        }

        public override OrdenDTO DBOtoDTO(Orden entity)
        {
            return new OrdenDTO()
            {
                id_orden = (int)entity.id_orden,
                fecha = (DateTime)entity.fecha,
                direccion = (string)entity.direccion,
                cantidad = (int)entity.cantidad,
                id_producto = (int)entity.id_producto,
                nInterno = (int)entity.nInterno,
                es_gestor = (bool)entity.es_gestor
            };
        }

        public override Orden DTOtoDBO(OrdenDTO entityDTO)
        {
            return new Orden()
            {
                id_orden = (int)entityDTO.id_orden,
                fecha = (DateTime)entityDTO.fecha,
                direccion = (string)entityDTO.direccion,
                cantidad = (int)entityDTO.cantidad,
                id_producto = (int)entityDTO.id_producto,
                nInterno = (int)entityDTO.nInterno,
                es_gestor = (bool)entityDTO.es_gestor
            };
        }

        public async Task<OrdenDTO> Obtener(int id_orden)
        {
            return DBOtoDTO(await Service.Obtener(id_orden));
        }

        public async Task<PaginadorDTO> ObtenerTodasOrdenes(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroOrdenDTO parametrosFiltro, decimal nInterno)
        {
            return await Service.ObtenerTodasOrdenes(parametrosPaginador, parametrosFiltro, nInterno, DBOtoDTO);
        }

        public async Task<OrdenDTO> ActualizarOrden(int id_orden,OrdenDTO ordendto)
        {
            return DBOtoDTO(await Service.ActualizarOrden(id_orden, DTOtoDBO(ordendto)));
        }


        public async Task<OrdenDTO> EliminarOrden(int id_orden)
        {
            return DBOtoDTO(await Service.EliminarOrden(id_orden));
        }

        public async Task<OrdenDTO> InsertarOrden(OrdenDTO ordendto)
        {
            return DBOtoDTO(await Service.InsertarOrden(DTOtoDBO(ordendto)));
        }

    }
}