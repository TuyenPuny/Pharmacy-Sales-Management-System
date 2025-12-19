namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CTPhieuDat")]
    public partial class CTPhieuDat
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string MaPDH { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string MaSP { get; set; }

        public int? SoLuong { get; set; }

        public int? DonGia { get; set; }

        public virtual SanPham SanPham { get; set; }

        public virtual PhieuDatHang PhieuDatHang { get; set; }
    }
}
