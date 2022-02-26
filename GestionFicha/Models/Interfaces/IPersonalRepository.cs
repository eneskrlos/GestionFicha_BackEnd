using System;
using System.Threading.Tasks;
using GestionFicha.Entity;
using GestionFicha.Models.DTO;
using GestionFicha.Services;

namespace GestionFicha.Models.Repositorios
{
    public interface IPersonalRepository : IBaseRepository<Personal, PersonalDTO, IPersonalService>
    {
        Task<PersonalConInfodeRolesDTO> DBOtoDTOconRoles(Personal persona);

        Task<PaginadorDTO> GetAll(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroPersonalDTO parametrosFiltro);

        Task<PaginadorDTO> GetAllConRoles(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroPersonalDTO parametrosFiltro);

        Task<Tuple<Personal, bool, bool>> ObtenerPersonaYRolGestor(int nInterno);

        Task<Tuple<Personal, bool, bool>> ObtenerPersonaYRolGestorDesdenUsuarioRed(string usuarioRed);
    }
}