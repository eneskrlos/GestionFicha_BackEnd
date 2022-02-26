using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionFicha.Entity;
using GestionFicha.Models.DTO;
using GestionFicha.Services;

namespace GestionFicha.Models.Repositorios
{
    public class GestoresRepository : NewBaseRepository<Gestor, GestorDTO, IGestoresService>, IGestoresRepository
    {
        #region Métodos abstractos de BaseRepository

        public GestoresRepository()
        {
            Service = new GestoresService();
        }

        public GestoresRepository(IGestoresService service)
        {
            Service = service;
        }

        public override Gestor DTOtoDBO(GestorDTO entityDTO)
        {
            return new Gestor()
            {
                nInterno = entityDTO.nInterno
            };
        }

        public override GestorDTO DBOtoDTO(Gestor entity)
        {
            return new GestorDTO()
            {
                gestor = true,
                nInterno = (int)entity.nInterno
            };
        }

        #endregion Métodos abstractos de BaseRepository

        #region Métodos CRUD

        public override async Task<BaseDTO> Create(BaseDTO gestorDTO)
        {
            try
            {
                return await base.Create(gestorDTO);
            }
            catch (ElementAlreadyExists)
            {
                throw new ElementAlreadyExists($"El usuario con id {gestorDTO.GetPk()} ya fue marcado como gestor");
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
                throw new ElementNotFound($"El usuario con id {pk} no es gestor");
            }
        }

        public override async Task<BaseDTO> Update(BaseDTO gestorDTO)
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
                throw new ElementNotFound(String.Format("El usuario con id {0} no es gestor", id));
            }
        }

        #endregion Métodos CRUD
    }
}