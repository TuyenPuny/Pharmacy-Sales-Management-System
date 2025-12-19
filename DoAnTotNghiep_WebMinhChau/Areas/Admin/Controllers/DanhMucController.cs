using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    public class DanhMucController : Controller
    {
        private Model1 db = new Model1();

        // GET: Admin/DanhMuc
        public ActionResult Index(string searchTerm, string selectedTrangThai)
        {
            // Đếm số lượng danh mục
            ViewBag.DanhMucCount = (int)db.Database.SqlQuery<int>("SELECT dbo.CountDanhMuc()").FirstOrDefault();
            ViewBag.DMNgungHD = (int)db.Database.SqlQuery<int>("SELECT dbo.CountDMNgungHD()").FirstOrDefault();
            ViewBag.DMHD = (int)db.Database.SqlQuery<int>("SELECT dbo.HoatDong()").FirstOrDefault();

            var model = db.DanhMucSP.AsQueryable();
            if (!string.IsNullOrEmpty(selectedTrangThai))
            {
                bool trangThai = selectedTrangThai == "true";
                model = model.Where(dm => dm.TrangThai == trangThai);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(dm => dm.TenDanhMuc.Contains(searchTerm));
            }
            return View(model.ToList());
        }

        //-----------------------------------------------------------Tạo danh mục----------------------------------------------------------

        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DanhMuc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DanhMucSP model)
        {
            var existingCategory = db.DanhMucSP.SingleOrDefault(d => d.MaDanhMuc == model.MaDanhMuc);
            if (existingCategory != null)
            {
                ModelState.AddModelError("MaDanhMuc", "Mã danh mục này đã tồn tại.");
            }
            if (ModelState.IsValid)
            {
                db.DanhMucSP.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //-----------------------------------------------------------Sửa danh mục----------------------------------------------------------

        // GET: DanhMuc/Edit/
        public ActionResult Edit(string maDanhMuc)
        {
            if (string.IsNullOrWhiteSpace(maDanhMuc))
            {
                return HttpNotFound();
            }
            var danhMuc = db.DanhMucSP.Find(maDanhMuc);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // POST: DanhMuc/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DanhMucSP danhMuc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhMuc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhMuc); 
        }


        //---------------------------------------------------------------------------Chi tiết danh mục 
        public ActionResult Details(string maDanhMuc)
        {
            if (string.IsNullOrWhiteSpace(maDanhMuc))
            {
                return HttpNotFound();
            }

            var danhMuc = db.DanhMucSP.Find(maDanhMuc);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }

     
            var danhMucCon = db.CTDanhMuc.Where(dm => dm.MaDanhMuc == maDanhMuc).ToList();

            ViewBag.DanhMuc = danhMuc;
            ViewBag.DanhMucCon = danhMucCon;

            return View();
        }

        //-----------------------------------------------------------Thêm danh mục con----------------------------------------------------------

        [HttpPost]
        public ActionResult CreateSubCategory(string maDanhMuc, string maCTDM, string tenCTDM)
        {

            if (db.CTDanhMuc.Any(dm => dm.MaDanhMuc == maDanhMuc && dm.MaCTDM == maCTDM))
            {
                TempData["ErrorMessage"] = "Mã danh mục con này đã tồn tại.";
                return RedirectToAction("Details", new { maDanhMuc });
            }


            var newSubCategory = new CTDanhMuc
            {
                MaDanhMuc = maDanhMuc,
                MaCTDM = maCTDM,
                TenCTDM = tenCTDM
            };

    
            try
            {
                db.CTDanhMuc.Add(newSubCategory);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Danh mục con đã được thêm thành công!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lưu dữ liệu: {ex.Message}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi lưu dữ liệu.";
            }

            return RedirectToAction("Details", new { maDanhMuc });
        }

        //-----------------------------------------------------------Cập nhật danh mục con----------------------------------------------------------

        [HttpPost]
        public JsonResult Update(string maDanhMuc, string maCTDM, string newName)
        {
       
            var category = db.CTDanhMuc.Find(maDanhMuc, maCTDM);
            if (category != null)
            {
                category.TenCTDM = newName;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }



        [HttpDelete]
        public JsonResult Delete(string maDanhMuc, string maCTDM)
        {
            try
            {        
                var category = db.CTDanhMuc.Find(maDanhMuc, maCTDM);

                if (category != null)
                {
                   
                    var products = db.SanPham
                                     .Where(p => p.MaDanhMuc == maDanhMuc && p.MaCTDM == maCTDM)
                                     .ToList();

                    if (products.Any())
                    {
                      
                        return Json(new { success = false, message = "Còn tồn tại sản phẩm trong danh mục, không thể xóa!" });
                    }

                 
                    db.CTDanhMuc.Remove(category);

             
                    db.SaveChanges();

                   
                    return Json(new { success = true, message = "Xóa thành công!" });
                }

            
                return Json(new { success = false, message = "Danh mục không tồn tại!" });
            }
            catch (Exception ex)
            {
                
                return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa!", error = ex.Message });
            }
        }

    }
}
