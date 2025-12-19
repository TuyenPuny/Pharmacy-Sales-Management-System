using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("name=ketnoiSQL")
        {
        }
        public DbSet<SanPham> SanPham { get; set; }
        
        public DbSet<DanhMucSP> DanhMuc { get; set; }
        public DbSet<CTDanhMuc> CTDanhMuc { get; set; }

        public DbSet<CartItem> GioHang { get; set; }

        public DbSet<HoaDon> HoaDon { get; set; }

        public DbSet<CTHoaDon> CTHoaDon { get; set; }

        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<TaiKhoan> TaiKhoan { get; set; }


    }

}
