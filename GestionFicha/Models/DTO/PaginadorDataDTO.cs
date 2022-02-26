using System.Collections.Generic;

namespace GestionFicha.Models.DTO
{
    public class PaginadorDTO
    {
        public List<BaseDTO> data { get; }

        public PaginadorMetaData metadata { get; }

        public PaginadorDTO(List<BaseDTO> resultados, PaginadorMetaData metadata)
        {
            this.data = resultados;
            this.metadata = metadata;
        }
    }

    public class PaginadorMetaData
    {
        public int cantidadTotal { get; set; }
        public int elementosPorPagina { get; set; }
        public int paginaActual { get; set; }
        public int totalDePaginas { get; set; }
        public bool paginaAnterior { get; set; }
        public bool paginaSiguiente { get; set; }
    }
}