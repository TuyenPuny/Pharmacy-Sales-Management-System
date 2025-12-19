using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    public class NhanVien
    {
        [Key]
        [Column(TypeName = "char")]
        [StringLength(10)]
        public string MaNV { get; set; } // Mã nhân viên, khóa chính, tối đa 10 ký tự

        [StringLength(50)]
        public string HoTen { get; set; } // Họ tên nhân viên, tối đa 50 ký tự

        public int? NamSinh { get; set; } // Năm sinh nhân viên, có ràng buộc kiểm tra

        [StringLength(50)]
        public string DiaChi { get; set; } // Địa chỉ nhân viên, tối đa 50 ký tự

        [StringLength(20)]
        public string GioiTinh { get; set; } // Giới tính nhân viên, tối đa 20 ký tự

        [Column(TypeName = "char")]
        [StringLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SDT { get; set; } // Số điện thoại, tối đa 10 ký tự, unique

        [Column(TypeName = "date")]
        public DateTime? NgayVaoLam { get; set; } // Ngày vào làm của nhân viên
    }
}