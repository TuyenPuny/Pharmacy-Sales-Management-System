using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    [Table("SanPham")]
    public class SanPham
    {
        public List<SanPham> danhsachSP;
        public List<SanPham> danhsachSP_ten { get; set; }

        [Key]
        [Column(TypeName = "char")]
        [StringLength(10)]
        public string MaSP { get; set; }

        [Required]
        [Column(TypeName = "char")]
        [StringLength(10)]
        public string MaNSX { get; set; }

        [Required]
        [Column(TypeName = "char")]
        [StringLength(10)]
        public string MaDanhMuc { get; set; }

        [Required]
        [Column(TypeName = "char")]
        [StringLength(10)]
        public string MaCTDM { get; set; }

        [StringLength(255)]
        public string TenSP { get; set; }

        public int? GiaBan { get; set; }
        public int? GiaKM { get; set; }
        public int? GiaNhap { get; set; }

        [StringLength(255)]
        public string HinhAnh { get; set; }

        // Navigation properties
        [ForeignKey("MaNSX")]
        public virtual NhaSanXuat NhaSanXuat { get; set; }

        [ForeignKey("MaDanhMuc, MaCTDM")]
        public virtual CTDanhMuc CTDanhMuc { get; set; }

        public void loadSP()
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                // Câu truy vấn được điều chỉnh theo bảng SanPham
                SqlCommand cmdSP = new SqlCommand(
                    "SELECT MaSP, MaNSX, MaDanhMuc, MaCTDM, TenSP, GiaBan, GiaKM, GiaNhap, HinhAnh FROM SanPham", conn);

                using (SqlDataReader drSP = cmdSP.ExecuteReader())
                {
                    danhsachSP = new List<SanPham>();
                    while (drSP.Read())
                    {
                        danhsachSP.Add(new SanPham
                        {
                            MaSP = drSP["MaSP"].ToString(),
                            MaNSX = drSP["MaNSX"].ToString(),
                            MaDanhMuc = drSP["MaDanhMuc"].ToString(),
                            MaCTDM = drSP["MaCTDM"].ToString(),
                            TenSP = drSP["TenSP"].ToString(),
                            GiaBan = drSP.IsDBNull(drSP.GetOrdinal("GiaBan")) ? (int?)null : Convert.ToInt32(drSP["GiaBan"]),
                            GiaKM = drSP.IsDBNull(drSP.GetOrdinal("GiaKM")) ? (int?)null : Convert.ToInt32(drSP["GiaKM"]),
                            GiaNhap = drSP.IsDBNull(drSP.GetOrdinal("GiaNhap")) ? (int?)null : Convert.ToInt32(drSP["GiaNhap"]),
                            HinhAnh = drSP["HinhAnh"].ToString(),
                        });
                    }
                }
            }
        }
        public void searchSP(string ten)
        {
            if (danhsachSP == null)
            {
                throw new InvalidOperationException("Danh sách sản phẩm chưa được tải.");
            }

            if (string.IsNullOrWhiteSpace(ten))
            {
             
                danhsachSP_ten = danhsachSP;
                return;
            }

            // Tách chuỗi tìm kiếm thành các từ, loại bỏ các khoảng trắng dư thừa
            var searchTerms = ten
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(term => term.Trim()) // Loại bỏ khoảng trắng ở đầu và cuối từ
                .ToList();

            // Tìm các sản phẩm mà ít nhất một từ trong chuỗi tìm kiếm xuất hiện trong tên sản phẩm
            danhsachSP_ten = danhsachSP
                .Where(sp => searchTerms.Any(term => sp.TenSP.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0))
                .ToList();
        }



        public void loadSP_DM(string maDanhMuc)
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlCommand cmdSP = new SqlCommand("SELECT MaSP, MaNSX, MaDanhMuc, MaCTDM, TenSP, GiaBan, GiaKM, GiaNhap, HinhAnh FROM SanPham WHERE MaDanhMuc = @MaDanhMuc", conn);
                cmdSP.Parameters.AddWithValue("@MaDanhMuc", maDanhMuc);

                using (SqlDataReader drSP = cmdSP.ExecuteReader())
                {
                    danhsachSP = new List<SanPham>();
                    while (drSP.Read())
                    {
                        danhsachSP.Add(new SanPham
                        {
                            MaSP = drSP["MaSP"].ToString(),
                            MaNSX = drSP["MaNSX"].ToString(),
                            MaDanhMuc = drSP["MaDanhMuc"].ToString(),
                            MaCTDM = drSP["MaCTDM"].ToString(),
                            TenSP = drSP["TenSP"].ToString(),
                            GiaBan = drSP.IsDBNull(drSP.GetOrdinal("GiaBan")) ? (int?)null : Convert.ToInt32(drSP["GiaBan"]),
                            GiaKM = drSP.IsDBNull(drSP.GetOrdinal("GiaKM")) ? (int?)null : Convert.ToInt32(drSP["GiaKM"]),
                            GiaNhap = drSP.IsDBNull(drSP.GetOrdinal("GiaNhap")) ? (int?)null : Convert.ToInt32(drSP["GiaNhap"]),
                            HinhAnh = drSP["HinhAnh"].ToString(),
                        });
                    }
                }
            }
        }

    }
}