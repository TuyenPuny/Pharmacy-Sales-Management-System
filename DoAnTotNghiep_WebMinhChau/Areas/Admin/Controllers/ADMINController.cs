using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    public class NhanVienController : Controller
    {
        private Model1 db = new Model1();

        // GET: Admin/ADMIN
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NhanVien()
        {
            return View();
        }
    }
}