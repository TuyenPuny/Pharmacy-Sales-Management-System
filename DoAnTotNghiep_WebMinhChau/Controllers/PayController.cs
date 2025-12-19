using DoAnTotNghiep_WebMinhChau.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace DoAnTotNghiep_WebMinhChau.Controllers
{
    public class PayController : Controller
    {
        private KhachHang KhachHang = new KhachHang();
        private DBContext db = new DBContext();
        private readonly IVnPayServers _vnPayservice;
        public PayController(IVnPayServers vnPayServers)
        {
            _vnPayservice = vnPayServers;
        }

        // GET: Pay
        [HttpGet]
        public ActionResult ThanhToanOff()
        {
            ViewBag.Tinh = Session["Tinh"];
            ViewBag.Quan = Session["Quan"];
            ViewBag.Phuong = Session["Phuong"];
            ViewBag.DiaChiView = Session["DiaChi"];
            // Lấy tổng tiền từ Session
            decimal totalAmount = Session["TotalAmount"] != null ? (decimal)Session["TotalAmount"] : 0.0m;

         
            ViewBag.TotalAmount = totalAmount;

            if (Session["Name"] == null)
            {
                return View();
            }
            else
            {
  
                string email = Session["Email"] as string;
                if (string.IsNullOrEmpty(email))
                {
                    return HttpNotFound("Email không hợp lệ.");
                }

             
                KhachHang.LoadKH();

                var khachHang = KhachHang.danhsachKH.FirstOrDefault(kh => kh.EmailKH == email);
                if (khachHang == null)
                {
                    return HttpNotFound("Khách hàng không tồn tại.");
                }

                return View(khachHang);
            }

        }

        public ActionResult ThanhToan()
        {
            return View();
        }
        public ActionResult InHoaDon(string maHD)
        {
            
            var hoaDon = db.HoaDon.FirstOrDefault(hd => hd.MaHD == maHD);
            if (hoaDon == null)
            {
                return HttpNotFound("Không tìm thấy hóa đơn!");
            }
      
            var chiTietHoaDon = (from cthd in db.CTHoaDon
                                 join sp in db.SanPham on cthd.MaSP equals sp.MaSP
                                 where cthd.MaHD == maHD
                                 select new CTHoaDonDTO
                                 {
                                     MaSP = cthd.MaSP,
                                     TenSP = sp.TenSP,
                                     SoLuong = cthd.SoLuong,
                                     DonGia = cthd.DonGia,
                                     ThanhTien = cthd.SoLuong * cthd.DonGia
                                 }).ToList();

            
            ViewBag.HoaDon = hoaDon; 
            ViewBag.ChiTietHoaDon = chiTietHoaDon;
            return View();
        }
        [HttpPost]
        public ActionResult ThanhToan(HoaDon hoadon)
        {
            try
            {
                hoadon.TongTien = hoadon.TongTien > 0 ? hoadon.TongTien : (decimal)(Session["TotalAmount"] ?? 0.0m);
                hoadon.HinhThucNhanHanh = Request.Form["deliveryMethod"];
                hoadon.PTTT = Request.Form["paymentMethod"];
                hoadon.DiaChiNhanHang = hoadon.HinhThucNhanHanh == "home" ? hoadon.DiaChiNhanHang : Request.Form["pharmacyAddress"];

               
                string maHD = Guid.NewGuid().ToString(); 
                Session["MaHD"] = maHD; 

                if (Session["Name"] == null)
                {
                    if (string.IsNullOrWhiteSpace(hoadon.HoTen) || string.IsNullOrWhiteSpace(hoadon.SDT))
                    {
                        ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin!";
                        return View(hoadon);
                    }
                    return xulyHoaDonKhiChuaDangNhap(hoadon);
                }
                else
                {
                    return xulyHoaDonKhiDaDangNhap(hoadon);
                }
            }
            catch (Exception ex)
            {               

         
                ViewBag.ErrorMessage = "Có lỗi xảy ra trong quá trình thanh toán. Vui lòng thử lại sau.";
                return View(hoadon); 
            }
        }
        private ActionResult xulyHoaDonKhiChuaDangNhap(HoaDon hoadon)
        {
            string maHD = Session["MaHD"].ToString();
            hoadon.MaHD = maHD;
            try
            {
                string sql = BuildHoaDonSql(hoadon);
                db.Database.ExecuteSqlCommand(sql, GetHoaDonParameters(hoadon));

                XyLyGioHang(hoadon.MaHD);

                Session.Remove(CartHelper.CartSession);

                if (hoadon.PTTT == "vnpay")
                {                   
                    TempData["HoaDon"] = hoadon;

                    return RedirectToAction("CheckOut", "Pay");
                }

                ViewBag.Message = "Thanh toán thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra khi thanh toán: " + ex.Message;
            }
            InHoaDon(maHD);
            return View(hoadon);
        }

        private ActionResult xulyHoaDonKhiDaDangNhap(HoaDon hoadon)
        {
            string maHD = Session["MaHD"].ToString();
            hoadon.MaHD = maHD;
            try
            {
                string sql = BuildHoaDonSql(hoadon, true);
                db.Database.ExecuteSqlCommand(sql, GetHoaDonParameters(hoadon));

                XyLyGioHang(hoadon.MaHD);

                if (hoadon.PTTT == "vnpay")
                {               
                    TempData["HoaDon"] = hoadon;

                    return RedirectToAction("CheckOut", "Pay", new { payment = "vnpay" });
                }

                ViewBag.Message = "Thanh toán thành công!";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra khi thanh toán: " + ex.Message;
            }
            InHoaDon(maHD);
            return View(hoadon);
        }

        private string BuildHoaDonSql(HoaDon hoadon, bool isExistingUser = false)
        {
            if (isExistingUser)
            {
                return @"EXEC AddHoaDon @MaHD, @SDT, @HinhThucNhanHanh, @EmailKH, @TinhThanh, @QuanHuyen, @PhuongXa, @DiaChiNhanHang, @PTTT, @TongTien";
            }
            else
            {
                return @"INSERT INTO HoaDon (MaHD, HoTen, SDT, HinhThucNhanHanh, EmailKH, TinhThanh, QuanHuyen, PhuongXa, DiaChiNhanHang, PTTT, TongTien)
                VALUES (@MaHD, @Hoten, @SDT, @HinhThucNhanHanh, null, @TinhThanh, @QuanHuyen, @PhuongXa, @DiaChiNhanHang, @PTTT, @TongTien)";
            }
        }

        private SqlParameter[] GetHoaDonParameters(HoaDon hoadon)
        {
            return new SqlParameter[]
            {
        new SqlParameter("@MaHD", hoadon.MaHD),
        new SqlParameter("@Hoten", hoadon.HoTen),
        new SqlParameter("@SDT", hoadon.SDT),
        new SqlParameter("@HinhThucNhanHanh", hoadon.HinhThucNhanHanh == "home" ? "Nhận Hàng Tại Nhà" : "Nhận Hàng Tại Quầy Thuốc"),
        new SqlParameter("@EmailKH", Session["Email"] ?? (object)DBNull.Value),
        new SqlParameter("@TinhThanh", hoadon.TinhThanh ?? (object)DBNull.Value),
        new SqlParameter("@QuanHuyen", hoadon.QuanHuyen ?? (object)DBNull.Value),
        new SqlParameter("@PhuongXa", hoadon.PhuongXa ?? (object)DBNull.Value),
        new SqlParameter("@DiaChiNhanHang", hoadon.DiaChiNhanHang ?? (object)DBNull.Value),
        new SqlParameter("@PTTT", hoadon.PTTT == "vnpay" ? "VNPay" : (hoadon.PTTT == "vietcombank" ? "Vietcombank" : "Thanh toán khi nhận hàng")),
        new SqlParameter("@TongTien", hoadon.TongTien)
            };
        }

        private void XyLyGioHang(string maHD)
        {
            var cart = Session[CartHelper.CartSession] as List<CartItem> ?? new List<CartItem>();
            foreach (var item in cart)
            {
                string ctSql = @"INSERT INTO CTHoaDon (MaHD, MaSP, SoLuong, DonGia) 
                         VALUES (@MaHD, @MaSP, @SoLuong, @DonGia)";
                db.Database.ExecuteSqlCommand(ctSql,
                    new SqlParameter("@MaHD", maHD),
                    new SqlParameter("@MaSP", item.product.MaSP),
                    new SqlParameter("@SoLuong", item.Quantity),
                    new SqlParameter("@DonGia", item.product.GiaKM ?? item.product.GiaBan));

                string updateSql = @"UPDATE ChiTietSP 
                             SET SoLuongTon = SoLuongTon - @SoLuong 
                             WHERE MaSP = @MaSP";
                db.Database.ExecuteSqlCommand(updateSql,
                    new SqlParameter("@SoLuong", item.Quantity),
                    new SqlParameter("@MaSP", item.product.MaSP));
            }
        }


        public ActionResult ConfirmOrder(decimal totalAmount)
        {
            // Save total amount to TempData
            TempData["TotalAmount"] = totalAmount;

            // Redirect to PayController's CheckOut action
            return RedirectToAction("CheckOut", "Pay");
        }

        public ActionResult CheckOut(CheckoutVM model, HoaDon hoaDon, CartItem cartItem, string payment = "vnpay")
        {
            if (payment == "vnpay")
            {
                decimal totalAmount = (decimal)Session["TotalAmount"];
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = totalAmount,
                    CreatedDate = DateTime.Now,
                    Description = $"{model.HoTen}{model.SoDienThoai}",
                    FullName = model.HoTen,
                    OrderId = hoaDon.MaHD
                };
                string text = _vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel);
                // Chuyển hướng đến URL thanh toán VNPay
                return Redirect(text);
            }
            // Trường hợp thanh toán không hợp lệ hoặc có lỗi, trở lại trang checkout
            return View("ThanhToan", hoaDon);
        }

        [AllowAnonymous]
        public ActionResult PaymentSuccess()
        {
            return View("Success");
        }
        [AllowAnonymous]
        public ActionResult PaymentFail()
        {

            return View("PaymentFail");
        }
        [AllowAnonymous]

        public ActionResult Cancelled()
        {
            return View("Cancelled");
        }
        [AllowAnonymous]
        public ActionResult PaymentCallBack()
        {
            //9704198526191432198
            //NGUYEN VAN A
            //07 / 15
            //123456
            //NCB
            string maHD = Session["MaHD"].ToString();
            var hashSecret = System.Configuration.ConfigurationManager.AppSettings["VnPay:HashSecret"];
            var response = _vnPayservice.PaymentExecute(Request.QueryString, hashSecret);

            if (response == null)
            {
                TempData["Message"] = "Lỗi thanh toán VN Pay: Phản hồi không hợp lệ hoặc chữ ký không hợp lệ.";
                return RedirectToAction("PaymentFail");
            }
            else if (response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Đơn hàng đã được hủy!!!";
                XoaHoaDon(maHD);
                return RedirectToAction("PaymentFail");
            }
            else
            {
                TempData["Message"] = "Thanh toán VNpay thành công";

               
                HoaDon hoadon = TempData["HoaDon"] as HoaDon;
                if (hoadon != null)
                {
                   
                    InHoaDon(hoadon.MaHD);

            
                    return View("ThanhToan", new { maHD = hoadon.MaHD });
                }
                return RedirectToAction("ThanhToan");
            }
        }

        private void XoaHoaDon(string maHD)
        {
            try
            {
                
                string connectionString = ConfigurationManager.ConnectionStrings["NhaThuocMC"].ConnectionString;

                
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();  

                    
                    using (var command = new SqlCommand("EXEC XoaHoaDon @MaHD", connection))
                    {
                    
                        command.Parameters.AddWithValue("@MaHD", maHD);

                     
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}