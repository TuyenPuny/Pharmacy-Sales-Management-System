using DoAnTotNghiep_WebMinhChau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnTotNghiep_WebMinhChau.Controllers
{
    public class HoaDonController : Controller
    {
        DBContext db = new DBContext();
        // GET: HoaDon
        public ActionResult TheoDoiDonHang()
        {
            List<HoaDon> danhSachHD = new List<HoaDon>();

            if (Session["Name"] != null && Session["Email"] != null)
            {
                string email = Session["Email"].ToString();
                HoaDon hd = new HoaDon();
                danhSachHD = hd.LoadHD_DN(email);
            }
            else if (Session["SDT"] != null)
            {
                string sdt = Session["SDT"].ToString();
                HoaDon hd = new HoaDon();
                danhSachHD = hd.LoadHD(sdt);
            }
            return View(danhSachHD);
        }

        public ActionResult Edit(string MaHD, string actionType)
        {
            string sqlQuery = string.Empty;

            if (actionType == "NhanHang")
            {
                // Cập nhật trạng thái đơn hàng thành "Đã nhận được hàng"
                sqlQuery = "UPDATE HoaDon SET TrangThai = @p0 WHERE MaHD = @p1";
                db.Database.ExecuteSqlCommand(sqlQuery, "Đã nhận được hàng", MaHD);
            }
            else if (actionType == "HuyDon")
            {
                // Cập nhật trạng thái đơn hàng thành "Đã huỷ"
                sqlQuery = "UPDATE HoaDon SET TrangThai = @p0 WHERE MaHD = @p1";
                db.Database.ExecuteSqlCommand(sqlQuery, "Đã huỷ", MaHD);
            }

            return RedirectToAction("TheoDoiDonHang");
        }

        [HttpPost]
        public ActionResult SearchByPhone(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
     
                Session["SDT"] = phoneNumber;
            }

            return RedirectToAction("TheoDoiDonHang");
        }
        public ActionResult ClearPhoneSession()
        {
           
            Session["SDT"] = null;

 
            return RedirectToAction("TheoDoiDonHang");
        }

        public JsonResult GetChiTietHoaDon(string MaHD)
        {
            CTHoaDon cthd = new CTHoaDon();

            
            var chiTietHD = cthd.LoadCTHD(MaHD);

        
            return Json(chiTietHD, JsonRequestBehavior.AllowGet);
        }
    }
}