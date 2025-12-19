using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    public class BC_ThongKe
    {
        public int? TongSoDonHang { get; set; }
        public decimal? TongDoanhThu { get; set; }
        public int TongSoLuongSanPham { get; set; }
        public decimal? LoiNhuan { get; set; }
        public double PhanTramHetHang { get; set; }
        public double PhanTramSapHet { get; set; }
        public double PhanTramConHang { get; set; }

        

    }
    public class ProductStat
    {
        public string MaSP { get; set; }
        public decimal DoanhThuSP { get; set; }
        public int TongSoLuongSanPham { get; set; }
        public string TenSP { get; set; }
        public DateTime HSD { get; set; }
        public int SoLuongTon { get; set; }
        public int TongSoLuongBan { get; set; }


    }

    public class OrderStatusStat
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int DangXuLy { get; set; }
        public int DaHoanThanh { get; set; }
        public int DaHuy { get; set; }
        public int ChoXacNhan { get; set; }

    }
}