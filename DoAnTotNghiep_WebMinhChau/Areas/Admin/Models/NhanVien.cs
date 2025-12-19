namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("NhanVien")]
    public partial class NhanVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhanVien()
        {
            DanhGiaSP = new HashSet<DanhGiaSP>();
            PhieuThu = new HashSet<PhieuThu>();
            TaiKhoan = new HashSet<TaiKhoan>();
        }

        [Key]
        [StringLength(10)]
        [Required(ErrorMessage = "Mã nhân viên không được để trống.")]
        [Remote("CheckMaNVExists", "NhanVien", ErrorMessage = "Mã nhân viên đã tồn tại.")]
        public string MaNV { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Họ tên không được để trống.")]
        public string HoTen { get; set; }

        [Range(1900, int.MaxValue, ErrorMessage = "Năm sinh không hợp lệ. Năm sinh không được lớn hơn năm hiện tại.")]
        public int? NamSinh { get; set; }

        [StringLength(50)]
        public string DiaChi { get; set; }

        [StringLength(20)]
        public string GioiTinh { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [StringLength(10, ErrorMessage = "Số điện thoại phải có độ dài chính xác 10 ký tự.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Remote("CheckPhoneExists", "NhanVien", ErrorMessage = "Số điện thoại đã được sử dụng.")]
        public string SDT { get; set; }
        [StringLength(100)]
        public string TrangThai { get; set; }


        [Column(TypeName = "date")]
        public DateTime? NgayVaoLam { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGiaSP> DanhGiaSP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuThu> PhieuThu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaiKhoan> TaiKhoan { get; set; }
    }
}
