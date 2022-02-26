namespace GestionFicha.Models.DTO
{
    public class PersonalDTO : BaseDTO
    {
        public int nInterno { get; set; }

        public string nombre { get; set; }

        public string apellidos { get; set; }

        public string usuarioRed { get; set; }

        public int? nInternoResp { get; set; }

        public override object GetPk()
        {
            return nInterno;
        }
    }

    public class RolesDTO
    {
        public bool esGestor { get; set; }
        public bool esAdmin { get; set; }
    }

    public class PersonalConInfodeRolesDTO : PersonalDTO
    {
        public RolesDTO roles { get; set; }

        public PersonalConInfodeRolesDTO()
        {
        }

        public PersonalConInfodeRolesDTO(PersonalDTO personalDTO, RolesDTO roles)
        {
            nInterno = personalDTO.nInterno;
            nombre = personalDTO.nombre;
            apellidos = personalDTO.apellidos;
            usuarioRed = personalDTO.usuarioRed;
            nInternoResp = personalDTO.nInternoResp;
            this.roles = roles;
        }
    }
}