namespace GestionFicha.Entity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ADMINISTRADORES")]
    public partial class Administrador : BaseModel
    {
        [Key]
        [Column("ID_PERSONA", TypeName = "numeric")]
        public decimal nInterno { get; set; }

        public virtual Personal Persona { get; set; }

        public override object GetPk()
        {
            return nInterno;
        }
    }
}