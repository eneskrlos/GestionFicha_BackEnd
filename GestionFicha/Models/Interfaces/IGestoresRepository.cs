using GestionFicha.Entity;
using GestionFicha.Models.DTO;
using GestionFicha.Services;

namespace GestionFicha.Models.Repositorios
{
    public interface IGestoresRepository : IBaseRepository<Gestor, GestorDTO, IGestoresService>
    {
    }
}