using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionFicha.Entity.RH
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRODUCTO")]
    public partial class Producto : BaseModel
    {
        [Key]
        [Column("ID_PRODUCTO")]
        public int id_producto { get; set; }

        [StringLength(150)]
        [Column("NOMBRE")]
        public string nombre { get; set; }

        [StringLength(60)]
        [Column("DESCRIPCION")]
        public string descripcion { get; set; }

        [Column("PRECIO", TypeName = "numeric")]
        public decimal precio { get; set; }

        public override object GetPk()
        {
            return id_producto;
        }
    }
}