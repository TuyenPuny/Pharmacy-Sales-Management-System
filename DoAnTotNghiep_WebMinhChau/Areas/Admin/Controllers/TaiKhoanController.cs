using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    public class TaiKhoanController : Controller
    {
        private Model1 db = new Model1();
        // GET: Admin/TaiKhoan
        public ActionResult taikhoan(string searchTerm)
        {
            var model = db.TaiKhoan.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(dm => dm.MaNV.Contains(searchTerm));
            }
            return View(model.ToList());
        }

        //----------------------------------------------------------Chỉnh sửa thông tin tài khoản----------------------------------------------------------

        public ActionResult Edit(int IDTK)
        {
            var nhanVien = db.TaiKhoan.Find(IDTK);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            return View(nhanVien);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaiKhoan nhanVien)
        {
            if (ModelState.IsValid)
            {
                try
                {            
                    var existingNhanVien = db.TaiKhoan.Find(nhanVien.IDTK);
                    if (existingNhanVien != null)
                    {        
                        if (!string.IsNullOrEmpty(nhanVien.MatKhau) && nhanVien.MatKhau != existingNhanVien.MatKhau)
                        {                    
                            string hashedPassword = mahoaMD5(nhanVien.MatKhau);               
                            existingNhanVien.MatKhau = hashedPassword;

                            if (!string.IsNullOrEmpty(existingNhanVien.MaNV))
                            {
                                doiMatKhauNguoiDung(existingNhanVien.MaNV.Trim(), hashedPassword);
                            }
                        }           
                        existingNhanVien.VaiTro = nhanVien.VaiTro;                
                        db.SaveChanges();            
                        return RedirectToAction("TaiKhoan");
                    }
                    else
                    {
                 
                        return HttpNotFound();
                    }
                }
                catch (SqlException sqlEx)
                {
  
                    ModelState.AddModelError("", $"Lỗi cơ sở dữ liệu: {sqlEx.Message}");
                }
                catch (Exception ex)
                {
        
                    ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
                }
            }

            return View(nhanVien);
        }


        private void doiMatKhauNguoiDung(string maNV, string newPassword)
        {
            try
            {
                using (var connection = new SqlConnection("data source=LAPTOP-MK66GERQ;initial catalog=DB_QLNhaThuocMC;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"))
                {
                    connection.Open();


                    string query = $@"
                        ALTER LOGIN [{maNV}] WITH PASSWORD = '{newPassword}'";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
      
                throw new Exception($"Không thể đổi mật khẩu cho user SQL Server: {sqlEx.Message}");
            }
        }



    
        private string mahoaMD5(string password)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                return BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }

        //-----------------------------------------------------------Thêm tài khoản----------------------------------------------------------
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaiKhoan nhanVien)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem tài khoản với ID hoặc thông tin duy nhất đã tồn tại chưa
                    var existingNhanVien = db.TaiKhoan.FirstOrDefault(x => x.IDTK == nhanVien.IDTK);
                    if (existingNhanVien != null)
                    {
                        ModelState.AddModelError("", "Tài khoản đã tồn tại.");
                        return View(nhanVien);
                    }

     
                    if (!string.IsNullOrEmpty(nhanVien.MatKhau))
                    {
                        nhanVien.MatKhau = mahoaMD5(nhanVien.MatKhau);
                    }

           
                    db.TaiKhoan.Add(nhanVien);

               
                    var nhanVienEntity = db.NhanVien.FirstOrDefault(x => x.MaNV == nhanVien.MaNV);
                    if (nhanVienEntity != null)
                    {
                 
                        nhanVienEntity.TrangThai = "Đang làm việc";
                    }

             
                    db.SaveChanges();

   
                    var loginName = nhanVien.MaNV;
                    var password = nhanVien.MatKhau; 
                    var createLoginQuery = $@"
                            CREATE LOGIN [{loginName}] WITH PASSWORD = '{password}';
                            CREATE USER [{loginName}] FOR LOGIN [{loginName}];
                            EXEC sp_addrolemember 'NhanVien', '{loginName}';
                        ";


                    db.Database.ExecuteSqlCommand(createLoginQuery);

                    return RedirectToAction("TaiKhoan");
                }
                catch (SqlException sqlEx)
                {
                    ModelState.AddModelError("", $"Lỗi cơ sở dữ liệu: {sqlEx.Message}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
                }
            }

  
            return View(nhanVien);
        }
        //-----------------------------------------------------------Xóa tài khoản----------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int IDTK)
        {
            var nhanVien = db.TaiKhoan.Find(IDTK);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            db.TaiKhoan.Remove(nhanVien);
            db.SaveChanges();

            return RedirectToAction("TaiKhoan");
        }
    }
}