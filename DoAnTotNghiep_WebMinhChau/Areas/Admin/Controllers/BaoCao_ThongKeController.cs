using System;
using System.Web.Mvc;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using OfficeOpenXml;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    public class BaoCao_ThongKeController : Controller
    {
        private Model1 db = new Model1();

        public ActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            var role = Session["Role"] as string;
            if (role != "ADMIN" && role != "Nhân viên")
            {
                // Nếu không có quyền, chuyển hướng về trang chủ hoặc trang khác
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            if (!startDate.HasValue)
            {
                startDate = DateTime.Today;
            }
            if (!endDate.HasValue)
            {
                endDate = DateTime.Today.AddDays(1).AddTicks(-1);
            }

            try
            {
                bool isPrint = Request.QueryString["print"] == "true";
                ViewBag.IsPrint = isPrint;
                //Thống kê tổng số đơn hàng, tổng doanh thu, tổng số lượng sản phẩm và lợi nhuận trong khoảng thời gian
                var result = db.Database.SqlQuery<BC_ThongKe>(
                    "EXEC ThongKeTongDoanhSo @StartDate, @EndDate",
                    new SqlParameter("@StartDate", startDate),
                    new SqlParameter("@EndDate", endDate)
                ).FirstOrDefault();


                //Thống kê doanh thu và số lượng sản phẩm theo từng sản phẩm trong khoảng thời gian
                var productStats = db.Database.SqlQuery<ProductStat>(
                    "EXEC ThongKeDoanhThuTheoSanPham @StartDate, @EndDate",
                    new SqlParameter("@StartDate", startDate),
                    new SqlParameter("@EndDate", endDate)
                ).ToList();


                // Thống kê tình trạng tồn kho
                var result1 = db.Database.SqlQuery<BC_ThongKe>(
                    "EXEC ThongKeTonnKhoThuoc"
                ).FirstOrDefault();


                // Danh sách sản phẩm gần hết hạn 
                var products = db.Database.SqlQuery<ProductStat>(
                    "EXEC ThuocGanHetHSD @Months = {0}", 1
                ).ToList();

                var products_hethan = db.Database.SqlQuery<ProductStat>(
                    "EXEC ThuocDaHetHSD @Months", new SqlParameter("@Months", 1)
                ).ToList();

                // Thống kê tình trạng đơn hàng theo trạng thái
                var orderStats = db.Database.SqlQuery<OrderStatusStat>(
                    "EXEC ThongKeDonHangTheoTrangThai @StartDate, @EndDate",
                    new SqlParameter("@StartDate", startDate),
                    new SqlParameter("@EndDate", endDate)
                ).ToList();
                ViewBag.OrderStatusLabels = new[] { "Chờ xác nhận", "Đang xử lý", "Đã hoàn thành", "Đã hủy" };
                ViewBag.OrderStatusData = new[] {
                    orderStats.Sum(o => o.ChoXacNhan),
                    orderStats.Sum(o => o.DangXuLy),
                    orderStats.Sum(o => o.DaHoanThanh),
                    orderStats.Sum(o => o.DaHuy)
                };


                // Danh sách 10 sản phẩm bán nhiều nhất  
                var spbanchay = db.Database.SqlQuery<ProductStat>(
                    "EXEC LietKeTop10SanPhamBanChay"
                ).ToList();


                // Trả về kết quả thống kê trong ViewBag để hiển thị
                ViewBag.ProductStats = productStats;
                ViewBag.OrderStats = orderStats;
                ViewBag.TongSoDonHang = result?.TongSoDonHang ?? 0;
                ViewBag.TongDoanhThu = result?.TongDoanhThu ?? 0;
                ViewBag.TongSoLuongSanPham = result?.TongSoLuongSanPham ?? 0;
                ViewBag.LoiNhuan = result?.LoiNhuan ?? 0;
                ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd") ?? currentDate;
                ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd") ?? currentDate;


                // Đưa dữ liệu vào ViewBag để sử dụng trong View
                ViewBag.PhanTramHetHang = result1?.PhanTramHetHang ?? 0;
                ViewBag.PhanTramSapHet = result1?.PhanTramSapHet ?? 0;
                ViewBag.PhanTramConHang = result1?.PhanTramConHang ?? 0;
                ViewBag.Products = products;
                ViewBag.Products_Hethan = products_hethan;
                ViewBag.ProductsTop10 = spbanchay;


                return View();
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi thống kê: " + ex.Message;
                return View();
            }
        }
        public ActionResult ExportToExcel()
        {
            try
            {
                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var data1 = GetDataFromSql<KhachHang>("SELECT * FROM KhachHang");
                var data2 = GetDataFromSql<NhanVien>("SELECT * FROM NhanVien");
                var data3 = GetDataFromSql<DanhMucSP>("SELECT * FROM DanhMucSP");
                var data4 = GetDataFromSql<CTDanhMuc>("SELECT * FROM CTDanhMuc");
                var data5 = GetDataFromSql<NhaSanXuat>("SELECT * FROM NhaSanXuat");
                var data6 = GetDataFromSql<NhaCungCap>("SELECT * FROM NhaCungCap");

                var data7 = GetDataFromSql<HoaDon>("SELECT * FROM HoaDon");
                var data8 = GetDataFromSql<CTHoaDon>("SELECT * FROM CTHoaDon");

                // Đảm bảo thư mục tồn tại
                string directoryPath = @"E:\DoAnTotNghiep-NhaThuocMinhChau\XuatFileEx";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Tạo file Excel mới
                using (var package = new ExcelPackage())
                {
                    var sheet1 = package.Workbook.Worksheets.Add("KhachHang");
                    sheet1.Cells["A1"].LoadFromCollection(data1, true);

                    var sheet2 = package.Workbook.Worksheets.Add("NhanVien");
                    sheet2.Cells["A1"].LoadFromCollection(data2, true);

                    var sheet3 = package.Workbook.Worksheets.Add("Danh Mục");
                    sheet3.Cells["A1"].LoadFromCollection(data3, true);

                    var sheet4 = package.Workbook.Worksheets.Add("Danh mục con ");
                    sheet4.Cells["A1"].LoadFromCollection(data4, true);
                    var sheet5 = package.Workbook.Worksheets.Add("NSX");
                    sheet5.Cells["A1"].LoadFromCollection(data5, true);

                    var sheet6 = package.Workbook.Worksheets.Add("NCC");
                    sheet6.Cells["A1"].LoadFromCollection(data6, true);

                    var sheet7 = package.Workbook.Worksheets.Add("HoaDon");
                    sheet7.Cells["A1"].LoadFromCollection(data7, true);

                    var sheet8 = package.Workbook.Worksheets.Add("CTHoaDon");
                    sheet8.Cells["A1"].LoadFromCollection(data8, true);

                    var fileInfo = new FileInfo(Path.Combine(directoryPath, "DataSet_Phamacity_MinhChau.xlsx"));
                    package.SaveAs(fileInfo);
                }

                // Lưu thông báo thành công vào TempData
                TempData["ExportMessage"] = "Xuất file Excel thành công!";
            }
            catch (Exception ex)
            {
                TempData["ExportMessage"] = "Có lỗi xảy ra: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
        private List<T> GetDataFromSql<T>(string query) where T : new()
        {
            var data = new List<T>();

            string connectionString = ConfigurationManager.ConnectionStrings["NhaThuocMC"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new T();

                            // Duyệt qua tất cả các cột trong SqlDataReader
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var property = typeof(T).GetProperty(reader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                                if (property != null && reader[i] != DBNull.Value)
                                {
                                    property.SetValue(item, reader[i]);
                                }
                            }

                            data.Add(item);
                        }
                    }
                }
            }

            return data;
        }
    }
}
