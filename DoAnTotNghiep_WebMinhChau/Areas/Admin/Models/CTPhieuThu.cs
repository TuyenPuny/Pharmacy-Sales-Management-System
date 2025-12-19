namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CTPhieuThu")]
    public partial class CTPhieuThu
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string MaPT { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string MaSP { get; set; }

        public int? SoLuong { get; set; }

        public int? DonGia { get; set; }

        public virtual SanPham SanPham { get; set; }

        public virtual PhieuThu PhieuThu { get; set; }
    }
}
