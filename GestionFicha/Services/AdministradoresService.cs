using System.Data.Entity;
using GestionFicha.Entity;

namespace GestionFicha.Services
{
    public class AdministradoresService : BaseService<Administrador>, IAdministradoresService
    {
        public override DbSet<Administrador> ObtenerDBSet()
        {
            return db.Administradores;
        }
    }
}