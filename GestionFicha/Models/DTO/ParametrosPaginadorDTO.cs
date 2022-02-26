namespace GestionFicha.Models.DTO
{
    public class ParametrosPaginadorDTO
    {
        public uint paginaActual { get; set; } = 1;

        public uint elementosPorPagina { get; set; } = 10;
    }
}