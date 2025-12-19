using System.Web.Mvc;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "BaoCao_ThongKe", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers" }
            );
        }
    }
}