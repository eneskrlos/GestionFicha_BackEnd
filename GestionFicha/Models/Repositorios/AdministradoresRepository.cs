using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionFicha.Entity;
using GestionFicha.Models.DTO;
using GestionFicha.Services;

namespace GestionFicha.Models.Repositorios
{
    public class AdministradoresRepository : NewBaseRepository<Administrador, AdministradorDTO, IAdministradoresService>, IAdministradoresRepository
    {
        #region Métodos abstractos de BaseRepository

        public AdministradoresRepository()
        {
            Service = new AdministradoresService();
        }

        public AdministradoresRepository(IAdministradoresService service)
        {
            Service = service;
        }

        public override Administrador DTOtoDBO(AdministradorDTO entityDTO)
        {
            return new Administrador()
            {
                nInterno = entityDTO.nInterno
            };
        }

        public override AdministradorDTO DBOtoDTO(Administrador entity)
        {
            return new AdministradorDTO()
            {
                administrador = true,
                nInterno = (int)entity.nInterno
            };
        }

        #endregion Métodos abstractos de BaseRepository

        #region Métodos CRUD

        public override async Task<BaseDTO> Create(BaseDTO administradorDTO)
        {
            try
            {
                return await base.Create(administradorDTO);
            }
            catch (ElementAlreadyExists)
            {
                throw new ElementAlreadyExists("El usuario con id " + administradorDTO.GetPk() + " ya fue marcado como administrador");
            }
        }

        public override async Task<List<BaseDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<BaseDTO> GetByPk(object pk)
        {
            try
            {
                return await base.GetByPk(pk);
            }
            catch (ElementNotFound)
            {
                throw new ElementNotFound("El usuario con id " + pk + " no es administrador");
            }
        }

        public override async Task<BaseDTO> Update(BaseDTO administradorDTO)
        {
            throw new NotImplementedException();
        }

        public override async Task Delete(int id)
        {
            try
            {
                await base.Delete(id);
            }
            catch (ElementNotFound)
            {
                throw new ElementNotFound(String.Format("El usuario con id {0} no es administrador", id));
            }
        }

        #endregion Métodos CRUD
    }
}