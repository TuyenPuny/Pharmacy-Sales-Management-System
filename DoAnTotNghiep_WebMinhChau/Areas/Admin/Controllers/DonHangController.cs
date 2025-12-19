using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        // GET: Admin/DonHang
        private Model1 db = new Model1();

        public ActionResult DonHang(string searchTerm, string dc)
        {
            var model = db.HoaDon.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(dm => dm.HoTen.Contains(searchTerm));
            }
            return View(model.ToList());
        }

        [HttpPost]
        public ActionResult UpdateStatus(string MaHD, string TrangThai)
        {
            // Tìm hóa đơn theo MaHD
            var hoaDon = db.HoaDon.Find(MaHD); // Giả sử bạn đang sử dụng Entity Framework
            if (hoaDon != null)
            {
                // Cập nhật trạng thái
                hoaDon.TrangThai = TrangThai;
                db.SaveChanges(); // Lưu thay đổi
            }
            return RedirectToAction("DonHang", "DonHang"); // Quay lại danh sách đơn hàng
        }

        public ActionResult ChiTiet(string maHD)
        {
            // Lấy thông tin hóa đơn từ bảng HoaDon
            var hoaDon = db.HoaDon.FirstOrDefault(hd => hd.MaHD == maHD);

            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách chi tiết hóa đơn và kết hợp để lấy tên sản phẩm
            var chiTietHoaDon = db.CTHoaDon
                .Where(ct => ct.MaHD == maHD)
                .Join(
                    db.SanPham,         
                    ct => ct.MaSP,            
                    sp => sp.MaSP,            
                    (ct, sp) => new ChiTietSanPhamViewModel
                    {
                        TenSP = sp.TenSP,      
                        SoLuong = ct.SoLuong ?? 0, 
                        DonGia = ct.DonGia ?? 0,
                        ThanhTien = (ct.SoLuong ?? 0) * (ct.DonGia ?? 0)
                    }
                )
                .ToList();

            // Tạo ViewModel để truyền sang View
            var viewModel = new ChiTietHoaDonViewModel
            {
                HoaDon = hoaDon,
                ChiTietHoaDon = chiTietHoaDon
            };

            return View(viewModel);
        }

    }
}