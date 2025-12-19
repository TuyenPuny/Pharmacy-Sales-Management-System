using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    [Table("DanhMucSP")]
    public class DanhMucSP
    {
        [Key]
        [Column(TypeName = "char")]
        [StringLength(10)]
        public string MaDanhMuc { get; set; }

        [StringLength(50)]
        public string TenDanhMuc { get; set; }

        public List<DanhMucSP> danhsachDM { get; set; }

        // Navigation property to CTDanhMuc (nếu có nhiều chi tiết danh mục)
        public virtual ICollection<CTDanhMuc> CTDanhMuc { get; set; }

        public DanhMucSP()
        {
            // Khởi tạo danh sách khi khởi tạo đối tượng
            danhsachDM = new List<DanhMucSP>();
        }

        public void loadDanhMuc()
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT MaDanhMuc, TenDanhMuc FROM DanhMucSP", conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        danhsachDM.Clear(); // Đảm bảo danh sách được làm sạch trước khi thêm mới
                        while (dr.Read())
                        {
                            danhsachDM.Add(new DanhMucSP
                            {
                                MaDanhMuc = dr["MaDanhMuc"].ToString(),
                                TenDanhMuc = dr["TenDanhMuc"].ToString()
                            });
                        }
                    }
                }
            }
        }
    }
}