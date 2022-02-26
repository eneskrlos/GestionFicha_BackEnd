using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using GestionFicha.Entity;
using GestionFicha.Models.DTO;
using GestionFicha.Models.Repositorios;
using GestionFicha.Utils;

namespace GestionFicha.Services
{
    public class PersonalService : BaseService<Personal>, IPersonalService
    {
        public override DbSet<Personal> ObtenerDBSet()
        {
            return db.Personal;
        }

        public async Task<PersonalYAdmin> ObtenerConRol(int nInterno)
        {
            var query = db.Personal.Where(x => x.nInterno == nInterno)
                .Include(x => x.admin)
                .Include(x => x.gestor)
                .DefaultIfEmpty();

            var obj = await query.FirstOrDefaultAsync();

            if (obj == null)
            {
                throw new ElementNotFound(String.Format("El usuario con id {0} no fue encontrado en la BD", nInterno));
            }

            return new PersonalYAdmin() { person = obj, admin = obj.admin, gestor = obj.gestor };
        }

        public async Task<PersonalYAdmin> ObtenerConRolDesdeUsuarioRed(string usuarioRed)
        {
            var query = db.Personal.Where(x => x.usuarioRed == usuarioRed)
                .Include(x => x.admin)
                .Include(x => x.gestor)
                .DefaultIfEmpty();

            var obj = await query.FirstOrDefaultAsync();

            if (obj == null)
            {
                throw new ElementNotFound(String.Format("El usuario de red {0} no fue encontrado en la BD", usuarioRed));
            }
            return new PersonalYAdmin() { person = obj, admin = obj.admin, gestor = obj.gestor };
        }

        public async Task<PaginadorDTO> ObtenerTodosConRoles(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroPersonalDTO parametrosFiltro, Func<PersonalYAdmin, BaseDTO> func)
        {
            var query = from person in db.Personal
                        from admin in db.Administradores
                        .Where(x => x.nInterno == person.nInterno).DefaultIfEmpty()
                        from gestor in db.Gestores
                        .Where(x => x.nInterno == person.nInterno).DefaultIfEmpty()
                        select new PersonalYAdmin()
                        {
                            person = person,
                            admin = admin,
                            gestor = gestor
                        };

            // Si tenemos filtros, los procesamos
            if (parametrosFiltro != null)
            {
                query = ProcesarFiltro(query, parametrosFiltro);
            }
            else
            {
                // De no haber filtro, utilizamos el orden por defecto
                query = query.OrderBy(x => x.person.nombre).ThenBy(x => x.person.apellidos);
            }

            return await Paginador.ProcesarPaginador(query, parametrosPaginador, func);
        }

        public async Task<PaginadorDTO> ObtenerTodos(ParametrosPaginadorDTO parametrosPaginador, ParametrosFiltroPersonalDTO parametrosFiltro, Func<Personal, PersonalDTO> func)
        {
            var personasQuery = ObtenerDBSet().AsQueryable();

            // Si tenemos filtros, los procesamos
            if (parametrosFiltro != null)
            {
                personasQuery = ProcesarFiltro(personasQuery, parametrosFiltro);
            }
            else
            {
                // De no haber filtro, utilizamos el orden por defecto
                personasQuery = personasQuery.OrderBy(x => x.nombre).ThenBy(x => x.apellidos);
            }

            return await Paginador.ProcesarPaginador(personasQuery, parametrosPaginador, func);
        }

        public IQueryable<PersonalYAdmin> ProcesarFiltro(IQueryable<PersonalYAdmin> query, ParametrosFiltroPersonalDTO parametrosFiltro)
        {
            var ordenDescendente = parametrosFiltro.ordenarPor.StartsWith("-");
            parametrosFiltro.ordenarPor = parametrosFiltro.ordenarPor.Split('-').Last();
            var ordenesPermitidos = new string[]
            {
                "nombre", "apellidos", "nInterno", "usuarioRed"
            };
            if (!ordenesPermitidos.Contains(parametrosFiltro.ordenarPor))
            {
                throw new InvalidParameter(String.Format("No es posible ordenar por {0}", parametrosFiltro.ordenarPor));
            }

            if (parametrosFiltro.busquedaGeneral != null)
            {
                var param = parametrosFiltro.busquedaGeneral.Trim();
                query = query.Where(x => (x.person.nombre + " " + x.person.apellidos).Contains(param));
            }

            if (parametrosFiltro.nombre != null)
            {
                query = query.Where(x => x.person.nombre.Contains(parametrosFiltro.nombre));
            }
            if (parametrosFiltro.apellidos != null)
            {
                query = query.Where(x => x.person.apellidos.Contains(parametrosFiltro.apellidos));
            }
            if (parametrosFiltro.esAdmin != null)
            {
                var esAdmin = parametrosFiltro.esAdmin;
                query = query.Where(x => (x.admin != null) == esAdmin);
            }
            if (parametrosFiltro.esGestor != null)
            {
                var esGestor = parametrosFiltro.esGestor;
                query = query.Where(x => (x.gestor != null) == esGestor);
            }
            if (parametrosFiltro.ordenarPor == "nombre")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.person.nombre).ThenByDescending(x => x.person.apellidos) : query.OrderBy(x => x.person.nombre).ThenBy(x => x.person.apellidos);
            }
            if (parametrosFiltro.ordenarPor == "apellidos")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.person.apellidos).ThenByDescending(x => x.person.nombre) : query.OrderBy(x => x.person.apellidos).ThenBy(x => x.person.nombre);
            }
            if (parametrosFiltro.ordenarPor == "nInterno")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.person.nInterno) : query.OrderBy(x => x.person.nInterno);
            }
            if (parametrosFiltro.ordenarPor == "usuarioRed")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.person.usuarioRed) : query.OrderBy(x => x.person.usuarioRed);
            }

            return query;
        }

        public IQueryable<Personal> ProcesarFiltro(IQueryable<Personal> query, ParametrosFiltroPersonalDTO parametrosFiltro)
        {
            var ordenDescendente = parametrosFiltro.ordenarPor.StartsWith("-");
            parametrosFiltro.ordenarPor = parametrosFiltro.ordenarPor.Split('-').Last();
            var ordenesPermitidos = new string[]
            {
                "nombre", "apellidos", "nInterno", "usuarioRed"
            };
            if (!ordenesPermitidos.Contains(parametrosFiltro.ordenarPor))
            {
                throw new InvalidParameter(String.Format("No es posible ordenar por {0}", parametrosFiltro.ordenarPor));
            }

            if (parametrosFiltro.busquedaGeneral != null)
            {
                var param = parametrosFiltro.busquedaGeneral.Trim();
                query = query.Where(x => (x.nombre + " " + x.apellidos).Contains(param));
            }

            if (parametrosFiltro.nombre != null)
            {
                query = query.Where(x => x.nombre.Contains(parametrosFiltro.nombre));
            }
            if (parametrosFiltro.ordenarPor == "nombre")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.nombre).ThenByDescending(x => x.apellidos) : query.OrderBy(x => x.nombre).ThenBy(x => x.apellidos);
            }
            if (parametrosFiltro.apellidos != null)
            {
                query = query.Where(x => x.apellidos.Contains(parametrosFiltro.apellidos));
            }
            if (parametrosFiltro.ordenarPor == "apellidos")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.apellidos).ThenByDescending(x => x.nombre) : query.OrderBy(x => x.apellidos).ThenBy(x => x.nombre);
            }
            if (parametrosFiltro.ordenarPor == "nInterno")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.nInterno) : query.OrderBy(x => x.nInterno);
            }
            if (parametrosFiltro.ordenarPor == "usuarioRed")
            {
                query = ordenDescendente ? query.OrderByDescending(x => x.usuarioRed) : query.OrderBy(x => x.usuarioRed);
            }

            return query;
        }

        public async Task<List<Personal>> ObtenerDesdeIds(decimal[] ids)
        {
            return await db.Personal.Where(x => ids.Contains(x.nInterno)).ToListAsync();
        }
    }
}