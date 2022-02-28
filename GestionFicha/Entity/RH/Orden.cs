using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionFicha.Entity.RH
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ORDEN")]
    public partial class Orden : BaseModel
    {
        [Key]
        [Column("ID_ORDEN")]
        public int id_orden { get; set; }

        [Column("FECHA")]
        public DateTime fecha { get; set; }

        [Column("DIRECCION")]
        public string direccion { get; set; }

        [Column("CANTIDAD")]
        public int cantidad { get; set; }

        [Column("ES_GESTOR", TypeName = "bit")]
        public bool es_gestor { get; set; }
        
        [ForeignKey("producto")]
        [Column("ID_PRODUCTO")]
        public int id_producto { get; set; }

        [ForeignKey("personal")]
        [Column("N_INTERNO", TypeName = "numeric")]
        public decimal nInterno { get; set; }

        public virtual Producto producto { get; set; }

        public virtual Personal personal { get; set; }

        public override object GetPk()
        {
            return id_orden;
        }
    }

    public class OrdenProductoPersonal
    {
        public Orden orden { get; set; }
        public Producto producto { get; set; }
        public Personal persona { get; set; }
    }
}