using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{

    [Table("NhaSanXuat")]
    public class NhaSanXuat
    {
        [Key]
        [Column(TypeName = "char")]
        [StringLength(10)]
        public string MaNSX { get; set; }

        [StringLength(50)]
        public string TenNSX { get; set; }

        [StringLength(50)]
        public string DiaChi { get; set; }

        [Column(TypeName = "char")]
        [StringLength(30)]
        public string DienThoai { get; set; }

        [Column(TypeName = "char")]
        [StringLength(30)]
        [Index(IsUnique = true)]  // Đảm bảo email là duy nhất
        public string Email { get; set; }

    }
}