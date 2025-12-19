using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    public class ChiTietHoaDonViewModel
    {
        public HoaDon HoaDon { get; set; }
        public List<ChiTietSanPhamViewModel> ChiTietHoaDon { get; set; }
    }
    public class ChiTietSanPhamViewModel
    {
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public int ThanhTien { get; set; }
    }
}