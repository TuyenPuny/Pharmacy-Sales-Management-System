using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    public class SanPham_CTSP
    {
        public SanPham SanPham { get; set; }
        public CTSanPham ChiTietSanPham { get; set; }

        public List<CTSanPham> ChiTietSP;

        public class CTSanPham
        {
            public string ThanhPhan { get; set; }
            public string CongDung { get; set; }
            public string CachDung { get; set; }
            public string DVT { get; set; }
            public string TrangThai { get; set; }
            public int SoLuongTon { get; set; }
            public DateTime? NSX { get; set; }
            public DateTime? HSD { get; set; }
            

            public void loadCTSP(string id, out SanPham_CTSP sanPham_CTSP)
            {
                sanPham_CTSP = new SanPham_CTSP();
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();

                    // Truy vấn JOIN 2 bảng SanPham và ChiTietSP
                    string query = @"
                    SELECT sp.MaSP, sp.TenSP, sp.GiaBan, sp.GiaKM, sp.HinhAnh, 
                           ctsp.ThanhPhan, ctsp.CongDung, ctsp.CachDung, ctsp.DVT, ctsp.TrangThai, 
                           ctsp.SoLuongTon, ctsp.NSX, ctsp.HSD
                    FROM SanPham sp
                    INNER JOIN ChiTietSP ctsp ON sp.MaSP = ctsp.MaSP
                    WHERE sp.MaSP = @MaSP";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSP", id);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                sanPham_CTSP.SanPham = new SanPham
                                {
                                    MaSP = dr["MaSP"].ToString(),
                                    TenSP = dr["TenSP"].ToString(),
                                    GiaBan = dr.IsDBNull(dr.GetOrdinal("GiaBan")) ? (int?)null : Convert.ToInt32(dr["GiaBan"]),
                                    GiaKM = dr.IsDBNull(dr.GetOrdinal("GiaKM")) ? (int?)null : Convert.ToInt32(dr["GiaKM"]),
                                    HinhAnh = dr["HinhAnh"].ToString()
                                };

                                sanPham_CTSP.ChiTietSanPham = new CTSanPham
                                {
                                    ThanhPhan = dr["ThanhPhan"].ToString(),
                                    CongDung = dr["CongDung"].ToString(),
                                    CachDung = dr["CachDung"].ToString(),
                                    DVT = dr["DVT"].ToString(),
                                    TrangThai = dr["TrangThai"].ToString(),
                                    SoLuongTon = Convert.ToInt32(dr["SoLuongTon"]),
                                    NSX = dr.IsDBNull(dr.GetOrdinal("NSX")) ? (DateTime?)null : Convert.ToDateTime(dr["NSX"]),
                                    HSD = dr.IsDBNull(dr.GetOrdinal("HSD")) ? (DateTime?)null : Convert.ToDateTime(dr["HSD"])
                                };
                            }
                        }
                    }
                }
            }
        }
    }
}