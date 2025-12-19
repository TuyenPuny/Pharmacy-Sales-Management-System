namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhGiaSP")]
    public partial class DanhGiaSP
    {
        [Key]
        [StringLength(10)]
        public string MaDG { get; set; }

        [StringLength(10)]
        public string MaSP { get; set; }

        [StringLength(100)]
        public string EmailKH { get; set; }

        [StringLength(10)]
        public string MaNV { get; set; }

        public DateTime? ThoiGian { get; set; }

        [StringLength(255)]
        public string NoiDung { get; set; }

        public int? SoDiemDanhGia { get; set; }

        [StringLength(255)]
        public string PhanHoi { get; set; }

        public virtual KhachHang KhachHang { get; set; }

        public virtual NhanVien NhanVien { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
