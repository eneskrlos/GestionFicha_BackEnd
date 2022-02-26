using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionFicha.Models.DTO
{
    public class ProductoDTO : BaseDTO
    {
        public int id_producto { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }

        public override object GetPk()
        {
            return id_producto;
        }
    }
}