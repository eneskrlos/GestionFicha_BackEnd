using System.Data.Entity;
using GestionFicha.Entity;

namespace GestionFicha.Services
{
    public class GestoresService : BaseService<Gestor>, IGestoresService
    {
        public override DbSet<Gestor> ObtenerDBSet()
        {
            return db.Gestores;
        }
    }
}