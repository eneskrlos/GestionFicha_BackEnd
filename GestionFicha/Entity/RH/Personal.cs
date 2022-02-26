namespace GestionFicha.Entity
{
    using GestionFicha.Entity.RH;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PERSONAL")]
    public partial class Personal : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Personal()
        {
            subordinados = new HashSet<Personal>();
            ordenes = new HashSet<Orden>();
        }

        [Key]
        [Column("N_INTERNO", Order = 0, TypeName = "numeric")]
        public decimal nInterno { get; set; }

        [StringLength(60)]
        [Column("NOMBRE")]
        public string nombre { get; set; }

        [StringLength(60)]
        [Column("APELLIDOS")]
        public string apellidos { get; set; }

        [StringLength(15)]
        [Column("USUARIO_RED")]
        public string usuarioRed { get; set; }

        [Column("N_INTERNO_RESP", TypeName = "numeric")]
        public decimal? nInternoResp { get; set; }

        public virtual Administrador admin { get; set; }

        public virtual Gestor gestor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Personal> subordinados { get; set; }

        public virtual Personal responsable { get; set; }

        public virtual ICollection<Orden> ordenes { get; set; }

        /// <summary>
        /// M�todo que devuelve el valor de la llave primaria
        /// est� puesto que devuelve object pues tenemos
        /// llaves primarias que son int y otras que son
        /// strings.
        /// </summary>
        /// <returns></returns>
        public override object GetPk()
        {
            return nInterno;
        }
    }

    public class PersonalYAdmin
    {
        public Personal person { get; set; }
        public Administrador admin { get; set; }
        public Gestor gestor { get; set; }
    }
}