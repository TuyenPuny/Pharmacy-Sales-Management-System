using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DoAnTotNghiep_WebMinhChau.Areas.Admin.Models;


namespace DoAnTotNghiep_WebMinhChau.Areas.Admin.Controllers
{
    [RoutePrefix("api/nsx")]
    public class API_NSXController : ApiController
    {
        private Model1 db = new Model1();

        // GET api/danhmucsp
        [HttpGet]
        [Route("")]
        public IHttpActionResult nhasanxuat()
        {
            var nsx = db.NhaSanXuat
                .Select(dm => new { dm.MaNSX, dm.TenNSX })
                .ToList();

            if (nsx == null || !nsx.Any())
            {
                return NotFound();
            }

            return Ok(nsx);
        }
    }
}
