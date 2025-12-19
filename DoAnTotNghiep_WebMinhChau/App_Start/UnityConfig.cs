using DoAnTotNghiep_WebMinhChau.Server;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace DoAnTotNghiep_WebMinhChau
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Register all components with the container here
            container.RegisterType<IVnPayServers, VnPayServer>();

            // Set up the dependency resolver for Web API
            //GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}