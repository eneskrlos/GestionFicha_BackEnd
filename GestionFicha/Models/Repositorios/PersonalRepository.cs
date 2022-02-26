using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestionFicha.Entity;
using GestionFicha.Models.DTO;
using GestionFicha.Services;

namespace GestionFicha.Models.Repositorios
{
    public class PersonalRepository : NewBaseRepository<Personal, PersonalDTO, IPersonalService>, IPersonalRepository
    {
        private IAdministradoresRepository administradoresRepository;

        public PersonalRepository()
        {
            Service = new PersonalService();
            administradoresRepository = new AdministradoresRepository();
        }

        public PersonalRepository(IPersonalService service)
        {
            Service = service;
            administradoresRepository = new AdministradoresRepository();
        }

        public PersonalRepository(IPersonalService service, IAdministradoresRepository administradoresRepository)
        {
            Service = service;
            this.administradoresRepository = administradoresRepository;
        }

        #region Métodos abstractos de BaseRepository

        public override PersonalDTO DBOtoDTO(Personal persona)
        {
            return new PersonalDTO()
            {
                nInterno = (int)persona.nInterno,
                nombre = persona.nombre,
                apellidos = persona.apellidos,
                usuarioRed = persona.usuarioRed,
                nInternoResp = persona.nInternoResp != null ? (int)persona.nInternoResp : new int?()
            };
        }

        public override Personal DTOtoDBO(PersonalDTO personaDTO)
        {
            return new Personal()
            {
                nInterno = personaDTO.nInterno,
                nombre = personaDTO.nombre,
                apellidos = personaDTO.apellidos,
                usuarioRed = personaDTO.usuarioRed,
                nInternoResp = personaDTO.nInternoResp
            };
        }

        #endregion Métodos abstractos de BaseRepository

        #region Métodos CRUD

        public override async Task<BaseDTO> Create(BaseDTO ejercicioDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginadorDTO> GetAllConRoles(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroPersonalDTO parametrosFiltro)
        {
            return await Service.ObtenerTodosConRoles(parametrosPaginador, parametrosFiltro, (obj) => new PersonalConInfodeRolesDTO(DBOtoDTO(obj.person), new RolesDTO() { esAdmin = obj.admin != null, esGestor = obj.gestor != null }));
        }

        public override async Task<List<BaseDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PaginadorDTO> GetAll(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroPersonalDTO parametrosFiltro)
        {
            return await Service.ObtenerTodos(parametrosPaginador, parametrosFiltro, DBOtoDTO);
        }

        public override async Task<BaseDTO> GetByPk(object pk)
        {
            var obj = await Service.Obtener(pk);
            return await (obj != null ? DBOtoDTOconRoles(obj) : null);
        }

        public override async Task<BaseDTO> Update(BaseDTO ejercicioDTO)
        {
            throw new NotImplementedException();
        }

        public override async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        #endregion Métodos CRUD

        #region Otros

        /// <summary>
        /// Devuelve una tupla, donde el primer elemento es el objeto Personal cuyo nInterno fue es el dado y el segundo es un boolean
        /// el cual indica si dicho usuario es Gestor o no.
        /// </summary>
        /// <param name="nInterno">Se lanza cuando no se encuentra ningún usuario con ese nInterno en la BD.</param>
        /// <returns></returns>
        public async Task<Tuple<Personal, bool, bool>> ObtenerPersonaYRolGestor(int nInterno)
        {
            var obj = await Service.ObtenerConRol(nInterno);
            return new Tuple<Personal, bool, bool>(obj.person, obj.admin != null, obj.gestor != null);
        }

        public async Task<Tuple<Personal, bool, bool>> ObtenerPersonaYRolGestorDesdenUsuarioRed(string usuarioRed)
        {
            var obj = await Service.ObtenerConRolDesdeUsuarioRed(usuarioRed);
            return new Tuple<Personal, bool, bool>(obj.person, obj.admin != null, obj.gestor != null);
        }

        public async Task<PersonalConInfodeRolesDTO> DBOtoDTOconRoles(Personal persona)
        {
            var _person = await Service.ObtenerConRol((int)persona.nInterno);

            return new PersonalConInfodeRolesDTO(DBOtoDTO(_person.person),
                new RolesDTO() { esAdmin = _person.admin != null, esGestor = _person.gestor != null });
        }

        /// <summary>
        /// Libera los recursos que no son manejados por el objeto.
        /// </summary>
        public override void Dispose()
        {
            // Liberamos el servicio
            Service.Dispose();
            // Liberamos el repositorio de administradores
            administradoresRepository.Dispose();
        }

        #endregion Otros
    }
}