using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    public class CheckoutVM
    {
            public string HoTen { get; set; } // Họ tên người mua hàng
            public string DiaChi { get; set; } // Địa chỉ giao hàng
            public string SoDienThoai { get; set; } // Số điện thoại liên hệ
            public string Email { get; set; } // Email liên hệ
            public string PhuongThucThanhToan { get; set; } // Phương thức thanh toán (COD, VnPay, v.v.)
            public double TongTien { get; set; } // Tổng tiền thanh toán

            // Các thuộc tính khác cần thiết cho quá trình thanh toán có thể được thêm vào đây
    }

}