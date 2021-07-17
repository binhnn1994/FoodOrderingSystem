using FoodOrderingSystem.Models.account;
using FoodOrderingSystem.Models.Cart;
using FoodOrderingSystem.Models.item;
using FoodOrderingSystem.Services.Implements;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("/CustomerDashboard")]
    public class CustomerDashboardController : Controller
    {
        private readonly ILogger<CustomerDashboardController> _logger;

        private readonly IAccountService accountService;
        private readonly IItemService itemService;
        private readonly ICustomerOrderService customerOrderService;
        private readonly IOrderDetailsService orderDetailsService;

        public CustomerDashboardController(ILogger<CustomerDashboardController> logger, IItemService _itemService, ICustomerOrderService _customerOrderService,
            IOrderDetailsService _orderDetailsService, IAccountService _accountService)
        {
            _logger = logger;
            itemService = _itemService;
            customerOrderService = _customerOrderService;
            orderDetailsService = _orderDetailsService;
            accountService = _accountService;
        }

        [Route("Index", Name = "Index")]
        public IActionResult Index()
        {
            var session = HttpContext.Session;
            string userID = session.GetString("USERID");
            ViewBag.userID = userID;
            var cart = GetCartItems();
            return View(cart);
        }

        [Route("/addcart/{itemID}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute] string itemID)
        {
            var product = itemService.GetDetailOfItem(itemID);
            if (product == null)
                return NotFound("Không có sản phẩm");
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.item.itemID == itemID);
            if (cartitem != null)
            {
                cartitem.quantity++;
            }
            else
            {
                cart.Add(new CartObj() { quantity = 1, item = product });
            }
            SaveCartSession(cart);
            return RedirectToAction(nameof(Index));
        }

        [Route("/removecart/{itemID}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] string itemID)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.item.itemID == itemID);
            if (cartitem != null)
            {
                cart.Remove(cartitem);
            }
            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] string itemID, [FromForm] int quantity)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.item.itemID == itemID);
            if (cartitem != null)
            {
                cartitem.quantity = quantity;
            }
            SaveCartSession(cart);
            return Ok();
        }

        // show cart
        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            var cart = GetCartItems();
            return View(cart);
        }

        [Route("/checkout")]
        public IActionResult CheckOut()
        {
            var session = HttpContext.Session;
            string userID = session.GetString("USERID");
            string note = "Default";
            double deliveryFee;
            double total = 0;
            var cart = GetCartItems();
            Account user = accountService.GetDetailOfAccount(userID);
            System.Diagnostics.Debug.WriteLine("Trying to checkout");
            System.Diagnostics.Debug.WriteLine("User ID is " + userID);

            if (!string.IsNullOrEmpty(userID))
            {
                foreach (var cartObject in cart)
                {
                    total += cartObject.quantity * (double)cartObject.item.unitPrice;
                }

                if (total > 500000)
                {
                    deliveryFee = 5000;
                }
                else if (total > 300000)
                {
                    deliveryFee = 10000;
                }
                else
                {
                    deliveryFee = 20000;
                }

                string orderID = customerOrderService.AddCustomerOrder(userID, user.address, deliveryFee, note, total);
                foreach (var cartObject in cart)
                {
                    orderDetailsService.AddOrderDetail(orderID, cartObject.item.itemID, cartObject.quantity);
                }
                System.Diagnostics.Debug.WriteLine("Checkout successfully");
                //delete cart out session
                ClearCart();
                RedirectToAction(nameof(Index));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to checkout");
            }

            return RedirectToAction(nameof(Index));
        }

        // Key save string json of Cart
        public const string CARTKEY = "cart";

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

        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        void SaveCartSession(List<CartObj> list)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(list);
            session.SetString(CARTKEY, jsoncart);
        }

        [Route("Profile")]
        public IActionResult Profile()
        {
            var session = HttpContext.Session;
            string userID = session.GetString("USERID");
            ViewBag.userID = userID;
            return View();
        }
    }
}
