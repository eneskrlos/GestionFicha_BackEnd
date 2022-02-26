using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionFicha.Models.DTO;

namespace GestionFicha.Models.Repositorios
{
    public interface IBasicBaseRepository : IDisposable
    {
        Task<BaseDTO> Create(BaseDTO objDTO);

        Task Delete(int id);

        Task<List<BaseDTO>> GetAll();

        Task<BaseDTO> GetByPk(object pk);

        Task<BaseDTO> Update(BaseDTO objDTO);
    }
}