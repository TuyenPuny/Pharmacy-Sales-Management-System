using DoAnTotNghiep_WebMinhChau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace DoAnTotNghiep_WebMinhChau.Controllers
{
    public class HomeController : Controller
    {
        private SanPham sanPham = new SanPham();
        private readonly SanPham model = new SanPham();
        DBContext db = new DBContext();
        // GET: Homes
        public ActionResult Index(string ten, int? page)
        {
            sanPham.loadSP();
            DanhMucSP danhMucSP = new DanhMucSP();
            danhMucSP.loadDanhMuc();
            CTDanhMuc ctDanhMuc = new CTDanhMuc();
            ctDanhMuc.loadCTDanhMuc();
            ViewBag.DanhSachDanhMucSP = danhMucSP.danhsachDM;
            ViewBag.DanhSachCTDM = ctDanhMuc.danhsachCTDM;

            if (!string.IsNullOrEmpty(ten))
            {
                sanPham.searchSP(ten);
            }
            else
            {
                sanPham.danhsachSP_ten = sanPham.danhsachSP;
            }


            var pageNumber = page ?? 1;
            int pageSize = 12;
            var items = sanPham.danhsachSP;

            var pagedList = sanPham.danhsachSP_ten.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }


        // Hiển thị sản phẩm theo danh mục
        public ActionResult HienThiSanPhamTheoDanhMuc(string maDanhMuc)
        {
            sanPham.loadSP();
            DanhMucSP danhMucSP = new DanhMucSP();
            danhMucSP.loadDanhMuc();
            CTDanhMuc ctDanhMuc = new CTDanhMuc();
            ctDanhMuc.loadCTDanhMuc();
            ViewBag.DanhSachDanhMucSP = danhMucSP.danhsachDM;
            ViewBag.DanhSachCTDM = ctDanhMuc.danhsachCTDM;
            var sanPhamTheoDanhMuc = sanPham.danhsachSP
                                              .Where(sp => sp.MaDanhMuc == maDanhMuc)
                                              .ToList();
            return View("Index", sanPhamTheoDanhMuc.ToPagedList(1, 12));
        }

        public ActionResult HienThiSanPhamTheoCTDM(string maCTDM, string maDanhMuc)
        {
            
            sanPham.loadSP();
            DanhMucSP danhMucSP = new DanhMucSP();
            danhMucSP.loadDanhMuc();
            CTDanhMuc ctDanhMuc = new CTDanhMuc();
            ctDanhMuc.loadCTDanhMuc();

            ViewBag.DanhSachDanhMucSP = danhMucSP.danhsachDM;
            ViewBag.DanhSachCTDM = ctDanhMuc.danhsachCTDM;

        
            if (string.IsNullOrEmpty(maDanhMuc) || string.IsNullOrEmpty(maCTDM))
            {
                ViewBag.Message = "Thông tin danh mục không hợp lệ.";
                return View("Index", new List<SanPham>().ToPagedList(1, 12));
            }

     
            var ctDanhMucChon = ctDanhMuc.danhsachCTDM
                                        .FirstOrDefault(ct => ct.MaCTDM == maCTDM && ct.MaDanhMuc == maDanhMuc);

            if (ctDanhMucChon == null)
            {
                ViewBag.Message = "Không tìm thấy chi tiết danh mục với mã đã chọn.";
                return View("Index", new List<SanPham>().ToPagedList(1, 12));
            }

  
            var sanPhamTheoCTDM = sanPham.danhsachSP
                                          .Where(sp => sp.MaCTDM == maCTDM && sp.MaDanhMuc == maDanhMuc)
                                          .ToList();

     
            return View("Index", sanPhamTheoCTDM.ToPagedList(1, 12));
        }



        public ActionResult TinTuc()
        {
            return View();
        }
    }

}