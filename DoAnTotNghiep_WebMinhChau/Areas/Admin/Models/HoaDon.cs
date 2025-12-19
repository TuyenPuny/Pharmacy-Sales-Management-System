namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDon")]
    public partial class HoaDon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoaDon()
        {
            CTHoaDon = new HashSet<CTHoaDon>();
        }

        [Key]
        [StringLength(100)]
        public string MaHD { get; set; }

        [StringLength(100)]
        public string HoTen { get; set; }

        public DateTime? thoiGian { get; set; }

        [StringLength(10)]
        public string SDT { get; set; }

        [StringLength(100)]
        public string HinhThucNhanHanh { get; set; }

        [StringLength(100)]
        public string EmailKH { get; set; }

        [StringLength(100)]
        public string TinhThanh { get; set; }

        [StringLength(100)]
        public string QuanHuyen { get; set; }

        [StringLength(100)]
        public string PhuongXa { get; set; }

        [StringLength(225)]
        public string DiaChiNhanHang { get; set; }

        [StringLength(100)]
        public string PTTT { get; set; }

        public decimal? TongTien { get; set; }

        [StringLength(100)]
        public string TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTHoaDon> CTHoaDon { get; set; }

        public virtual KhachHang KhachHang { get; set; }
    }
}
