using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    public class CTSanPham
    {
        [Key]
        public int MaCTSP { get; set; }

        [Required]
        [StringLength(10)]
        public string MaSP { get; set; }

        [StringLength(255)]
        public string ThanhPhan { get; set; }

        [StringLength(255)]
        public string CongDung { get; set; }

        [StringLength(255)]
        public string CachDung { get; set; }

        [StringLength(50)]
        public string DVT { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; } = "Còn hàng";

        public int SoLuongTon { get; set; }

        public DateTime? NSX { get; set; }

        public DateTime? HSD { get; set; }

        [ForeignKey("MaSP")]
        public virtual SanPham SanPham { get; set; }
        public List<DanhMucSP> danhsachDM { get; set; }

        public List<CTSanPham> ChiTietSP;
        public void loadCTSP(string id)
        {
            ChiTietSP = new List<CTSanPham>();
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString;
            SqlConnection conn = new SqlConnection(conStr);
            conn.Open();
            SqlCommand cmdSP = new SqlCommand("SELECT * FROM ChiTietSP WHERE MaSP = @MaSP", conn);
            cmdSP.Parameters.AddWithValue("@MaSP", id);
            SqlDataReader drSP = cmdSP.ExecuteReader();
            ChiTietSP = new List<CTSanPham>();
            while (drSP.Read())
            {
                ChiTietSP.Add(new CTSanPham
                {
                    MaCTSP = Convert.ToInt32(drSP["MaCTSP"]),
                    MaSP = drSP["MaSP"].ToString(),
                    ThanhPhan = drSP["ThanhPhan"].ToString(),
                    CongDung = drSP["CongDung"].ToString(),
                    CachDung = drSP["CachDung"].ToString(),
                    DVT = drSP["DVT"].ToString(),
                    TrangThai = drSP["TrangThai"].ToString(),
                    SoLuongTon = Convert.ToInt32(drSP["SoLuongTon"]),
                    NSX = drSP.IsDBNull(drSP.GetOrdinal("NSX")) ? (DateTime?)null : Convert.ToDateTime(drSP["NSX"]),
                    HSD = drSP.IsDBNull(drSP.GetOrdinal("HSD")) ? (DateTime?)null : Convert.ToDateTime(drSP["HSD"]),
                });
            }
        }
    }
}