using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionFicha.Entity
{
    [Table("GESTORES")]
    public partial class Gestor : BaseModel
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