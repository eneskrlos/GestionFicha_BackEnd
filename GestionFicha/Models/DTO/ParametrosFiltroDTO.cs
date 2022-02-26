using System;

namespace GestionFicha.Models.DTO
{
    public class BaseParametrosFiltroDTO
    {
    }

    public class ParametrosFiltroPersonalDTO : BaseParametrosFiltroDTO
    {
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string busquedaGeneral { get; set; }
        public bool? esAdmin { get; set; }
        public bool? esGestor { get; set; }
        public string ordenarPor { get; set; } = "nInterno";
    }

    public class ParametrosFiltroProductoDTO : BaseParametrosFiltroDTO
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal? precio { get; set; }
        public string busquedaGeneral { get; set; }
        public string ordenarPor { get; set; } = "id_producto";
    }

    public class ParametrosFiltroOrdenDTO : BaseParametrosFiltroDTO
    {
        public DateTime fecha { get; set; }
        public string direccion { get; set; }
        public int? cantidad { get; set; }
        public string nombre { get; set; }
        public string busquedaGeneral { get; set; }
        public string ordenarPor { get; set; } = "id_orden";
    }
}