using System;
using System.Security.Principal;
using GestionFicha.Entity;

namespace GestionFicha.Utils.Security
{
    public class Principal : IPrincipal
    {
        private bool Administrador { get; }

        private bool Gestor { get; }

        public IIdentity Identity { get; }

        public Principal(Usuario user, bool esAdmin, bool esGestor)
        {
            Identity = user;
            Administrador = esAdmin;
            Gestor = esGestor;
        }

        public Guid RequestIdentifier { get; }

        public Principal(Personal person, bool esAdmin, bool esGestor, Guid requestIdentifier, WindowsIdentity windowsIdentity = null)
        {
            Identity = new Usuario(person, windowsIdentity);
            Administrador = esAdmin;
            Gestor = esGestor;
            RequestIdentifier = requestIdentifier;
        }

        public bool IsInRole(string rol)
        {
            switch (rol)
            {
                case Constants.Roles.Administrador:
                    return Administrador;

                case Constants.Roles.Gestor:
                    return Gestor;

                default:
                    throw new NotImplementedException(String.Format("El rol {0} no está implementado", rol));
            }
        }
    }
}