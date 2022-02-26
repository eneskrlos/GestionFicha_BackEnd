using System.Collections.Generic;
using System.Threading.Tasks;
using GestionFicha.Entity;
using GestionFicha.Models.DTO;
using GestionFicha.Services;

namespace GestionFicha.Models.Repositorios
{
    public interface IBaseRepository<TEntity, TEntityDTO, TService> : IBasicBaseRepository
        where TEntity : BaseModel
        where TEntityDTO : BaseDTO
        where TService : IBaseService<TEntity>
    {
        TEntityDTO DBOtoDTO(TEntity entity);

        TEntity DTOtoDBO(TEntityDTO entityDTO);

        Task<TEntityDTO> Create(TEntityDTO objDTO);

        Task<TEntityDTO> Update(TEntityDTO objDTO);

        void PreCreateValidation(TEntityDTO entityDTO, TEntity entity);

        Task PreCreateValidationAsync(TEntityDTO entityDTO, TEntity entity);

        void PreUpdateValidation(TEntityDTO entityDTO, TEntity entity);

        Task PreUpdateValidationAsync(TEntityDTO entityDTO, TEntity entity);
    }

    public interface IBasicRepositoryConUsuario : IBasicBaseRepository
    {
        Task<BaseDTO> Create(BaseDTO objDTO, Personal usuario, bool esGestor);

        Task Delete(int id, Personal usuario, bool esGestor);

        Task<List<BaseDTO>> GetAll(Personal usuario, bool esGestor);

        Task<BaseDTO> GetByPk(object pk, Personal usuario, bool esGestor);

        Task<BaseDTO> Update(BaseDTO objDTO, Personal usuario, bool esGestor);
    }
}