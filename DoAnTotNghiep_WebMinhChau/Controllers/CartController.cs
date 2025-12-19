using DoAnTotNghiep_WebMinhChau.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DoAnTotNghiep_WebMinhChau.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public DBContext db = new DBContext();
        private CartItem cartItem = new CartItem();
        private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            List<CartItem> list = new List<CartItem>();
            if (Session["Name"] == null)
            {
                var cart = Session[CartSession];
                if (cart != null)
                {
                    list = (List<CartItem>)cart;
                }
            }
            else
            {
                string emailKH = Session["Email"].ToString();
                list = cartItem.loadCart(emailKH);
            }

            // Tính tổng tiền
            decimal totalAmount = list.Sum(item => ((item.product?.GiaKM ?? 0.0m) > 0 ? (decimal)item.product.GiaKM : (decimal)(item.product?.GiaBan ?? 0.0m)) * item.Quantity);


          
            ViewBag.TotalAmount = totalAmount;

       
            Session["TotalAmount"] = totalAmount;

            return View(list);
        }

        //-----------------------------------------------------------Xóa toàn bộ giỏ hàng----------------------------------------------------------

        public JsonResult DeleteAll()
        {
            if (Session["Name"] == null)
            {
                Session[CartSession] = null;
            }
            else
            {
                string emailKH = Session["Email"].ToString();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmdDeleteAll = new SqlCommand(
                        @"DELETE FROM GioHang 
                  WHERE EmailKH = @EmailKH", conn);

                    cmdDeleteAll.Parameters.AddWithValue("@EmailKH", emailKH);

                    cmdDeleteAll.ExecuteNonQuery();
                }
            }

            return Json(new
            {
                status = true
            });
        }

        //-----------------------------------------------------------Xóa sản phẩm trong giỏ hàng----------------------------------------------------------

        public JsonResult Delete(string id)
        {
            // Lấy giỏ hàng từ session
            var sessionCart = (List<CartItem>)Session[CartSession];

            if (Session["Name"] == null) 
            {
               
                sessionCart.RemoveAll(x => x.product.MaSP == id);
                Session[CartSession] = sessionCart; 
            }
            else 
            {
                string emailKH = Session["Email"].ToString(); 

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString))
                {
                    conn.Open();

             
                    SqlCommand cmdDelete = new SqlCommand(
                        @"DELETE FROM GioHang WHERE MaSP = @MaSP AND EmailKH = @EmailKH", conn);

                    cmdDelete.Parameters.AddWithValue("@MaSP", id);
                    cmdDelete.Parameters.AddWithValue("@EmailKH", emailKH);

                    cmdDelete.ExecuteNonQuery();
                }
            }

            return Json(new
            {
                status = true
            });
        }

        //-----------------------------------------------------------Cập nhật số lượng của sản phẩm trong giỏ hàng----------------------------------------------------------

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];

            if (Session["Name"] == null)
            {
                foreach (var item in sessionCart)
                {
                    var updatedItem = jsonCart.SingleOrDefault(x => x.product.MaSP == item.product.MaSP);
                    if (updatedItem != null)
                    {
                        // Lấy số lượng tồn từ bảng ChiTietSP
                        var soLuongTon = db.Database.SqlQuery<int>("SELECT SoLuongTon FROM ChiTietSP WHERE MaSP = @p0", item.product.MaSP).SingleOrDefault();

                        // Kiểm tra số lượng muốn cập nhật có vượt quá tồn kho không
                        if (updatedItem.Quantity > soLuongTon)
                        {
                            return Json(new { status = false, message = $"Sản phẩm chỉ còn {soLuongTon} trong kho, không thể thêm {updatedItem.Quantity} vào giỏ." });
                        }

                        // Cập nhật số lượng trong giỏ hàng
                        item.Quantity = updatedItem.Quantity;
                    }
                }

                // Cập nhật lại giỏ hàng trong session
                Session[CartSession] = sessionCart;
            }
            else
            {
                string emailKH = Session["Email"].ToString();
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString))
                {
                    conn.Open();
                    foreach (var item in jsonCart)
                    {
                      
                        var soLuongTon = db.Database.SqlQuery<int>("SELECT SoLuongTon FROM ChiTietSP WHERE MaSP = @p0", item.product.MaSP).SingleOrDefault();

                      
                        if (item.Quantity > soLuongTon)
                        {
                            return Json(new { status = false, message = $"Sản phẩm chỉ còn {soLuongTon} trong kho, không thể thêm {item.Quantity} vào giỏ." });
                        }

                        var cmd = new SqlCommand(@"UPDATE GioHang SET SoLuong = @SoLuong WHERE MaSP = @MaSP AND EmailKH = @EmailKH", conn);
                        cmd.Parameters.AddWithValue("@SoLuong", item.Quantity);
                        cmd.Parameters.AddWithValue("@MaSP", item.product.MaSP);
                        cmd.Parameters.AddWithValue("@EmailKH", emailKH);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            return Json(new { status = true });
        }

        //-----------------------------------------------------------Thêm sản phẩm vào giỏ hàng----------------------------------------------------------

        [HttpPost]
        public JsonResult AddItem(string productId, int quantity)
        {
            // Lấy thông tin sản phẩm
            var prod = db.Database.SqlQuery<SanPham>("SELECT *, COALESCE(GiaKM, GiaBan) AS GiaBan FROM SanPham WHERE MaSP = @p0", productId).SingleOrDefault();
            if (prod == null) return Json(new { success = false, message = "Sản phẩm không tồn tại." });

            // Lấy số lượng tồn từ bảng ChiTietSP
            var soLuongTon = db.Database.SqlQuery<int>("SELECT SoLuongTon FROM ChiTietSP WHERE MaSP = @p0", productId).SingleOrDefault();

            // Kiểm tra nếu số lượng sản phẩm trong giỏ hàng cộng với số lượng cần thêm vượt quá số lượng tồn kho
            int currentQuantityInCart = 0;

            if (Session["Name"] == null)
            {
                var cart = Session[CartHelper.CartSession] as List<CartItem> ?? new List<CartItem>();
                var item = cart.FirstOrDefault(x => x.product.MaSP == productId);
                // Nếu sản phẩm đã có trong giỏ, lấy số lượng hiện tại
                if (item != null)
                {
                    currentQuantityInCart = item.Quantity; // Lấy số lượng hiện tại của sản phẩm trong giỏ hàng
                }
                int totalQuantityInCart = currentQuantityInCart + quantity;
                if (totalQuantityInCart > soLuongTon)
                {
                    return Json(new { success = false, message = $"Sản phẩm này chỉ còn {soLuongTon} trong kho. Không thể thêm tiếp {quantity} sản phẩm vào giỏ." });
                }
                if (item != null)
                {
                    currentQuantityInCart = item.Quantity; // Số lượng hiện tại trong giỏ
                    item.Quantity += quantity;
                }
                else
                {
                    cart.Add(new CartItem { product = prod, Quantity = quantity });
                }
                // Kiểm tra số lượng trong giỏ hàng + số lượng cần thêm có vượt quá tồn kho không
                

                Session[CartHelper.CartSession] = cart;
            }
            else
            {
                var emailKH = Session["Email"].ToString();
                var existingItem = db.Database.SqlQuery<int>("SELECT COUNT(*) FROM GioHang WHERE MaSP = @p0 AND EmailKH = @p1", new SqlParameter("@p0", productId), new SqlParameter("@p1", emailKH)).SingleOrDefault();

              
                if (existingItem > 0)
                {
                    currentQuantityInCart = db.Database.SqlQuery<int>("SELECT SoLuong FROM GioHang WHERE MaSP = @p0 AND EmailKH = @p1", new SqlParameter("@p0", productId), new SqlParameter("@p1", emailKH)).SingleOrDefault();
                }

                int totalQuantityInCart = currentQuantityInCart + quantity;

           
                if (totalQuantityInCart > soLuongTon)
                {
                    return Json(new { success = false, message = $"Sản phẩm này chỉ còn {soLuongTon} trong kho. Không thể thêm tiếp {quantity} sản phẩm vào giỏ." });
                }

                var query = existingItem > 0
                    ? "UPDATE GioHang SET SoLuong = SoLuong + @p0 WHERE MaSP = @p1 AND EmailKH = @p2"
                    : "INSERT INTO GioHang (SoLuong, MaSP, EmailKH) VALUES (@p0, @p1, @p2)";
                
                db.Database.ExecuteSqlCommand(query,
                    new SqlParameter("@p0", quantity),
                    new SqlParameter("@p1", productId),
                    new SqlParameter("@p2", emailKH.Trim()));
            }

            return Json(new { success = true });
        }

        //-----------------------------------------------------------Số sản phẩm trong giỏ hàng---------------------------------------------------------

        public ActionResult GetCartCount()
        {
            int cartCount = 0;
            if (Session["Name"] != null)
            {
                string emailKH = Session["Email"].ToString();
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ketnoiSQL"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT COUNT(*) FROM GioHang WHERE EmailKH = @EmailKH", conn);
                    cmd.Parameters.AddWithValue("@EmailKH", emailKH);

                    // Lấy số dòng có trong giỏ hàng
                    object result = cmd.ExecuteScalar();
                    cartCount = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;
                }
            }
            else
            {
                var cartItems = Session[CartSession] as List<CartItem> ?? new List<CartItem>();
                cartCount = cartItems.Count;
            }
            return Json(new { count = cartCount }, JsonRequestBehavior.AllowGet);
        }

        //-----------------------------------------------------------Kiểm tra số lượng tồn----------------------------------------------------------

        [HttpPost]
        public JsonResult CheckStock(string productId, int quantity)
        {
            var soLuongTon = db.Database.SqlQuery<int>("SELECT SoLuongTon FROM ChiTietSP WHERE MaSP = @p0", productId).SingleOrDefault();
            if (quantity > soLuongTon)
                return Json(new { success = false, message = $"Sản phẩm này chỉ còn {soLuongTon} trong kho. Bạn không thể đặt {quantity} sản phẩm." });

            return Json(new { success = true });
        }
    }


}

