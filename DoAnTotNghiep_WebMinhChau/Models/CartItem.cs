using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    [Serializable]

    public class CartItem
    {
        public int Id { get; set; } // Khóa chính

        public string MaSP { get; set; } // Thuộc tính khóa ngoại
        public string EmailKH { get; set; } // Thuộc tính khóa ngoại

        public virtual SanPham product { get; set; } // liên kết tới SanPham

        public virtual KhachHang CartKH { get; set; } // liên kết tới KhachHang

        public int Quantity { get; set; }

        public int ThanhTien => Quantity * (product?.GiaKM != null ? product.GiaKM.Value : product?.GiaBan ?? 0);

        public int TongTien { get; set; }

        List<CartItem> cartItems = new List<CartItem>(); // Khởi tạo danh sách giỏ hàng
        public List<CartItem> loadCart(string emailKH)
        {         
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                // Câu truy vấn để lấy thông tin từ GioHang và SanPham
                SqlCommand cmdCart = new SqlCommand(
                    @"SELECT 
                sp.MaSP,
                sp.TenSP,
                sp.HinhAnh,
                CASE 
                    WHEN sp.GiaKM IS NOT NULL THEN sp.GiaKM 
                    ELSE sp.GiaBan 
                END AS Gia,
                gh.SoLuong,
                gh.ThanhTien
            FROM 
                SanPham sp
            JOIN 
                GioHang gh ON sp.MaSP = gh.MaSP
            WHERE 
                gh.EmailKH = @EmailKH", conn);

                // Thêm tham số để tránh SQL Injection
                cmdCart.Parameters.AddWithValue("@EmailKH", emailKH);

                using (SqlDataReader drCart = cmdCart.ExecuteReader())
                {
                    while (drCart.Read())
                    {
                        cartItems.Add(new CartItem
                        {
                            product = new SanPham
                            {
                                MaSP = drCart["MaSP"].ToString(),
                                TenSP = drCart["TenSP"].ToString(),
                                HinhAnh = drCart["HinhAnh"].ToString(),
                                GiaBan = drCart.IsDBNull(drCart.GetOrdinal("Gia")) ? (int?)null : Convert.ToInt32(drCart["Gia"])
                            },
                            Quantity = drCart.IsDBNull(drCart.GetOrdinal("SoLuong")) ? 0 : Convert.ToInt32(drCart["SoLuong"]),
                            TongTien = drCart.IsDBNull(drCart.GetOrdinal("ThanhTien")) ? 0 : Convert.ToInt32(drCart["ThanhTien"]),
                        });
                    }
                }
            }
            return cartItems; // Trả về danh sách giỏ hàng
        }



    }
}