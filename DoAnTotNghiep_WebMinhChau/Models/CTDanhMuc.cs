using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    [Table("CTDanhMuc")]
    public class CTDanhMuc
    {
        [Key, Column(TypeName = "char", Order = 0)]
        [StringLength(10)]
        public string MaDanhMuc { get; set; }

        [Key, Column(TypeName = "char", Order = 1)]
        [StringLength(10)]
        public string MaCTDM { get; set; }

        [StringLength(100)]
        public string TenCTDM { get; set; }
        public List<CTDanhMuc> danhsachCTDM { get; set; }

        public CTDanhMuc()
        {
            // Khởi tạo danh sách khi khởi tạo đối tượng
            danhsachCTDM = new List<CTDanhMuc>();
        }

        // Foreign key reference to DanhMucSP
        [ForeignKey("MaDanhMuc")]
        public virtual DanhMucSP DanhMucSP { get; set; }

        public void loadCTDanhMuc()
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT MaDanhMuc, MaCTDM,TenCTDM FROM CTDanhMuc", conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        danhsachCTDM.Clear(); // Đảm bảo danh sách được làm sạch trước khi thêm mới
                        while (dr.Read())
                        {
                            danhsachCTDM.Add(new CTDanhMuc
                            {
                                MaDanhMuc = dr["MaDanhMuc"].ToString(),
                                MaCTDM = dr["MaCTDM"].ToString(),
                                TenCTDM = dr["TenCTDM"].ToString(),
                            });
                        }
                    }
                }
            }
        }
    }
}