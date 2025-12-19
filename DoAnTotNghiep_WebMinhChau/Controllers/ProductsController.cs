using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep_WebMinhChau.Models;

namespace DoAnTotNghiep_WebMinhChau.Controllers
{
    public class ProductsController : Controller
    {
        private DBContext db = new DBContext();
        private readonly SanPham_CTSP model = new SanPham_CTSP();
        public ActionResult chiTietSanPham(string id)
        {
            SanPham_CTSP sanPham_CTSP = new SanPham_CTSP();
            var chiTietSanPham = new SanPham_CTSP.CTSanPham();
            chiTietSanPham.loadCTSP(id, out sanPham_CTSP);
            if (sanPham_CTSP == null || sanPham_CTSP.SanPham == null)
            {
                return HttpNotFound("Không tìm thấy sản phẩm");
            }
            return View(sanPham_CTSP);
        }
    }
}