﻿using FoodOrderingSystem.Models.Cart;
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

        public CustomerDashboardController(ILogger<CustomerDashboardController> logger, IItemService _itemService)
        {
            _logger = logger;
            itemService = _itemService;
        }

        [Route("", Name = "Index")]
        public IActionResult Index()
        {
            var products = itemService.ViewItemListFilterCategory("all", "all", 10, 1); //tests
            return View(products);
        }

        /// Thêm sản phẩm vào cart
        [Route ("addcart/{itemID}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute] string itemID)
        {
            var product = itemService.GetDetailOfItem(itemID);
            if (product == null)
                return NotFound("Không có sản phẩm");
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.item.itemID == itemID);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartObj() { quantity = 1, item = product });
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
        public IActionResult CheckOut([FromForm] string email, [FromForm] string address)
        {

            // Xử lý khi đặt hàng
            var cart = GetCartItems();
            ViewData["email"] = email;
            ViewData["address"] = address;
            ViewData["cart"] = cart;

            if (!string.IsNullOrEmpty(email))
            {
                //tạo cấu trúc db lưu lại đơn hàng
                //add customerOrder
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
