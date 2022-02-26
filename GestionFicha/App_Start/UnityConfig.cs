using System.Web.Http;
using GestionFicha.Models.Interfaces;
using GestionFicha.Models.Repositorios;
using GestionFicha.Services;
using GestionFicha.Services.Interfaces;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace GestionFicha
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            container.RegisterType<IAdministradoresService, AdministradoresService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPersonalService, PersonalService>(new HierarchicalLifetimeManager());
            container.RegisterType<IGestoresService, GestoresService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProductoService, ProductoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOrdenService, OrdenService>(new HierarchicalLifetimeManager());

            container.RegisterType<IAdministradoresRepository, AdministradoresRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPersonalRepository, PersonalRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IGestoresRepository, GestoresRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IProductosRepository, ProductoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IOrdenRepository, OrdenRepository>(new HierarchicalLifetimeManager());


        }
    }
}