using FoodOrderingSystem.Models.Cart;
using FoodOrderingSystem.Models.item;
using FoodOrderingSystem.Services.Implements;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Controllers
{
    [Route("/CustomerDashboard")]
    public class CustomerDashboardController : Controller
    {
        private readonly ILogger<CustomerDashboardController> _logger;

        private readonly IItemService itemService;
        private readonly ICustomerOrderService customerOrderService;
        private readonly IOrderDetailsService orderDetailsService;

        public CustomerDashboardController(ILogger<CustomerDashboardController> logger, IItemService _itemService
                                        ,ICustomerOrderService _customerOrderService, IOrderDetailsService _orderDetailsService
                                        )
        {
            _logger = logger;
            itemService = _itemService;
            customerOrderService = _customerOrderService;
            orderDetailsService = _orderDetailsService;
        }

        [Route("", Name = "Index")]
        public IActionResult Index()
        {
            //var products = itemService.ViewItemListFilterCategory("all", "all", 10, 1); //tests
            //return View(products);
			var cart = GetCartItems();
			return View(cart);
        }

        /// Thêm sản phẩm vào cart
        [Route ("/addcart", Name = "addcart")]
        public IActionResult AddToCart([FromForm] string itemID, [FromForm] int quantity)
        {
            var product = itemService.GetDetailOfItem(itemID);
            if (product == null)
                return NotFound("Không có sản phẩm");
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.item.itemID == itemID);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity =quantity;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartObj() { quantity = quantity, item = product });
            }
            // Lưu cart vào Session
            SaveCartSession(cart);
            // Chuyển đến trang hiện thị Cart
            return RedirectToAction(nameof(Index));
        }

        // xóa item trong cart
        [Route("/removecart/{itemID}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] string itemID)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.item.itemID == itemID);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }
            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        /// Cập nhật
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] string itemID, [FromForm] int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.item.itemID == itemID);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity = quantity;
            }
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }


        // Hiện thị giỏ hàng
        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            var cart = GetCartItems();/*
            var product = itemService.GetDetailOfItem("183b8d8fb523");
            var cartitem = cart.Find(p => p.item.itemID == "183b8d8fb523");
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartObj() { quantity = 1, item = product });
            }*/
            return View(cart);
        }

        [Route("/checkout")]
        public IActionResult CheckOut([FromForm] string address, [FromForm] string note, [FromForm] double total)
        {
            var session = HttpContext.Session;
            //string userID = session.GetString("USERID");
            string userID = "487iuewur398";
            double deliveryFee = 20000;
            // Xử lý khi đặt hàng
            var cart = GetCartItems();
            ViewData["address"] = address;
            ViewData["cart"] = cart;

            if (!string.IsNullOrEmpty(userID))
            {
                //tạo cấu trúc db lưu lại đơn hàng
                string orderID = customerOrderService.AddCustomerOrder(userID, "abc", deliveryFee, "cyz", 202000);
                foreach (var cartObject in cart)
                {
                    orderDetailsService.AddOrderDetail(orderID, cartObject.item.itemID, cartObject.quantity);
                }
                //xóa cart khỏi session
                ClearCart();
                RedirectToAction(nameof(Index));
            }
            
            return View();
        }

        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        // Lấy cart từ Session (danh sách CartItem)
        List<CartObj> GetCartItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartObj>>(jsoncart);
            }
            return new List<CartObj>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartObj) vào session
        void SaveCartSession(List<CartObj> list)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(list);
            session.SetString(CARTKEY, jsoncart);
        }
    }
}
