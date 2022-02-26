namespace GestionFicha.Models.DTO
{
    public abstract class BaseDTO
    {
        /// <summary>
        /// Método que devuelve el id del elemento utilizado para la URL
        /// </summary>
        /// <returns></returns>
        public abstract object GetPk();
    }

    /// <summary>
    /// DTO que sirve de base para serializar modelos con id
    /// </summary>
    /// <seealso cref="BaseDTO" />
    public abstract class BaseModeloConIdDTO : BaseDTO
    {
        public int id { get; set; }

        public override object GetPk()
        {
            return id;
        }
    }
}