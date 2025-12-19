namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuDatHang")]
    public partial class PhieuDatHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuDatHang()
        {
            CTPhieuDat = new HashSet<CTPhieuDat>();
        }

        [Key]
        [StringLength(10)]
        public string MaPDH { get; set; }

        public DateTime? ThoiGian { get; set; }

        [StringLength(100)]
        public string EmailKH { get; set; }

        [StringLength(255)]
        public string DiaChi { get; set; }

        [StringLength(100)]
        public string PTTT { get; set; }

        public int? TongTien { get; set; }

        [StringLength(100)]
        public string TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTPhieuDat> CTPhieuDat { get; set; }

        public virtual KhachHang KhachHang { get; set; }
    }
}
