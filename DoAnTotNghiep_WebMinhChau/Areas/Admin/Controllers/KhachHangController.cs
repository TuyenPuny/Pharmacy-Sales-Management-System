using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        private Model1 db = new Model1();
        // GET: Admin/KhachHang
        public ActionResult KhachHang(string searchTerm)
        {
            ViewBag.SLKH = (int)db.Database.SqlQuery<int>("SELECT dbo.KH()").FirstOrDefault();

            var model = db.KhachHang.AsQueryable();

           
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(dm => dm.SDT.Contains(searchTerm));
            }
            return View(model.ToList());
        }

    }
}