namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            CTPhieuDat = new HashSet<CTPhieuDat>();
            CTPhieuThu = new HashSet<CTPhieuThu>();
            CTHoaDon = new HashSet<CTHoaDon>();
            ChiTietSP = new HashSet<ChiTietSP>();
            DanhGiaSP = new HashSet<DanhGiaSP>();
            GioHang = new HashSet<GioHang>();
        }

        [Key]
        [StringLength(10)]
        public string MaSP { get; set; }

        [Required]
        [StringLength(10)]
        public string MaNSX { get; set; }

        [Required]
        [StringLength(10)]
        public string MaDanhMuc { get; set; }

        [Required]
        [StringLength(10)]
        public string MaCTDM { get; set; }

        [StringLength(255)]
        public string TenSP { get; set; }

        public int? GiaBan { get; set; }

        public int? GiaKM { get; set; }

        public int? GiaNhap { get; set; }

        [StringLength(255)]
        public string HinhAnh { get; set; }

        public virtual CTDanhMuc CTDanhMuc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTPhieuDat> CTPhieuDat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTPhieuThu> CTPhieuThu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTHoaDon> CTHoaDon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietSP> ChiTietSP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGiaSP> DanhGiaSP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHang { get; set; }

        public virtual NhaSanXuat NhaSanXuat { get; set; }
    }
}
