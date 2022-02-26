using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionFicha.Entity;
using GestionFicha.Models.DTO;

namespace GestionFicha.Services
{
    public interface IPersonalService : IBaseService<Personal>
    {
        Task<PersonalYAdmin> ObtenerConRol(int nInterno);

        Task<PersonalYAdmin> ObtenerConRolDesdeUsuarioRed(string usuarioRed);

        Task<PaginadorDTO> ObtenerTodosConRoles(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroPersonalDTO parametrosFiltro, Func<PersonalYAdmin, BaseDTO> func);

        Task<PaginadorDTO> ObtenerTodos(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroPersonalDTO parametrosFiltro, Func<Personal, PersonalDTO> func);

        Task<List<Personal>> ObtenerDesdeIds(decimal[] ids);
    }
}