using System.Linq;
using System.Web.Http;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;

namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    [RoutePrefix("api/ctdanhmuc")]
    public class API_CTDanhMucController : ApiController
    {
        private Model1 db = new Model1();

        // GET api/ctdanhmuc/{maDanhMuc}
        [HttpGet]
        [Route("{maDanhMuc}")]
        public IHttpActionResult GetChiTietDanhMuc(string maDanhMuc)
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
