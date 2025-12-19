using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    public class NhaCungCapController : Controller
    {
        private Model1 db = new Model1();
        // GET: Admin/NhaCungCap
        public ActionResult NhaCungCap(string searchTerm)
        {
            
            ViewBag.SLNCC = (int)db.Database.SqlQuery<int>("SELECT dbo.NCC()").FirstOrDefault();
            var model = db.NhaCungCap.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(dm => dm.TenNCC.Contains(searchTerm));
            }
            return View(model.ToList());
        }

        //-----------------------------------------------------------Thêm nhà cung cấp----------------------------------------------------------

        // GET: Admin/NhaCungCap/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NhaCungCap/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NhaCungCap nhaCungCap)
        {
            if (ModelState.IsValid)
            {
                db.NhaCungCap.Add(nhaCungCap);
                db.SaveChanges();
                return RedirectToAction("NhaCungCap");
            }
            return View(nhaCungCap);
        }


        //-----------------------------------------------------------Tìm kiếm nhà cung cấp----------------------------------------------------------

        public ActionResult Search(string searchTerm)
        {
            var nhaCungCaps = db.NhaCungCap.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                nhaCungCaps = nhaCungCaps.Where(ncc => ncc.TenNCC.Contains(searchTerm));
            }

            return View("Index", nhaCungCaps.ToList());
        }


        //-----------------------------------------------------------Chỉnh sửa nhà cung cấp----------------------------------------------------------

        // GET: Admin/NhaCungCap/Edit/
        public ActionResult Edit(string id)
        {
            var nhaCungCap = db.NhaCungCap.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            return View(nhaCungCap);
        }

        // POST: Admin/NhaCungCap/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NhaCungCap nhaCungCap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaCungCap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NhaCungCap");
            }
            return View(nhaCungCap);
        }

       
    }
}