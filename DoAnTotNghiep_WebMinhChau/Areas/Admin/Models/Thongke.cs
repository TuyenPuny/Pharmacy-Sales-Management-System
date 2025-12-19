using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    public class Thongke
    {
        //public decimal TongDoanhThu { get; set; }
        public decimal TongChiPhi { get; set; }
        public decimal TongLoiNhuan { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal? TongDoanhThu { get; set; }
        public decimal? TongSoLuong { get; set; }
        public int? TongSoDonHang { get; set; }

    }
}