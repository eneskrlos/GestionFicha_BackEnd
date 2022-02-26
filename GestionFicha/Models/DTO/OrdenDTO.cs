using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionFicha.Models.DTO
{
    public class OrdenDTO : BaseDTO
    {
        public int id_orden { get; set; }
        public DateTime fecha { get; set; }
        public string direccion { get; set; }
        public int cantidad { get; set; }
        public bool es_gestor { get; set; }
        public int id_producto { get; set; }
        public decimal nInterno { get; set; }

        public override object GetPk()
        {
            return id_orden;
        }
    }
}