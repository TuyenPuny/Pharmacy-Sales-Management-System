using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;
using System.Data.Entity;
using System.IO;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        private Model1 db = new Model1();
        public ActionResult SanPham(string searchTerm)
        {
            ViewBag.slSP = (int)db.Database.SqlQuery<int>("SELECT dbo.sp()").FirstOrDefault();
            ViewBag.spKM = (int)db.Database.SqlQuery<int>("SELECT dbo.spKM()").FirstOrDefault();
            ViewBag.spSHH = (int)db.Database.SqlQuery<int>("SELECT dbo.spHH(30)").FirstOrDefault();
            ViewBag.spGH = (int)db.Database.SqlQuery<int>("SELECT dbo.spGH(30)").FirstOrDefault();

            var model = db.SanPham.AsQueryable();

            //  thêm 30 ngày
            var thresholdDate = DateTime.Now.AddDays(30);

            // Kiểm tra và xử lý các từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                if (searchTerm == "Sản phẩm đang khuyến mãi")
                {
                    model = model.Where(sp => sp.GiaKM != null);
                }
                else if (searchTerm == "Sản phẩm sắp hết hạn")
                {
                    model = model.Where(sp => db.ChiTietSP.Any(ct => ct.MaSP == sp.MaSP && ct.HSD < thresholdDate));
                }
                else if (searchTerm == "Sản phẩm gần hết")
                {
                    model = model.Where(sp => db.ChiTietSP.Any(ct => ct.MaSP == sp.MaSP && ct.SoLuongTon < 30));
                }
                else
                {
                    model = model.Where(sp => sp.TenSP.Contains(searchTerm));
                }
            }

            return View(model.ToList());
        }


        //----------------------------------------------------------------------------------------------------------------------------THÊM SẢN PHẨM
        public ActionResult Create()
        {
            var Sanpham = new SanPham();
            var ChiTietSP = new ChiTietSP();
            ViewBag.SanPham = Sanpham;
            ViewBag.ChiTietSanPham = ChiTietSP;
            return View();
        }

        // POST: Admin/SanPham/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sanPham, ChiTietSP chiTietSP, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.SanPham.Add(sanPham);
                    db.SaveChanges();
                    if (HinhAnh != null && HinhAnh.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(HinhAnh.FileName);
                        var path = Path.Combine(Server.MapPath("~/Assets/Image/"), fileName);
                        HinhAnh.SaveAs(path);
                        sanPham.HinhAnh = fileName;
                    }
                    chiTietSP.MaSP = sanPham.MaSP;
                    if (chiTietSP.SoLuongTon == 0)
                    {
                        chiTietSP.TrangThai = "Hết hàng"; 
                    }
                    else if (chiTietSP.SoLuongTon < 20)
                    {
                        chiTietSP.TrangThai = "Sắp hết hàng"; 
                    }
                    else
                    {
                        chiTietSP.TrangThai = "Còn hàng";
                    }
                    db.ChiTietSP.Add(chiTietSP);
                    db.SaveChanges();

                    return RedirectToAction("SanPham");
                }
                catch (DbUpdateException ex)
                {
                    var errorMessage = ex.InnerException?.InnerException?.Message ?? ex.Message;
                    ModelState.AddModelError("", "Có lỗi xảy ra khi lưu dữ liệu: " + errorMessage);
                }
            }

            ViewBag.SanPham = sanPham;
            ViewBag.ChiTietSanPham = chiTietSP;

            return View();
        }
        //------------------------------------------------------------------------------------------------------------sỬA SẢN PHẨM 
        public ActionResult Edit(string MaSP)
        {
            var sanPham = db.SanPham.Find(MaSP);

            if (sanPham == null)
            {
                return HttpNotFound();
            }

            // Lấy thông tin chi tiết sản phẩm
            var chiTietSP = db.ChiTietSP.FirstOrDefault(ct => ct.MaSP == sanPham.MaSP);
            ViewBag.SanPham = sanPham;
            ViewBag.ChiTietSanPham = chiTietSP;

            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SanPham sanPham, ChiTietSP chiTietSP, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var editSP = db.SanPham.Find(sanPham.MaSP);
                    if (editSP == null)
                    {
                        return HttpNotFound();
                    }

                    // Cập nhật thông tin bảng SanPham
                    editSP.MaNSX = sanPham.MaNSX;
                    editSP.MaDanhMuc = sanPham.MaDanhMuc;
                    editSP.MaCTDM = sanPham.MaCTDM;
                    editSP.TenSP = sanPham.TenSP;
                    editSP.GiaBan = sanPham.GiaBan;
                    editSP.GiaKM = sanPham.GiaKM;
                    editSP.GiaNhap = sanPham.GiaNhap;

                    // Xử lý hình ảnh
                    if (HinhAnh != null && HinhAnh.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(HinhAnh.FileName);
                        var path = Path.Combine(Server.MapPath("~/Assets/Image/"), fileName);
                        HinhAnh.SaveAs(path);
                        editSP.HinhAnh = fileName;
                    }

                    // Cập nhật thông tin bảng ChiTietSP
                    var chiTiet = db.ChiTietSP.FirstOrDefault(ct => ct.MaSP == sanPham.MaSP);
                    if (chiTiet != null)
                    {
                        chiTiet.ThanhPhan = chiTietSP.ThanhPhan;
                        chiTiet.CongDung = chiTietSP.CongDung;
                        chiTiet.CachDung = chiTietSP.CachDung;
                        chiTiet.DVT = chiTietSP.DVT;
                        chiTiet.TrangThai = chiTietSP.TrangThai;
                        chiTiet.SoLuongTon = chiTietSP.SoLuongTon;
                        chiTiet.NSX = chiTietSP.NSX;
                        chiTiet.HSD = chiTietSP.HSD;
                    }

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    db.Entry(editSP).State = EntityState.Modified;
                    db.Entry(chiTiet).State = EntityState.Modified;  // Cập nhật ChiTietSP
                    db.SaveChanges();

                    // Gọi thủ tục để cập nhật trạng thái
                    db.Database.ExecuteSqlCommand("EXEC UpdateTrangThaiChiTietSP");

                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction("Edit", new { MaSP = editSP.MaSP });
                }
                catch (DbUpdateException ex)
                {
                    var errorMessage = ex.InnerException?.InnerException?.Message ?? ex.Message;
                    ModelState.AddModelError("", "Có lỗi xảy ra khi lưu dữ liệu: " + errorMessage);
                }
            }

            return View(sanPham);
        }



        //------------------------------------------------------------------------------------------------------XÓA SẢN PHÂM
        [HttpDelete]
        public JsonResult Delete(string MaSP)
        {
            try
            {
                // Tìm sản phẩm cần xóa
                var sanPham = db.SanPham.Find(MaSP);
                if (sanPham == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
                }

                // Kiểm tra sản phẩm có tồn tại trong HoaDon hay không
                var existsInHoaDon = db.CTHoaDon.Any(hd => hd.MaSP == MaSP);

                if (existsInHoaDon)
                {
                    // Nếu sản phẩm có trong HoaDon, chuyển trạng thái trong ChiTietSP thành "Ngừng kinh doanh"
                    var chiTietSP = db.ChiTietSP.FirstOrDefault(ct => ct.MaSP == MaSP);
                    if (chiTietSP != null)
                    {
                        chiTietSP.TrangThai = "Ngừng kinh doanh";
                        db.Entry(chiTietSP).State = EntityState.Modified; // Đánh dấu cập nhật
                    }
                }
                else
                {
                    // Nếu sản phẩm không có trong HoaDon, xóa ChiTietSP liên quan trước
                    var chiTietSP = db.ChiTietSP.FirstOrDefault(ct => ct.MaSP == MaSP);
                    if (chiTietSP != null)
                    {
                        db.ChiTietSP.Remove(chiTietSP);
                    }

                    // Xóa sản phẩm
                    db.SanPham.Remove(sanPham);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                // Thông báo thành công
                string message = existsInHoaDon ? "Sản phẩm đã được cập nhật trạng thái là 'Ngừng kinh doanh'" : "Sản phẩm đã được xóa thành công!";
                return Json(new { success = true, message });
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và trả về thông báo lỗi
                return Json(new { success = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }
    }
}