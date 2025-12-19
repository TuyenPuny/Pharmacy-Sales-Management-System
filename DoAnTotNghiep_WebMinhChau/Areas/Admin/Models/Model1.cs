using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=NhaThuocMC")
        {
        }

        public virtual DbSet<CTDanhMuc> CTDanhMuc { get; set; }
        public virtual DbSet<CTPhieuDat> CTPhieuDat { get; set; }
        public virtual DbSet<CTPhieuThu> CTPhieuThu { get; set; }
        public virtual DbSet<CTHoaDon> CTHoaDon { get; set; }
        public virtual DbSet<ChiTietSP> ChiTietSP { get; set; }
        public virtual DbSet<DanhGiaSP> DanhGiaSP { get; set; }
        public virtual DbSet<DanhMucSP> DanhMucSP { get; set; }
        public virtual DbSet<GioHang> GioHang { get; set; }
        public virtual DbSet<HoaDon> HoaDon { get; set; }
        public virtual DbSet<KhachHang> KhachHang { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCap { get; set; }
        public virtual DbSet<NhanVien> NhanVien { get; set; }
        public virtual DbSet<NhaSanXuat> NhaSanXuat { get; set; }
        public virtual DbSet<PhieuDatHang> PhieuDatHang { get; set; }
        public virtual DbSet<PhieuThu> PhieuThu { get; set; }
        public virtual DbSet<SanPham> SanPham { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }
        //public DbSet<InventoryPercentageViewModel> InventoryPercentageViewModel { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CTDanhMuc>()
                .Property(e => e.MaDanhMuc)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CTDanhMuc>()
                .Property(e => e.MaCTDM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CTDanhMuc>()
                .HasMany(e => e.SanPham)
                .WithRequired(e => e.CTDanhMuc)
                .HasForeignKey(e => new { e.MaDanhMuc, e.MaCTDM })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CTPhieuDat>()
                .Property(e => e.MaPDH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CTPhieuDat>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CTPhieuThu>()
                .Property(e => e.MaPT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CTPhieuThu>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CTHoaDon>()
                .Property(e => e.MaHD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CTHoaDon>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ChiTietSP>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DanhGiaSP>()
                .Property(e => e.MaDG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DanhGiaSP>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DanhGiaSP>()
                .Property(e => e.EmailKH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DanhGiaSP>()
                .Property(e => e.MaNV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DanhMucSP>()
                .Property(e => e.MaDanhMuc)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DanhMucSP>()
                .HasMany(e => e.CTDanhMuc)
                .WithRequired(e => e.DanhMucSP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GioHang>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GioHang>()
                .Property(e => e.EmailKH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HoaDon>()
                .Property(e => e.MaHD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HoaDon>()
                .Property(e => e.EmailKH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HoaDon>()
                .Property(e => e.TongTien)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HoaDon>()
                .HasMany(e => e.CTHoaDon)
                .WithRequired(e => e.HoaDon)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KhachHang>()
                .Property(e => e.EmailKH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KhachHang>()
                .Property(e => e.SDT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KhachHang>()
                .HasMany(e => e.GioHang)
                .WithRequired(e => e.KhachHang)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NhaCungCap>()
                .Property(e => e.MaNCC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NhaCungCap>()
                .Property(e => e.DienThoai)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NhaCungCap>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.MaNV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.SDT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NhaSanXuat>()
                .Property(e => e.MaNSX)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NhaSanXuat>()
                .Property(e => e.DienThoai)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NhaSanXuat>()
                .Property(e => e.Email)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NhaSanXuat>()
                .HasMany(e => e.SanPham)
                .WithRequired(e => e.NhaSanXuat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhieuDatHang>()
                .Property(e => e.MaPDH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PhieuDatHang>()
                .Property(e => e.EmailKH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PhieuDatHang>()
                .HasMany(e => e.CTPhieuDat)
                .WithRequired(e => e.PhieuDatHang)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhieuThu>()
                .Property(e => e.MaPT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PhieuThu>()
                .Property(e => e.MaNV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PhieuThu>()
                .Property(e => e.MaNCC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PhieuThu>()
                .HasMany(e => e.CTPhieuThu)
                .WithRequired(e => e.PhieuThu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.MaNSX)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.MaDanhMuc)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.MaCTDM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.CTPhieuDat)
                .WithRequired(e => e.SanPham)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.CTPhieuThu)
                .WithRequired(e => e.SanPham)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.CTHoaDon)
                .WithRequired(e => e.SanPham)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.GioHang)
                .WithRequired(e => e.SanPham)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.VaiTro)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.MaNV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.EmailKH)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
