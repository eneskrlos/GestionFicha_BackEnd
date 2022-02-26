namespace GestionFicha.Models.DTO
{
    public class AdministradorDTO : BaseDTO
    {
        public int nInterno { get; set; }

        public bool administrador { get; set; }

        public override object GetPk()
        {
            return nInterno;
        }
    }
}