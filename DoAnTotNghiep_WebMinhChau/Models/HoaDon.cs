using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key]
        [StringLength(100)]
        public string MaHD { get; set; }

        [StringLength(100)]
        public string HoTen { get; set; }

        public DateTime ThoiGian { get; set; } = DateTime.Now;


        [StringLength(10)]
        public string SDT { get; set; }

        [StringLength(100)]
        public string HinhThucNhanHanh { get; set; }

        [Required]
        [StringLength(100)]
        public string EmailKH { get; set; }

        [StringLength(100)]
        public string TinhThanh { get; set; }

        [StringLength(100)]
        public string QuanHuyen { get; set; }

        [StringLength(100)]
        public string PhuongXa { get; set; }

        [StringLength(225)]
        public string DiaChiNhanHang { get; set; }

        [StringLength(100)]
        public string PTTT { get; set; }

        public decimal TongTien { get; set; }

        [StringLength(100)]
        public string TrangThai { get; set; } = "Chờ xác nhận";

        // Navigation property
        public ICollection<CTHoaDon> CTHoaDon { get; set; }

        public List<HoaDon> LoadHD_DN(string emailKH)
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString;
            List<HoaDon> danhsachHD = new List<HoaDon>();

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmdHD = new SqlCommand(
                    "SELECT MaHD, HoTen, ThoiGian, SDT, HinhThucNhanHanh, TinhThanh, QuanHuyen, PhuongXa, DiaChiNhanHang, PTTT, TongTien, TrangThai FROM HoaDon WHERE EmailKH = @EmailKH", conn);

                cmdHD.Parameters.AddWithValue("@EmailKH", emailKH);

                using (SqlDataReader drHD = cmdHD.ExecuteReader())
                {
                    while (drHD.Read())
                    {
                        danhsachHD.Add(new HoaDon
                        {
                            MaHD = drHD["MaHD"].ToString(),
                            HoTen = drHD["HoTen"].ToString(),
                            ThoiGian = drHD.IsDBNull(drHD.GetOrdinal("ThoiGian")) ? DateTime.Now : Convert.ToDateTime(drHD["ThoiGian"]),
                            SDT = drHD["SDT"].ToString(),
                            HinhThucNhanHanh = drHD["HinhThucNhanHanh"].ToString(),
                            TinhThanh = drHD["TinhThanh"].ToString(),
                            QuanHuyen = drHD["QuanHuyen"].ToString(),
                            PhuongXa = drHD["PhuongXa"].ToString(),
                            DiaChiNhanHang = drHD["DiaChiNhanHang"].ToString(),
                            PTTT = drHD["PTTT"].ToString(),
                            TongTien = drHD.IsDBNull(drHD.GetOrdinal("TongTien")) ? 0 : Convert.ToDecimal(drHD["TongTien"]),
                            TrangThai = drHD["TrangThai"].ToString()
                        });
                    }
                }
            }

            return danhsachHD;
        }
        public List<HoaDon> LoadHD(string sdt)
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString;
            List<HoaDon> danhsachHD = new List<HoaDon>();

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmdHD = new SqlCommand(
                    "SELECT MaHD, HoTen, ThoiGian, HinhThucNhanHanh, TinhThanh, QuanHuyen, PhuongXa, DiaChiNhanHang, PTTT, TongTien, TrangThai FROM HoaDon WHERE SDT = @sdt", conn);

                cmdHD.Parameters.AddWithValue("@sdt", sdt);

                using (SqlDataReader drHD = cmdHD.ExecuteReader())
                {
                    while (drHD.Read())
                    {
                        danhsachHD.Add(new HoaDon
                        {
                            MaHD = drHD["MaHD"].ToString(),
                            HoTen = drHD["HoTen"].ToString(),
                            ThoiGian = drHD.IsDBNull(drHD.GetOrdinal("ThoiGian")) ? DateTime.Now : Convert.ToDateTime(drHD["ThoiGian"]),                 
                            HinhThucNhanHanh = drHD["HinhThucNhanHanh"].ToString(),
                            TinhThanh = drHD["TinhThanh"].ToString(),
                            QuanHuyen = drHD["QuanHuyen"].ToString(),
                            PhuongXa = drHD["PhuongXa"].ToString(),
                            DiaChiNhanHang = drHD["DiaChiNhanHang"].ToString(),
                            PTTT = drHD["PTTT"].ToString(),
                            TongTien = drHD.IsDBNull(drHD.GetOrdinal("TongTien")) ? 0 : Convert.ToDecimal(drHD["TongTien"]),
                            TrangThai = drHD["TrangThai"].ToString()
                        });
                    }
                }
            }

            return danhsachHD;
        }

    }
}
