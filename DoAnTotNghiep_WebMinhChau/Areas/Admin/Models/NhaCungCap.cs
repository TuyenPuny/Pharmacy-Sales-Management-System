namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhaCungCap")]
    public partial class NhaCungCap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhaCungCap()
        {
            PhieuThu = new HashSet<PhieuThu>();
        }

        [Key]
        [StringLength(10)]
        public string MaNCC { get; set; }

        [StringLength(255)]
        public string TenNCC { get; set; }

        [StringLength(255)]
        public string DiaChi { get; set; }

        [StringLength(30)]
        public string DienThoai { get; set; }

        [StringLength(30)]
        public string Email { get; set; }

        [StringLength(30)]
        public string TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuThu> PhieuThu { get; set; }
    }
}
