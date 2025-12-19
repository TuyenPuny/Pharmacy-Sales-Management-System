using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using DoAnTotNghiep_WebMinhChau.Server;
using Unity;

namespace DoAnTotNghiep_WebMinhChau
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            var container = new UnityContainer();
            container.RegisterType<IVnPayServers, VnPayServer>();

            // Set Unity as the dependency resolver for MVC
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // Set Unity as the dependency resolver for Web API
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            UnityConfig.RegisterComponents();  // Đăng ký Dependency Injection          
        }
    }
}