using System.Linq;
using System.Web.Http;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    [RoutePrefix("api/danhmucsp")]
    public class API_DanhMucController : ApiController
    {
        private Model1 db = new Model1();

        // GET api/danhmucsp
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetDanhMucSP()
        {
            var danhMucSP = db.DanhMucSP
                .Select(dm => new { dm.MaDanhMuc, dm.TenDanhMuc })
                .ToList();

            if (danhMucSP == null || !danhMucSP.Any())
            {
                return NotFound();
            }

            return Ok(danhMucSP);
        }

        // GET api/danhmucsp/{maDanhMuc}/ctdanhmuc
        [HttpGet]
        [Route("{maDanhMuc}/ctdanhmuc")]
        public IHttpActionResult GetCTDanhMucByMaDanhMuc(string maDanhMuc)
        {
            var chiTietDanhMuc = db.CTDanhMuc
                .Where(ctdm => ctdm.MaDanhMuc == maDanhMuc)
                .Select(ctdm => new { ctdm.MaCTDM, ctdm.TenCTDM })
                .ToList();

            if (chiTietDanhMuc == null || !chiTietDanhMuc.Any())
            {
                return NotFound();
            }

            return Ok(chiTietDanhMuc);
        }
    }
}
