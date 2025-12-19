using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAnTotNghiep_WebMinhChau.Models
{
    [Table("TaiKhoan")]
    public class TaiKhoan
    {
        [Key]
        public int IDTK { get; set; } // Khóa chính, tự tăng

        [Required]
        [StringLength(50)]
        public string MatKhau { get; set; } // Mật khẩu không null, tối đa 50 ký tự

        [Required]
        [StringLength(50)]
        public string VaiTro { get; set; } // Vai trò không null, tối đa 50 ký tự

        [StringLength(10)]
        public string MaNV { get; set; } // Mã nhân viên có thể null, tối đa 10 ký tự, liên kết với bảng NhanVien

        [StringLength(100)]
        public string EmailKH { get; set; } // Email khách hàng có thể null, tối đa 100 ký tự, liên kết với bảng KhachHang

        // Navigation properties
        public virtual KhachHang KhachHang { get; set; } // Liên kết với bảng KhachHang
        public virtual NhanVien NhanVien { get; set; } // Liên kết với bảng NhanVien
    }
}