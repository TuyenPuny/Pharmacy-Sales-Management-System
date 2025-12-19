using DoAnTotNghiep_WebMinhChau.Models;
using DoAnTotNghiep_WebMinhChau.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DoAnTotNghiep_WebMinhChau.Controllers
{
    public class UsersController : Controller
    {
        DBContext db = new DBContext();
        AccountService forgotpass = new AccountService();


        //-----------------------------------------------------------Đăng nhập----------------------------------------------------------
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string u, string p)
        {
            u = Request.Form["Username"];
            p = Request.Form["pass"];

            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
            {
                ViewBag.Message = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }
            string connStr = ketnoiSQL(u, p);

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    if (conn.State != System.Data.ConnectionState.Open) return View();

                    // Kiểm tra thông tin người dùng và lưu vào Session
                    if (!ktraTTNguoiDung(conn, u))
                    {
                        ViewBag.Message = "Đăng nhập thất bại: Không tìm thấy người dùng";
                        return View();
                    }

                    // Xử lý giỏ hàng
                    xulyGioHangKhiDangNhap(conn);

                    Session["Role"] = u.StartsWith("NV") ? "Nhân viên" : (u.StartsWith("AD") ? "ADMIN" : "Khách hàng");

                    if (Session["Role"] == "Nhân viên")
                    {
                        return RedirectToAction("DonHang", "DonHang", new { area = "Admin" });
                    }
                    else
                    {
                        // ADMIN hoặc Khách hàng
                        return RedirectToAction(
                            "Index",
                            Session["Role"] == "ADMIN" ? "BaoCao_ThongKe" : "Home",
                            Session["Role"] == "ADMIN" ? new { area = "Admin" } : null
                        );
                    }

                }
            }
            catch (SqlException ex)
            {
                ViewBag.Message = "Thông tin đăng nhập không chính xác";
            }

            return View();
        }
        private bool ktraTTNguoiDung(SqlConnection conn, string username)
        {
            string sql = (username.StartsWith("NV") || username.StartsWith("AD"))
                                                    ? "SELECT HoTen FROM NhanVien WHERE MaNV = @username"
                                                    : "SELECT HoTen, EmailKH, SDT FROM KhachHang WHERE EmailKH = @username";
            var command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@username", username);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Session["Name"] = reader["HoTen"];
                    if (!username.StartsWith("NV") & !username.StartsWith("AD"))
                    {
                        Session["Email"] = reader["EmailKH"].ToString();
                        Session["SDT"] = reader["SDT"].ToString();
                    }
                    return true;
                }
            }
            return false;
        }
        private string ketnoiSQL(string username, string password)
        {
            string hashedPassword = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(password))).Replace("-", "");
            return System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString
                   .Replace("connectNTMC", username).Replace("123", hashedPassword);
        }
        private void xulyGioHangKhiDangNhap(SqlConnection conn)
        {
            var cart = Session[CartHelper.CartSession] as List<CartItem>;
            if (cart != null && Session["Email"] != null)
            {
                foreach (var item in cart)
                {
                    var cmd = new SqlCommand("IF EXISTS (SELECT 1 FROM GioHang WHERE MaSP = @p0 AND EmailKH = @p1) " +
                                             "UPDATE GioHang SET SoLuong = SoLuong + @p2 WHERE MaSP = @p0 AND EmailKH = @p1 " +
                                             "ELSE " +
                                             "INSERT INTO GioHang (MaSP, EmailKH, SoLuong) VALUES (@p0, @p1, @p2)", conn);
                    cmd.Parameters.AddWithValue("@p0", item.product.MaSP);
                    cmd.Parameters.AddWithValue("@p1", Session["Email"]);
                    cmd.Parameters.AddWithValue("@p2", item.Quantity);
                    cmd.ExecuteNonQuery();
                }
                Session.Remove(CartHelper.CartSession); 
            }
        }


        //-----------------------------------------------------------Đăng xuất----------------------------------------------------------

        [HttpPost]
        public ActionResult Logout()
        {
            Session["Name"] = null;
            Session["Email"] = null;
            Session["SDT"] = null;
            return RedirectToAction("Index", "Home");
        }


        //-----------------------------------------------------------Đăng ký----------------------------------------------------------

        public ActionResult Register()
        {
            var u = Request.Form["Username"];
            var e = Request.Form["Email"];
            var sdt = Request.Form["SDT"];
            var p = Request.Form["pass"];
            var np = Request.Form["pass2"];

            if (p != np)
            {
                ViewBag.Message = "Mật khẩu xác nhận không trùng khớp.";
                return View();
            }

            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(e) || string.IsNullOrEmpty(p))
            {
                ViewBag.Message = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            string passwordHash = mahoaMD5(p);
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["NhaThuocMC"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();

                if (kiemtraTonTaiEmail(conn, e))
                {
                    ViewBag.Message = "Email đã tồn tại.";
                    return View();
                }

                try
                {
                    taoDuLieuNguoiDung(conn, e, u, passwordHash, sdt);
                    return RedirectToAction("Login", "Users");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Đăng ký thất bại: " + ex.Message;
                    return View();
                }
            }
        }
        private void taoDuLieuNguoiDung(SqlConnection conn, string email, string username, string passwordHash, string phoneNumber)
        {
            string[] queries = new string[]
            {
        "INSERT INTO KhachHang (EmailKH, HoTen, SDT) VALUES (@EmailKH, @HoTen, @SDT)",
        "INSERT INTO TaiKhoan (MatKhau, VaiTro, EmailKH) VALUES (@MatKhau, N'Khách Hàng', @EmailKH)",
        $"CREATE LOGIN [{email}] WITH PASSWORD = '{passwordHash}'; CREATE USER [{email}] FOR LOGIN [{email}]; EXEC sp_addrolemember 'KhachHang', '{email}';"
            };

            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    foreach (var query in queries)
                    {
                        using (var cmd = new SqlCommand(query, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@EmailKH", email);
                            cmd.Parameters.AddWithValue("@HoTen", username);
                            cmd.Parameters.AddWithValue("@SDT", phoneNumber);
                            cmd.Parameters.AddWithValue("@MatKhau", passwordHash);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw; 
                }
            }
        }

        private string mahoaMD5(string password) =>
    BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(password))).Replace("-", "");

        private bool kiemtraTonTaiEmail(SqlConnection conn, string email)
        {
            string query = "SELECT COUNT(*) FROM KhachHang WHERE EmailKH = @Email";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }


        //-----------------------------------------------------------Thông tin người dùng----------------------------------------------------------

        public ActionResult Info()
        {
            KhachHang kh = new KhachHang();
            kh.LoadKH_CNTT();
     
            if (kh.danhsachKH_CNTT.Count > 0)
            {
                return View(kh.danhsachKH_CNTT.First());
            }

            return View(kh);
        }
        // POST: User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Info(KhachHang model)
        {
            if (ModelState.IsValid)
            {
       
                var userEmail = Session["Email"] as string;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return RedirectToAction("Login", "Account"); 
                }

                var user = db.KhachHang.SingleOrDefault(u => u.EmailKH == userEmail);
                if (user == null)
                {
                    return HttpNotFound(); 
                }

                // Cập nhật các thông tin người dùng từ form
                user.HoTen = model.HoTen;
                user.NamSinh = model.NamSinh;
                user.TinhThanh = model.TinhThanh;
                user.QuanHuyen = model.QuanHuyen;
                user.PhuongXa = model.PhuongXa;
                user.DiaChi = model.DiaChi;
                user.GioiTinh = model.GioiTinh;
                user.SDT = model.SDT;

        
                db.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToAction("Info"); 
            }

            return View(model);
        }

        //-----------------------------------------------------------Đổi mật khẩu----------------------------------------------------------

        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {

            string email = Session["Email"] as string;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                return View();
            }

            var user = db.TaiKhoan.FirstOrDefault(u => u.EmailKH == email);
            var mk = mahoaMD5(currentPassword);
            if (user == null || mk != user.MatKhau || newPassword != confirmPassword)
            {
                ModelState.AddModelError("", user == null ? "Không tìm thấy người dùng." : "Mật khẩu không đúng hoặc mật khẩu xác nhận không khớp.");
                return View();
            }

            user.MatKhau = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(newPassword))).Replace("-", "");
            db.SaveChanges();
            capnhatMatKhauNguoiDung(email, user.MatKhau);

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("Info");
        }
        private void capnhatMatKhauNguoiDung(string sqlUserName, string newPassword)
        {

            string connectionString = "Server=LAPTOP-MK66GERQ;Database=DB_QLNhaThuocMC;Integrated Security=True;";
            string query = $"ALTER LOGIN [{sqlUserName.Trim()}] WITH PASSWORD = '{newPassword}'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                 
                    throw new Exception("Không thể thay đổi mật khẩu user SQL Server: " + ex.Message);
                }
            }
        }


        //-----------------------------------------------------------Quên mật khẩu----------------------------------------------------------

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string userEmail)
        {

            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập email.";
                return View(); 
            }

       
            using (var db = new DBContext()) 
            {
                
                var kiemtraTonTaiEmail = db.TaiKhoan.Any(t => t.EmailKH == userEmail);

          
                if (!kiemtraTonTaiEmail)
                {
                    TempData["ErrorMessage"] = "Email không tồn tại trong hệ thống.";
                    return View(); 
                }
            }

      
            forgotpass.ResetPassword(userEmail);

            return RedirectToAction("ResetPassword");
        }

        //-----------------------------------------------------------Tạo lại mật khẩu----------------------------------------------------------

        public ActionResult ResetPassword()
        {
            return View();
        }
    }
}
