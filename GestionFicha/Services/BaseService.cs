using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using GestionFicha.Entity;
using GestionFicha.Models.Repositorios;
using GestionFicha.Utils;
using static GestionFicha.Utils.Constants;

namespace GestionFicha.Services
{
    /// <summary>
    /// Clase base para la interacción con la base de datos
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que maneja este servicio. Debe ser una clase hija de BaseModel</typeparam>
    public abstract class BaseService<TEntity> : IDisposable, IBaseService<TEntity> where TEntity : BaseModel
    {
        // Contexto de los datos
        public GestionFichaContext db { get; }

        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="BaseService{TEntity}"/>.
        /// Genera un nuevo contexto de los datos
        /// </summary>
        public BaseService()
        {
            db = new GestionFichaContext();
        }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="BaseService{TEntity}"/>.
        /// </summary>
        /// <param name="db">El contexto de los datos.</param>
        public BaseService(GestionFichaContext db)
        {
            this.db = db;
        }

        #endregion Constructores

        #region Métodos abstractos

        /// <summary>
        /// Devuelve el DbSet asociado a este servicio
        /// </summary>
        /// <returns></returns>
        public abstract DbSet<TEntity> ObtenerDBSet();

        #endregion Métodos abstractos

        #region Métodos CRUD

        /// <summary>
        /// Inserta el objeto en la BD
        /// </summary>
        /// <param name="obj">El objeto que se quiere insertar.</param>
        /// <returns>El objeto con la PK ya puesta</returns>
        public virtual async Task<TEntity> Insertar(TEntity obj)
        {
            var _dbSet = ObtenerDBSet();
            _dbSet.Add(obj);

            try
            {
                await db.SaveChangesAsync();
                return obj;
            }
            catch (Exception e)
            {
                await ManejarErrorAlInsertar(e, obj);
                throw e;
            }
        }

        /// <summary>
        /// Devuelve todos los objetos del tipo dado
        /// </summary>
        public virtual async Task<List<TEntity>> ObtenerTodos()
        {
            var _dbSet = ObtenerDBSet();
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Devuelve el objeto cuya llave primaria es dada
        /// </summary>
        /// <param name="pk">La llave primaria.</param>
        /// <exception cref="ElementNotFound"></exception>
        public virtual async Task<TEntity> Obtener(params object[] pk)
        {
            var _dbSet = ObtenerDBSet();
            var obj = await _dbSet.FindAsync(pk);
            if (obj == null)
            {
                throw new ElementNotFound(String.Format("Entidad con id {0} no ha sido encontrado en la BD", pk));
            }
            return obj;
        }

        /// <summary>
        /// Saca el objeto del contexto (para que no sea trackeado()
        ///
        /// NOTA: Este método se utiliza cuando se saca un elemento de la BD
        /// mediante Obtener(id), luego ese objeto se sobreescribe por otro
        /// y al final se quiere hacer un Update.
        /// </summary>
        /// <param name="obj">El objeto</param>
        public virtual void Detach(TEntity obj)
        {
            db.Entry(obj).State = EntityState.Detached;
        }

        /// <summary>
        /// Actualiza una entidad en la BD
        /// </summary>
        /// <param name="obj">La entidad</param>
        /// <returns></returns>
        public virtual async Task<TEntity> Actualizar(TEntity obj)
        {
            db.Entry(obj).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await ManejarErrorAlActualizar(e, obj);
            }

            return obj;
        }

        /// <summary>
        /// Borra la entidad cuya llave primaria es dada
        /// </summary>
        /// <param name="pk">La llave primaria</param>
        /// <returns></returns>
        public virtual async Task Borrar(params object[] pk)
        {
            var _dbSet = ObtenerDBSet();
            var obj = await Obtener(pk);
            _dbSet.Remove(obj);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await ManejarErrorAlBorrar(e, obj);
            }
        }

        #endregion Métodos CRUD

        #region Otros

        /// <summary>
        /// Maneja los errores a la hora de insertar entidades en la BD.
        /// </summary>
        /// <param name="e">La excepción</param>
        /// <param name="obj">El objeto que provocó la excepción</param>
        /// <returns></returns>
        /// <exception cref="ElementAlreadyExists">Lanza esta excepción
        /// cuando se intenta crear una entidad que ya existe en la BD
        /// </exception>
        public virtual async Task ManejarErrorAlInsertar(Exception e, TEntity obj)
        {
            Detach(obj);
            if (e is DbEntityValidationException)
            {
                throw new ValidationError("Ha ocurrido un error de validación", e);
            }
            if (e is DbUpdateException)
            {
                var ex = e;
                while (ex != null)
                {
                    try
                    {
                        throw ex;
                    }
                    catch (System.Data.SqlClient.SqlException exx)
                    {
                        if (exx.Number == Constants.SqlErrorCodes.ConflictoDeUnicidad)
                        {
                            throw new ElementAlreadyExists("Ya existe una entidad como esta en la BD");
                        }
                        else if (exx.Number == Constants.SqlErrorCodes.ConflictoDeFK)
                        {
                            throw new ValidationError("Se ha producido un error de validación");
                        }
                        ex = ex.InnerException;
                    }
                    catch
                    {
                        ex = ex.InnerException;
                    }
                }
            }
            if (await EntidadExiste(obj))
            {
                throw new ElementAlreadyExists($"La entidad con id {obj.GetPk()} ya existe en la BD");
            }
            else
            {
                throw e;
            }
            throw e;
        }

        /// <summary>
        /// Maneja los errores a la hora de actualizar entidades en la BD.
        /// </summary>
        /// <param name="e">La excepción</param>
        /// <param name="obj">El objeto que provocó la excepción</param>
        /// <returns></returns>
        /// <exception cref="ValidationError">Ha ocurrido un error de validación</exception>
        /// <exception cref="ElementAlreadyExists">Ya existe una entidad como esta en la BD</exception>
        /// <exception cref="ElementNotFound">Lanza esta excepción
        /// cuando se intenta actualizar una entidad que no existe
        /// en la BD</exception>
        public virtual async Task ManejarErrorAlActualizar(Exception e, TEntity obj)
        {
            if (e is DbEntityValidationException)
            {
                throw new ValidationError("Ha ocurrido un error de validación", e);
            }
            if (e is DbUpdateException)
            {
                var ex = e;
                while (ex != null)
                {
                    try
                    {
                        throw ex;
                    }
                    catch (System.Data.SqlClient.SqlException exx)
                    {
                        if (exx.Number == Constants.SqlErrorCodes.ConflictoDeUnicidad)
                        {
                            throw new ElementAlreadyExists("Ya existe una entidad como esta en la BD");
                        }
                        else if (exx.Number == Constants.SqlErrorCodes.ConflictoDeFK)
                        {
                            throw new ValidationError("Se ha producido un error de validación");
                        }
                        ex = ex.InnerException;
                    }
                    catch
                    {
                        ex = ex.InnerException;
                    }
                }
            }
            if (!await EntidadExiste(obj))
            {
                throw new ElementNotFound($"La entidad con id {obj.GetPk()} ya existe en la BD");
            }
            else
            {
                throw e;
            }
        }

        public virtual async Task ManejarErrorAlBorrar(Exception e, TEntity obj)
        {
            if (e is DbEntityValidationException)
            {
                throw new ValidationError("Ha ocurrido un error de validación", e);
            }
            if (e is DbUpdateException)
            {
                var ex = e;
                while (ex != null)
                {
                    try
                    {
                        throw ex;
                    }
                    catch (System.Data.SqlClient.SqlException exx)
                    {
                        if (exx.Number == Constants.SqlErrorCodes.ConflictoDeFK)
                        {
                            throw new ElementoNoSePuedeBorrar("El entity no se puede borrar pues tiene entidades relacionadas");
                        }
                    }
                    catch
                    {
                        ex = ex.InnerException;
                    }
                }
            }
            throw e;
        }

        /// <summary>
        /// Indica si un objeto dado ya existe en la BD o no
        /// </summary>
        /// <param name="obj">El objeto.</param>
        public virtual async Task<bool> EntidadExiste(TEntity obj)
        {
            return await EntidadExiste(obj.GetPk());
        }

        /// <summary>
        /// Indica la entidad cuya llave primaria es pasada como
        /// argumento existe en la BD
        /// </summary>
        /// <param name="pk">La llave primaria</param>
        public virtual async Task<bool> EntidadExiste(object pk)
        {
            var _dbSet = ObtenerDBSet();
            return await _dbSet.FindAsync(pk) != null;
        }

        /// <summary>
        /// Libera los recursos que no son manejados por el objeto.
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }

        #endregion Otros
    }
}