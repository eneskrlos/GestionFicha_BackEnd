namespace GestionFicha.Models.DTO
{
    public class GestorDTO : BaseDTO
    {
        public int nInterno { get; set; }

        public bool gestor { get; set; }

        public override object GetPk()
        {
            return nInterno;
        }
    }
}