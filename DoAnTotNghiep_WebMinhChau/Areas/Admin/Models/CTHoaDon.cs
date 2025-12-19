namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;

    [Table("CTHoaDon")]
    public partial class CTHoaDon
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(100)]
        public string MaHD { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string MaSP { get; set; }

        public int? SoLuong { get; set; }

        public int? DonGia { get; set; }

        public virtual SanPham SanPham { get; set; }

        public virtual HoaDon HoaDon { get; set; }

        public List<CTHoaDon> LoadCTHD(string mahd)
        {
            // Lấy chuỗi kết nối từ file cấu hình
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString;

            // Danh sách chi tiết hóa đơn
            List<CTHoaDon> danhSachCTHD = new List<CTHoaDon>();

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();

                // Câu lệnh SQL lấy thông tin chi tiết hóa đơn và tên sản phẩm
                SqlCommand cmdHD = new SqlCommand(
                    "SELECT CTHoaDon.MaHD, SanPham.TenSP, CTHoaDon.SoLuong, CTHoaDon.DonGia " +
                    "FROM CTHoaDon " +
                    "JOIN SanPham ON SanPham.MaSP = CTHoaDon.MaSP " +
                    "WHERE CTHoaDon.MaHD = @mahd", conn);

                // Thêm tham số vào câu truy vấn
                cmdHD.Parameters.AddWithValue("@mahd", mahd);

                using (SqlDataReader drHD = cmdHD.ExecuteReader())
                {
                    while (drHD.Read())
                    {
                        // Tạo đối tượng CTHoaDon và ánh xạ dữ liệu
                        CTHoaDon chiTietHD = new CTHoaDon
                        {
                            MaHD = drHD["MaHD"].ToString(),
                            SoLuong = Convert.ToInt32(drHD["SoLuong"]),
                            DonGia = Convert.ToInt32(drHD["DonGia"]),
                            SanPham = new SanPham
                            {
                                TenSP = drHD["TenSP"].ToString()
                            }
                        };

                        // Thêm vào danh sách
                        danhSachCTHD.Add(chiTietHD);
                    }
                }
            }

            // Trả về danh sách chi tiết hóa đơn
            return danhSachCTHD;
        }
    }
}
