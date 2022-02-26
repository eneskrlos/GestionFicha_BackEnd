using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionFicha.Entity;
using GestionFicha.Models.DTO;
using GestionFicha.Services;
using GestionFicha.Utils;
using static GestionFicha.Utils.Constants;

namespace GestionFicha.Models.Repositorios
{
    public abstract class NewBaseRepository<TEntity, TEntityDTO, TService> : IDisposable, IBaseRepository<TEntity, TEntityDTO, TService> where TEntity : BaseModel where TEntityDTO : BaseDTO where TService : IBaseService<TEntity>
    {
        public TService Service;

        #region Métodos abstractos

        /// <summary>
        /// Convierte un objeto del modelo en un DTO
        /// </summary>
        /// <param name="entity">El entity.</param>
        /// <returns></returns>
        public abstract TEntityDTO DBOtoDTO(TEntity entity);

        /// <summary>
        /// Convierte un DTO en un objeto del modelo
        /// </summary>
        /// <param name="entityDTO">El DTO.</param>
        /// <returns></returns>
        public abstract TEntity DTOtoDBO(TEntityDTO entityDTO);

        #endregion Métodos abstractos

        #region Métodos CRUD

        /// <summary>
        /// Crea un entity a partir del DTO
        /// </summary>
        /// <param name="objDTO">El DTO.</param>
        /// <returns></returns>
        public virtual async Task<BaseDTO> Create(BaseDTO objDTO)
        {
            var _objDTO = (TEntityDTO)objDTO;

            return await Create(_objDTO);
        }

        /// <summary>
        /// Crea un entity a partir del DTO
        /// </summary>
        /// <param name="objDTO">El DTO.</param>
        /// <returns></returns>
        public virtual async Task<TEntityDTO> Create(TEntityDTO objDTO)
        {
            var obj = DTOtoDBO(objDTO);

            await PreCreateValidationAsync(objDTO, obj);

            await Service.Insertar(obj);

            return DBOtoDTO(obj);
        }

        /// <summary>
        /// Devuelve todos los DTOs de todos los entities
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<BaseDTO>> GetAll()
        {
            List<BaseDTO> lista = new List<BaseDTO> { };

            var list = await Service.ObtenerTodos();
            list.ForEach(x => lista.Add(DBOtoDTO(x)));

            return lista;
        }

        /// <summary>
        /// Devuelve el DTO del objeto cuyo id es dado
        /// </summary>
        /// <param name="pk">El pk.</param>
        /// <returns></returns>
        public virtual async Task<BaseDTO> GetByPk(object pk)
        {
            return DBOtoDTO(await Service.Obtener(pk));
        }

        /// <summary>
        /// Actualiza el objeto referenciado por el DTO
        /// </summary>
        /// <param name="objDTO">El DTO.</param>
        /// <returns></returns>
        public virtual async Task<BaseDTO> Update(BaseDTO objDTO)
        {
            var _objDTO = (TEntityDTO)objDTO;

            return await Update(_objDTO);
        }

        /// <summary>
        /// Actualiza el objeto referenciado por el DTO
        /// </summary>
        /// <param name="objDTO">El DTO.</param>
        /// <returns></returns>
        public virtual async Task<TEntityDTO> Update(TEntityDTO objDTO)
        {
            var obj = await Service.Obtener(objDTO.GetPk());

            Service.Detach(obj);

            if (obj == null)
            {
                throw new ElementNotFound(String.Format("Entidad con id {0} no fue encontrada en la DB", objDTO.GetPk()));
            }

            await PreUpdateValidationAsync(objDTO, obj);

            obj = DTOtoDBO(objDTO);

            await Service.Actualizar(obj);

            // No hay necesidad de crear un nuevo DTO
            return objDTO;
        }

        /// <summary>
        /// Elimina el objeto cuyo id es dado
        /// </summary>
        /// <param name="id">El id.</param>
        /// <returns></returns>
        public virtual async Task Delete(int id)
        {
            await Service.Borrar(id);
        }

        #endregion Métodos CRUD

        #region Validaciones

        /// <summary>
        /// Validaciones antes de crear un entity
        /// </summary>
        /// <param name="entityDTO">El DTO.</param>
        /// <param name="entity">El entity.</param>
        /// <returns></returns>
        public virtual async Task PreCreateValidationAsync(TEntityDTO entityDTO, TEntity entity)
        {
            // Esta validación no es asíncrona, pero está definida así de modo que cada subclase la sobrescriba
            PreCreateValidation(entityDTO, entity);
        }

        /// <summary>
        /// Validaciones antes de crear un entity
        /// </summary>
        /// <param name="entityDTO">El DTO.</param>
        /// <param name="entity">El entity.</param>
        /// <returns></returns>
        public virtual void PreCreateValidation(TEntityDTO entityDTO, TEntity entity)
        {
            // Por defecto no hacemos ninguna validación antes del create
        }

        /// <summary>
        /// Validaciones antes de actualizar un entity
        /// </summary>
        /// <param name="entityDTO">El DTO con la información a modificar.</param>
        /// <param name="entity">El entity al que referencia el DTO tal cual está en la BD.</param>
        /// <returns></returns>
        public virtual async Task PreUpdateValidationAsync(TEntityDTO entityDTO, TEntity entity)
        {
            // Esta validación no es asíncrona, pero está definida así de modo que cada subclase la sobrescriba
            PreUpdateValidation(entityDTO, entity);
        }

        /// <summary>
        /// Validaciones antes de actualizar un entity
        /// </summary>
        /// <param name="entityDTO">El DTO con la información a modificar.</param>
        /// <param name="entity">El entity al que referencia el DTO tal cual está en la BD.</param>>
        /// <returns></returns>
        public virtual void PreUpdateValidation(TEntityDTO entityDTO, TEntity entity)
        {
            // Por defecto no hacemos ninguna validación antes del update
        }

        #endregion Validaciones

        #region Otros

        /// <summary>
        /// Libera los recursos que no son manejados por el objeto.
        /// </summary>
        public virtual void Dispose()
        {
            if (Service != null)
            {
                Service.Dispose();
            }
        }

        #endregion Otros
    }

    #region Excepciones

    /// <summary>
    /// El elemento no se puede borrar
    /// </summary>
    /// <seealso cref="BaseException" />
    public class ElementoNoSePuedeBorrar : BaseException
    {
        public ElementoNoSePuedeBorrar(string message) : base(message)
        {
        }

        public ElementoNoSePuedeBorrar(string message, Exception baseException) : base(message, baseException)
        {
        }
    }

    /// <summary>
    /// El elemento que se busca no existe en la BD
    /// </summary>
    /// <seealso cref="BaseException" />
    public class ElementNotFound : BaseException
    {
        public ElementNotFound(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// El elemento que se está tratando de insertar ya existe en la BD
    /// </summary>
    /// <seealso cref="BaseException" />
    public class ElementAlreadyExists : BaseException
    {
        public ElementAlreadyExists(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Excepción base para todos los errores de validación en los modelos
    /// </summary>
    /// <seealso cref="BaseException" />
    public class ValidationError : BaseException
    {
        public ValidationError(string message) : base(message)
        {
        }

        public ValidationError(string message, Exception e) : base(message, e)
        {
        }
    }

    /// <summary>
    /// El usuario no tiene permisos para realizar la acción
    /// </summary>
    /// <seealso cref="BaseException" />
    public class UnauthorizedAccess : BaseException
    {
        public UnauthorizedAccess(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// La llave primaria se ha definido antes de la inserción
    /// </summary>
    /// <seealso cref="ValidationError" />
    public class PrimaryKeyDefined : ValidationError
    {
        public PrimaryKeyDefined() : base("La llave primaria no puede definirse antes de insertar")
        {
        }

        public PrimaryKeyDefined(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Alguna entidad relacionada no ha sido encontrada en la BD
    /// </summary>
    /// <seealso cref="ValidationError" />
    public class EntidadRelacionadaNoEncontrada : ValidationError
    {
        public EntidadRelacionadaNoEncontrada(string message, Exception e) : base(message, e)
        {
        }

        public EntidadRelacionadaNoEncontrada(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Se está intentando editar una propiedad que ya no es editable
    /// </summary>
    /// <seealso cref="ValidationError" />
    public class PropiedadNoEditable : ValidationError
    {
        public PropiedadNoEditable(string message, Exception e) : base(message, e)
        {
        }

        public PropiedadNoEditable(string message) : base(message)
        {
        }

        public PropiedadNoEditable() : base("Está intentando modificar una propiedad que ya no es editable")
        {
        }
    }

    /// <summary>
    /// El elemento no es editable
    /// </summary>
    /// <seealso cref="BaseException" />
    public class ElementoNoEditable : BaseException
    {
        public ElementoNoEditable(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Un parámetro es inválido
    /// </summary>
    /// <seealso cref="BaseException" />
    public class InvalidParameter : BaseException
    {
        public InvalidParameter(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Se intenta desactivar un mantenimiento maestro que ya está inactivo
    /// </summary>
    /// <seealso cref="BaseException" />
    public class ElementoYaEstaInactivo : BaseException
    {
        public ElementoYaEstaInactivo(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Se intenta hacer un cambio de estado no permitido
    /// </summary>
    /// <seealso cref="ValidationError" />
    public class CambioDeEstadoNoPermitido : ValidationError
    {
        public CambioDeEstadoNoPermitido(string message) : base(message)
        {
        }
    }

    public class PermisoNoDefinido: ValidationError
    {
        public PermisoNoDefinido(string message) : base(message)
        {
        }

        public PermisoNoDefinido(int idPermiso, int idConvenio) : base($"El permiso {idPermiso} no está definido para el convenio {idConvenio}")
        {
        }
    }

    /// <summary>
    /// La entidad ha sido modificada por otro usuario
    /// </summary>
    /// <seealso cref="BaseException" />
    public class ErrorDeConcurrencia : BaseException
    {
        public ErrorDeConcurrencia(string message, Exception e) : base(message, e)
        {
        }

        public ErrorDeConcurrencia(string message) : base(message)
        {
        }
    }

    #endregion Excepciones
}