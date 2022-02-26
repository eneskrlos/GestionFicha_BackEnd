using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using GestionFicha.Entity;

namespace GestionFicha.Services
{
    /// <summary>
    /// Interface base para la interacción con la base de datos
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que maneja este servicio. Debe ser una clase hija de BaseModel</typeparam>
    public interface IBaseService<TEntity> where TEntity : BaseModel
    {
        GestionFichaContext db { get; }

        Task Borrar(params object[] pk);

        Task<TEntity> Insertar(TEntity obj);

        Task ManejarErrorAlActualizar(Exception e, TEntity obj);

        Task ManejarErrorAlBorrar(Exception e, TEntity obj);

        Task ManejarErrorAlInsertar(Exception e, TEntity obj);

        Task<TEntity> Obtener(params object[] pk);

        void Detach(TEntity obj);

        DbSet<TEntity> ObtenerDBSet();

        Task<List<TEntity>> ObtenerTodos();

        Task<TEntity> Actualizar(TEntity obj);

        Task<bool> EntidadExiste(object pk);

        Task<bool> EntidadExiste(TEntity obj);

        void Dispose();
    }
}