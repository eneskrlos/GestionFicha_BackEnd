using System;
using System.Security.Principal;
using GestionFicha.Entity;

namespace GestionFicha.Utils.Security
{
    public class Usuario : IIdentity
    {
        public string Name { get; }

        public string AuthenticationType { get; }

        public bool IsAuthenticated { get; }

        public Personal Persona { get; }

        public WindowsIdentity windowsIdentity { get; }

        public Usuario(Personal persona, WindowsIdentity windowsIdentity = null)
        {
            Name = String.Format("{0} {1}", persona.nombre, persona.apellidos);
            AuthenticationType = "Header";
            IsAuthenticated = true;
            Persona = persona;
            if (windowsIdentity != null)
            {
                this.windowsIdentity = windowsIdentity;
            }
        }
    }
}