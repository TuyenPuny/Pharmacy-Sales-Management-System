namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CTDanhMuc")]
    public partial class CTDanhMuc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CTDanhMuc()
        {
            SanPham = new HashSet<SanPham>();
        }

        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string MaDanhMuc { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string MaCTDM { get; set; }

        [StringLength(100)]
        public string TenCTDM { get; set; }

        public virtual DanhMucSP DanhMucSP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPham { get; set; }
    }
}
