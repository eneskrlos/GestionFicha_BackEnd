using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionFicha.Entity
{
    /// <summary>
    /// Clase base de todos nuestros modelos
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        /// Método que devuelve el valor de la llave primaria
        /// está puesto que devuelve object pues tenemos
        /// llaves primarias que son int y otras que son
        /// strings.
        /// </summary>
        /// <returns></returns>
        public abstract object GetPk();
    }

    /// <summary>
    /// Clase base para todas las entidades con id
    /// </summary>
    /// <seealso cref="BaseModel" />
    public abstract class BaseModelConId : BaseModel
    {
        [Key]
        [Column("ID")]
        public int id { get; set; }

        /// <summary>
        /// Método que devuelve el valor de la llave primaria
        /// está puesto que devuelve object pues tenemos
        /// llaves primarias que son int y otras que son
        /// strings.
        /// </summary>
        /// <returns></returns>
        public override object GetPk()
        {
            return id;
        }
    }
}