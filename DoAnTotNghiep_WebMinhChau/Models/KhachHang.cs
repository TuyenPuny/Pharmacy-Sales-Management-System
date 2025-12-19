using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    [Table("KhachHang")]
    public class KhachHang
    {
        [Key]
        [Column(TypeName = "char")]
        [StringLength(100)]
        public string EmailKH { get; set; } // Khóa chính, không được null, tối đa 100 ký tự

        [StringLength(50)]
        public string HoTen { get; set; } // Tên khách hàng, tối đa 50 ký tự

        public int? NamSinh { get; set; } // Năm sinh của khách hàng, với ràng buộc kiểm tra

        [StringLength(100)]
        public string TinhThanh { get; set; }

        [StringLength(100)]
        public string QuanHuyen { get; set; }

        [StringLength(100)]
        public string PhuongXa { get; set; }

        [StringLength(50)]
        public string DiaChi { get; set; }

        [StringLength(20)]
        public string GioiTinh { get; set; } // Giới tính khách hàng, tối đa 20 ký tự

        [Column(TypeName = "char")]
        [StringLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SDT { get; set; } // Số điện thoại, tối đa 10 ký tự, ràng buộc unique

        public List<KhachHang> danhsachKH { get; private set; }
        public List<KhachHang> danhsachKH_CNTT { get; private set; }

        public void LoadKH()
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmdKH = new SqlCommand(
                    "SELECT EmailKH, HoTen, NamSinh, TinhThanh, QuanHuyen, PhuongXa, DiaChi, GioiTinh, SDT FROM KhachHang", conn);

                using (SqlDataReader drKH = cmdKH.ExecuteReader())
                {
                    danhsachKH = new List<KhachHang>();
                    while (drKH.Read())
                    {
                        danhsachKH.Add(new KhachHang
                        {
                            EmailKH = drKH["EmailKH"].ToString(),
                            HoTen = drKH["HoTen"].ToString(),
                            NamSinh = drKH.IsDBNull(drKH.GetOrdinal("NamSinh")) ? (int?)null : Convert.ToInt32(drKH["NamSinh"]),
                            TinhThanh = drKH["TinhThanh"].ToString(),
                            QuanHuyen = drKH["QuanHuyen"].ToString(),
                            PhuongXa = drKH["PhuongXa"].ToString(),
                            DiaChi = drKH["DiaChi"].ToString(),
                            GioiTinh = drKH["GioiTinh"].ToString(),
                            SDT = drKH["SDT"].ToString(),
                        });
                    }
                }
            }
        }

        public void LoadKH_CNTT()
        {
            
            string email = HttpContext.Current.Session["Email"] as string; 

            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmdKH = new SqlCommand(
                    "SELECT EmailKH, HoTen, NamSinh, TinhThanh, QuanHuyen, PhuongXa, DiaChi, GioiTinh, SDT FROM KhachHang WHERE EmailKH = @EmailKH", conn);

          
                cmdKH.Parameters.AddWithValue("@EmailKH", email);

                using (SqlDataReader drKH = cmdKH.ExecuteReader())
                {
                    danhsachKH_CNTT = new List<KhachHang>();
                    while (drKH.Read())
                    {
                        danhsachKH_CNTT.Add(new KhachHang
                        {
                            EmailKH = drKH["EmailKH"].ToString(),
                            HoTen = drKH["HoTen"].ToString(),
                            NamSinh = drKH.IsDBNull(drKH.GetOrdinal("NamSinh")) ? (int?)null : Convert.ToInt32(drKH["NamSinh"]),
                            TinhThanh = drKH["TinhThanh"].ToString(),
                            QuanHuyen = drKH["QuanHuyen"].ToString(),
                            PhuongXa = drKH["PhuongXa"].ToString(),
                            DiaChi = drKH["DiaChi"].ToString(),
                            GioiTinh = drKH["GioiTinh"].ToString(),
                            SDT = drKH["SDT"].ToString(),
                        });
                    }
                }
            }
        }
    }
}