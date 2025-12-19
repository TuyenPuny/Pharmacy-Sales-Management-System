namespace DoAnTotNghiep_WebMinhChau.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.__MigrationHistory",
                c => new
                    {
                        MigrationId = c.String(nullable: false, maxLength: 150),
                        ContextKey = c.String(nullable: false, maxLength: 300),
                        Model = c.Binary(nullable: false),
                        ProductVersion = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => new { t.MigrationId, t.ContextKey });
            
            CreateTable(
                "dbo.CTDanhMuc",
                c => new
                    {
                        MaDanhMuc = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        MaCTDM = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        TenCTDM = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => new { t.MaDanhMuc, t.MaCTDM })
                .ForeignKey("dbo.DanhMucSP", t => t.MaDanhMuc)
                .Index(t => t.MaDanhMuc);
            
            CreateTable(
                "dbo.DanhMucSP",
                c => new
                    {
                        MaDanhMuc = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        TenDanhMuc = c.String(nullable: false, maxLength: 50),
                        TrangThai = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MaDanhMuc);
            
            CreateTable(
                "dbo.SanPham",
                c => new
                    {
                        MaSP = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        MaNSX = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        MaDanhMuc = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        MaCTDM = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        TenSP = c.String(maxLength: 255),
                        GiaBan = c.Int(),
                        GiaKM = c.Int(),
                        GiaNhap = c.Int(),
                        HinhAnh = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.MaSP)
                .ForeignKey("dbo.NhaSanXuat", t => t.MaNSX)
                .ForeignKey("dbo.CTDanhMuc", t => new { t.MaDanhMuc, t.MaCTDM })
                .Index(t => t.MaNSX)
                .Index(t => new { t.MaDanhMuc, t.MaCTDM });
            
            CreateTable(
                "dbo.CTPhieuDat",
                c => new
                    {
                        MaPDH = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        MaSP = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        SoLuong = c.Int(),
                        DonGia = c.Int(),
                    })
                .PrimaryKey(t => new { t.MaPDH, t.MaSP })
                .ForeignKey("dbo.PhieuDatHang", t => t.MaPDH)
                .ForeignKey("dbo.SanPham", t => t.MaSP)
                .Index(t => t.MaPDH)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.PhieuDatHang",
                c => new
                    {
                        MaPDH = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        ThoiGian = c.DateTime(),
                        EmailKH = c.String(maxLength: 100, fixedLength: true, unicode: false),
                        DiaChi = c.String(maxLength: 255),
                        PTTT = c.String(maxLength: 100),
                        TongTien = c.Int(),
                        TrangThai = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.MaPDH)
                .ForeignKey("dbo.KhachHang", t => t.EmailKH)
                .Index(t => t.EmailKH);
            
            CreateTable(
                "dbo.KhachHang",
                c => new
                    {
                        EmailKH = c.String(nullable: false, maxLength: 100, fixedLength: true, unicode: false),
                        HoTen = c.String(maxLength: 50),
                        NamSinh = c.Int(),
                        DiaChi = c.String(maxLength: 50),
                        GioiTinh = c.String(maxLength: 20),
                        SDT = c.String(maxLength: 10, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.EmailKH);
            
            CreateTable(
                "dbo.HoaDon",
                c => new
                    {
                        MaHD = c.String(nullable: false, maxLength: 100, fixedLength: true, unicode: false),
                        HoTen = c.String(maxLength: 100),
                        ThoiGian = c.DateTime(nullable: false),
                        SDT = c.String(maxLength: 10),
                        HinhThucNhanHanh = c.String(maxLength: 100),
                        EmailKH = c.String(nullable: false, maxLength: 100, fixedLength: true, unicode: false),
                        TinhThanh = c.String(maxLength: 100),
                        QuanHuyen = c.String(maxLength: 100),
                        PhuongXa = c.String(maxLength: 100),
                        DiaChiNhanHang = c.String(maxLength: 225),
                        PTTT = c.String(maxLength: 100),
                        TongTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TrangThai = c.String(maxLength: 100),
                        MaNV = c.String(maxLength: 10, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.MaHD)
                .ForeignKey("dbo.NhanVien", t => t.MaNV)
                .ForeignKey("dbo.KhachHang", t => t.EmailKH)
                .Index(t => t.EmailKH)
                .Index(t => t.MaNV);
            
            CreateTable(
                "dbo.CTHoaDon",
                c => new
                    {
                        MaHD = c.String(nullable: false, maxLength: 100, fixedLength: true, unicode: false),
                        MaSP = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        SoLuong = c.Int(),
                        DonGia = c.Int(),
                    })
                .PrimaryKey(t => new { t.MaHD, t.MaSP })
                .ForeignKey("dbo.HoaDon", t => t.MaHD)
                .ForeignKey("dbo.SanPham", t => t.MaSP)
                .Index(t => t.MaHD)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.NhanVien",
                c => new
                    {
                        MaNV = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        HoTen = c.String(maxLength: 50),
                        NamSinh = c.Int(),
                        DiaChi = c.String(maxLength: 50),
                        GioiTinh = c.String(maxLength: 20),
                        SDT = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        NgayVaoLam = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.MaNV);
            
            CreateTable(
                "dbo.PhieuThu",
                c => new
                    {
                        MaPT = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        ThoiGian = c.DateTime(),
                        MaNV = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        MaNCC = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        TongTien = c.Int(),
                    })
                .PrimaryKey(t => t.MaPT)
                .ForeignKey("dbo.NhaCungCap", t => t.MaNCC)
                .ForeignKey("dbo.NhanVien", t => t.MaNV)
                .Index(t => t.MaNV)
                .Index(t => t.MaNCC);
            
            CreateTable(
                "dbo.CTPhieuThu",
                c => new
                    {
                        MaPT = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        MaSP = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        SoLuong = c.Int(),
                        DonGia = c.Int(),
                    })
                .PrimaryKey(t => new { t.MaPT, t.MaSP })
                .ForeignKey("dbo.PhieuThu", t => t.MaPT)
                .ForeignKey("dbo.SanPham", t => t.MaSP)
                .Index(t => t.MaPT)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.NhaCungCap",
                c => new
                    {
                        MaNCC = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        TenNCC = c.String(maxLength: 255),
                        DiaChi = c.String(maxLength: 255),
                        DienThoai = c.String(maxLength: 30, fixedLength: true, unicode: false),
                        Email = c.String(maxLength: 30, unicode: false),
                        TrangThai = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.MaNCC);
            
            CreateTable(
                "dbo.TaiKhoan",
                c => new
                    {
                        IDTK = c.Int(nullable: false, identity: true),
                        MatKhau = c.String(nullable: false, maxLength: 50, unicode: false),
                        VaiTro = c.String(nullable: false, maxLength: 50, unicode: false),
                        MaNV = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        EmailKH = c.String(maxLength: 100, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.IDTK)
                .ForeignKey("dbo.KhachHang", t => t.EmailKH)
                .ForeignKey("dbo.NhanVien", t => t.MaNV)
                .Index(t => t.MaNV)
                .Index(t => t.EmailKH);
            
            CreateTable(
                "dbo.ChiTietSP",
                c => new
                    {
                        MaCTSP = c.Int(nullable: false, identity: true),
                        MaSP = c.String(maxLength: 10, fixedLength: true, unicode: false),
                        ThanhPhan = c.String(maxLength: 255),
                        CongDung = c.String(maxLength: 255),
                        CachDung = c.String(maxLength: 255),
                        DVT = c.String(maxLength: 50),
                        TrangThai = c.String(maxLength: 50),
                        SoLuongTon = c.Int(),
                        NSX = c.DateTime(storeType: "date"),
                        HSD = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.MaCTSP)
                .ForeignKey("dbo.SanPham", t => t.MaSP)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.NhaSanXuat",
                c => new
                    {
                        MaNSX = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        TenNSX = c.String(maxLength: 255),
                        DiaChi = c.String(maxLength: 255),
                        DienThoai = c.String(maxLength: 30, fixedLength: true, unicode: false),
                        Email = c.String(maxLength: 30, fixedLength: true, unicode: false),
                        TrangThai = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.MaNSX);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SanPham", new[] { "MaDanhMuc", "MaCTDM" }, "dbo.CTDanhMuc");
            DropForeignKey("dbo.SanPham", "MaNSX", "dbo.NhaSanXuat");
            DropForeignKey("dbo.ChiTietSP", "MaSP", "dbo.SanPham");
            DropForeignKey("dbo.CTHoaDon", "MaSP", "dbo.SanPham");
            DropForeignKey("dbo.CTPhieuThu", "MaSP", "dbo.SanPham");
            DropForeignKey("dbo.CTPhieuDat", "MaSP", "dbo.SanPham");
            DropForeignKey("dbo.PhieuDatHang", "EmailKH", "dbo.KhachHang");
            DropForeignKey("dbo.HoaDon", "EmailKH", "dbo.KhachHang");
            DropForeignKey("dbo.TaiKhoan", "MaNV", "dbo.NhanVien");
            DropForeignKey("dbo.TaiKhoan", "EmailKH", "dbo.KhachHang");
            DropForeignKey("dbo.PhieuThu", "MaNV", "dbo.NhanVien");
            DropForeignKey("dbo.PhieuThu", "MaNCC", "dbo.NhaCungCap");
            DropForeignKey("dbo.CTPhieuThu", "MaPT", "dbo.PhieuThu");
            DropForeignKey("dbo.HoaDon", "MaNV", "dbo.NhanVien");
            DropForeignKey("dbo.CTHoaDon", "MaHD", "dbo.HoaDon");
            DropForeignKey("dbo.CTPhieuDat", "MaPDH", "dbo.PhieuDatHang");
            DropForeignKey("dbo.CTDanhMuc", "MaDanhMuc", "dbo.DanhMucSP");
            DropIndex("dbo.ChiTietSP", new[] { "MaSP" });
            DropIndex("dbo.TaiKhoan", new[] { "EmailKH" });
            DropIndex("dbo.TaiKhoan", new[] { "MaNV" });
            DropIndex("dbo.CTPhieuThu", new[] { "MaSP" });
            DropIndex("dbo.CTPhieuThu", new[] { "MaPT" });
            DropIndex("dbo.PhieuThu", new[] { "MaNCC" });
            DropIndex("dbo.PhieuThu", new[] { "MaNV" });
            DropIndex("dbo.CTHoaDon", new[] { "MaSP" });
            DropIndex("dbo.CTHoaDon", new[] { "MaHD" });
            DropIndex("dbo.HoaDon", new[] { "MaNV" });
            DropIndex("dbo.HoaDon", new[] { "EmailKH" });
            DropIndex("dbo.PhieuDatHang", new[] { "EmailKH" });
            DropIndex("dbo.CTPhieuDat", new[] { "MaSP" });
            DropIndex("dbo.CTPhieuDat", new[] { "MaPDH" });
            DropIndex("dbo.SanPham", new[] { "MaDanhMuc", "MaCTDM" });
            DropIndex("dbo.SanPham", new[] { "MaNSX" });
            DropIndex("dbo.CTDanhMuc", new[] { "MaDanhMuc" });
            DropTable("dbo.NhaSanXuat");
            DropTable("dbo.ChiTietSP");
            DropTable("dbo.TaiKhoan");
            DropTable("dbo.NhaCungCap");
            DropTable("dbo.CTPhieuThu");
            DropTable("dbo.PhieuThu");
            DropTable("dbo.NhanVien");
            DropTable("dbo.CTHoaDon");
            DropTable("dbo.HoaDon");
            DropTable("dbo.KhachHang");
            DropTable("dbo.PhieuDatHang");
            DropTable("dbo.CTPhieuDat");
            DropTable("dbo.SanPham");
            DropTable("dbo.DanhMucSP");
            DropTable("dbo.CTDanhMuc");
            DropTable("dbo.__MigrationHistory");
        }
    }
}
