namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietSP")]
    public partial class ChiTietSP
    {
        [Key]
        public int MaCTSP { get; set; }

        [StringLength(10)]
        public string MaSP { get; set; }

        [StringLength(255)]
        public string ThanhPhan { get; set; }

        [StringLength(255)]
        public string CongDung { get; set; }

        [StringLength(255)]
        public string CachDung { get; set; }

        [StringLength(50)]
        public string DVT { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; }

        public int? SoLuongTon { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NSX { get; set; }

        [Column(TypeName = "date")]
        public DateTime? HSD { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
