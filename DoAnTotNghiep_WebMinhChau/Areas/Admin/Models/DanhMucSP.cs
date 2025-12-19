namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhMucSP")]
    public partial class DanhMucSP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DanhMucSP()
        {
            CTDanhMuc = new HashSet<CTDanhMuc>();
        }

        [Key]
        [StringLength(10)]
        public string MaDanhMuc { get; set; }

        [StringLength(50)]
        public string TenDanhMuc { get; set; }

        public bool TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTDanhMuc> CTDanhMuc { get; set; }
    }
}
