using ClosedXML.Excel;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;



namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    public class PhieuThuController : Controller
    {
        private readonly Model1 db;
        public PhieuThuController(Model1 db)
        {
            this.db = db;
        }

        public ActionResult QLPhieuThu()
        {
            var phieuThu = db.PhieuThu.ToList();
            return View(phieuThu);
        }
        //-----------------------------------------------------------Tạo phiếu thu----------------------------------------------------------

        [HttpPost]
        public ActionResult ImportPhieuThu(HttpPostedFileBase excelFile)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                // Kiểm tra file tồn tại và đúng định dạng
                if (excelFile == null || excelFile.ContentLength <= 0)
                    return Json(new { success = false, message = "Vui lòng chọn một file để tải lên." });

                var fileExtension = Path.GetExtension(excelFile.FileName);
                if (fileExtension != ".xlsx" && fileExtension != ".xls")
                    return Json(new { success = false, message = "Chỉ chấp nhận file Excel (.xlsx, .xls)" });

                // Đọc file Excel
                using (var package = new ExcelPackage(excelFile.InputStream))
                {
                    // Sheet1: PhieuThu
                    var sheet1 = package.Workbook.Worksheets["PhieuThu"];
                    if (sheet1 == null)
                        return Json(new { success = false, message = "Không tìm thấy sheet PhieuThu." });

                    string maPT = sheet1.Cells[2, 1].Value?.ToString();
                    string thoiGianString = sheet1.Cells[2, 2].Value?.ToString();
                    DateTime thoiGian;

                    // Xử lý chuỗi ngày giờ và chuyển đổi
                    thoiGianString = thoiGianString.Replace(" SA", "").Replace(" AM", "").Replace(" PM", "");
                    if (DateTime.TryParseExact(thoiGianString, "dd/MM/yyyy HH:mm:ss",
                                               System.Globalization.CultureInfo.InvariantCulture,
                                               System.Globalization.DateTimeStyles.None, out thoiGian))
                    {
                        thoiGian = thoiGian.Date; 
                    }
                    else
                    {
                        throw new Exception("Dữ liệu ngày giờ không đúng định dạng dd/MM/yyyy HH:mm:ss.");
                    }

                    string maNV = sheet1.Cells[2, 3].Value?.ToString();
                    string maNCC = sheet1.Cells[2, 4].Value?.ToString();
                    int tongTien = int.Parse(sheet1.Cells[2, 5].Value?.ToString());

           
                    var existingPhieuThu = db.PhieuThu.FirstOrDefault(pt => pt.MaPT == maPT);
                    if (existingPhieuThu != null)
                    {
                        return Json(new { success = false, message = "Mã phiếu thu đã tồn tại trong cơ sở dữ liệu." });
                    }

                    // Thêm vào bảng PhieuThu
                    var phieuThu = new PhieuThu
                    {
                        MaPT = maPT,
                        ThoiGian = thoiGian,
                        MaNV = maNV,
                        MaNCC = maNCC,
                        TongTien = tongTien
                    };
                    db.PhieuThu.Add(phieuThu);

                    // Sheet2: CTPhieuThu
                    var sheet2 = package.Workbook.Worksheets["CTPhieuThu"];
                    if (sheet2 == null)
                        return Json(new { success = false, message = "Không tìm thấy sheet CTPhieuThu." });

            
                    for (int row = sheet2.Dimension.Start.Row + 1; row <= sheet2.Dimension.End.Row; row++) 
                    {
                        string maSP = sheet2.Cells[row, 1].Value?.ToString();
                        int soLuong = int.Parse(sheet2.Cells[row, 2].Value?.ToString());
                        int donGia = int.Parse(sheet2.Cells[row, 3].Value?.ToString());


                        var existingSP = db.SanPham.FirstOrDefault(sp => sp.MaSP == maSP);
                        if (existingSP == null)
                        {
                            // Tạo sản phẩm tạm thời với các thuộc tính null hoặc mặc định
                            var newSanPham = new SanPham
                            {
                                MaSP = maSP,
                                MaNSX = "NSX001",
                                MaDanhMuc = "DM001",
                                MaCTDM = "CTDM001",
                                TenSP = "Tên sản phẩm tạm thời", 
                                GiaBan = 1,  
                                GiaKM = 0,   
                                GiaNhap = donGia, 
                                HinhAnh = null 
                            };

                            db.SanPham.Add(newSanPham);

                            var newChiTietSP = new ChiTietSP
                            {
                                MaSP = maSP,
                                ThanhPhan = null,  
                                CongDung = null,   
                                CachDung = null,   
                                DVT = null,        
                                TrangThai = "Còn hàng", 
                                SoLuongTon = soLuong,  
                                NSX = DateTime.Now, 
                                HSD = DateTime.Now.AddYears(1) 
                            };

                            db.ChiTietSP.Add(newChiTietSP);
                        }

                        var ctPhieuThu = new CTPhieuThu
                        {
                            MaPT = maPT,
                            MaSP = maSP,
                            SoLuong = soLuong,
                            DonGia = donGia
                        };
                     
                        db.CTPhieuThu.Add(ctPhieuThu);
                    }

          
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Dữ liệu đã được nhập thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        //-----------------------------------------------------------Tạo chi tiết phiếu thu----------------------------------------------------------

        public ActionResult ShowDetails(string maPT)
        {
            try
            {               
                var ctPhieuThuList = db.CTPhieuThu.Where(ct => ct.MaPT == maPT).ToList();         
                return View(ctPhieuThuList);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "PhieuThu", "ShowDetails"));
            }
        }


    }
}