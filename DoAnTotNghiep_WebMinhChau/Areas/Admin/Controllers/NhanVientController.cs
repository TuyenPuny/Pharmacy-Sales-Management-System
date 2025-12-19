using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    public class NhanVientController : Controller
    {
        private Model1 db = new Model1();
        // GET: Admin/NhanVient
        public ActionResult NhanVien(string searchTerm)
        {
            ViewBag.SLNV = (int)db.Database.SqlQuery<int>("SELECT dbo.NV()").FirstOrDefault();
            var model = db.NhanVien.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(dm => dm.HoTen.Contains(searchTerm));
            }
            return View(model.ToList());
        }

        // =============================================================================================== CHỈNH SỬA THÔNG TIN 
        public ActionResult Edit(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV))
            {
                return HttpNotFound();
            }

            var nhanVien = db.NhanVien.Find(maNV);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            return View(nhanVien);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NhanVien nhanVien)
        {
            int currentYear = DateTime.Now.Year;
            if (nhanVien.NamSinh.HasValue && nhanVien.NamSinh > currentYear)
            {
                ModelState.AddModelError("NamSinh", "Năm sinh không được lớn hơn năm hiện tại.");
            }

            var existingPhone = db.NhanVien.FirstOrDefault(nv => nv.SDT == nhanVien.SDT && nv.MaNV != nhanVien.MaNV);
            if (existingPhone != null)
            {
                ModelState.AddModelError("SDT", "Số điện thoại này đã được sử dụng bởi nhân viên khác.");
            }
            if (!ModelState.IsValid)
            {
                return View(nhanVien);
            }

            var existingNhanVien = db.NhanVien.Find(nhanVien.MaNV);
            if (existingNhanVien == null)
            {
                ModelState.AddModelError("", "Nhân viên không tồn tại.");
                return View(nhanVien);
            }

            // Cập nhật thông tin nhân viên
            existingNhanVien.HoTen = nhanVien.HoTen;
            existingNhanVien.NamSinh = nhanVien.NamSinh;
            existingNhanVien.DiaChi = nhanVien.DiaChi;
            existingNhanVien.GioiTinh = nhanVien.GioiTinh;
            existingNhanVien.SDT = nhanVien.SDT;
            existingNhanVien.TrangThai = nhanVien.TrangThai;
            existingNhanVien.NgayVaoLam = nhanVien.NgayVaoLam;

            try
            {
                db.SaveChanges();
                return RedirectToAction("NhanVien");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu thay đổi. Vui lòng thử lại.");
                ModelState.AddModelError("", ex.Message);
                return View(nhanVien);
            }
        }

        // =============================================================================================== THÊM NHÂN VIÊN
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NhanVien nhanVien)
        {
            int currentYear = DateTime.Now.Year;
            if (nhanVien.NamSinh >= currentYear)
            {
                ModelState.AddModelError("NamSinh", "Năm sinh không được lớn hơn năm hiện tại.");
            }
            var existingPhone = db.NhanVien.FirstOrDefault(nv => nv.SDT == nhanVien.SDT && nv.MaNV != nhanVien.MaNV);
            if (existingPhone != null)
            {
                ModelState.AddModelError("SDT", "Số điện thoại này đã được sử dụng bởi nhân viên khác.");
            }
            if (!ModelState.IsValid)
            {
                return View(nhanVien);
            }
            var existingNhanVien = db.NhanVien.Find(nhanVien.MaNV);
            if (existingNhanVien != null)
            {
                ModelState.AddModelError("MaNV", "Mã nhân viên này đã tồn tại.");
            }
            if (!ModelState.IsValid)
            {
                return View(nhanVien);
            }

            db.NhanVien.Add(nhanVien);
            db.SaveChanges();
            return RedirectToAction("NhanVien");
        }


    }
}