using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    public class NhaSanXuatController : Controller
    {
        private Model1 db = new Model1();
        // GET: Admin/NhaSanXuat
        public ActionResult NhaSanXuat(string searchTerm, string dc)
        {
            ViewBag.SLNSX = (int)db.Database.SqlQuery<int>("SELECT dbo.NSX()").FirstOrDefault();
            var model = db.NhaSanXuat.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(dm => dm.TenNSX.Contains(searchTerm));
            }
            if (!string.IsNullOrEmpty(dc))
            {
                model = model.Where(dm => dm.DiaChi.Contains(dc));
            }
            return View(model.ToList());
        }

        //-----------------------------------------------------------Thêm nhà sản xuất----------------------------------------------------------

        // GET: Admin/NhaSanXuat/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NhaSanXuat/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NhaSanXuat nhaSanXuat)
        {
            if (ModelState.IsValid)
            {
                db.NhaSanXuat.Add(nhaSanXuat);
                db.SaveChanges();
                return RedirectToAction("NhaSanXuat");
            }
            return View(nhaSanXuat);
        }

        //-----------------------------------------------------------Tìm kiếm nhà sản xuất----------------------------------------------------------

        public ActionResult Search(string searchTerm)
        {
            var nhaCungCaps = db.NhaSanXuat.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                nhaCungCaps = nhaCungCaps.Where(ncc => ncc.TenNSX.Contains(searchTerm));
            }

            return View("Index", nhaCungCaps.ToList()); // Hoặc tên view phù hợp của bạn
        }


        //-----------------------------------------------------------Sửa nhà sản xuất----------------------------------------------------------

        // GET: Admin/NhaSanXuat/Edit/
        public ActionResult Edit(string id)
        {
            var nhaSanXuat = db.NhaSanXuat.Find(id);
            if (nhaSanXuat == null)
            {
                return HttpNotFound();
            }
            return View(nhaSanXuat);
        }

        // POST: Admin/NhaCungCap/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NhaSanXuat nhaSanXuat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaSanXuat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NhaSanXuat");
            }
            return View(nhaSanXuat);
        }
    }
}