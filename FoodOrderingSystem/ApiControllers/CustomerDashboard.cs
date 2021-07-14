using FoodOrderingSystem.Models.item;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.ApiControllers
{
    [Route("api/[controller]")]
    public class CustomerDashboard : ControllerBase
    {
        private readonly ILogger<CustomerDashboard> _logger;

        public CustomerDashboard(ILogger<CustomerDashboard> logger)
        {
            _logger = logger;
        }

        public class Request
        {
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
            public string UserID { get; set; }
            public string Note { get; set; }
            public string CategoryName { get; set; }
            public string Status { get; set; }
            public string ItemID { get; set; }
            public string SearchValue { get; set; }
            public string OrderID { get; set; }
        }

        [HttpPost]
        [Route("ViewItemList")]
        public JsonResult ViewItemList([FromServices] IItemService itemService, [FromBody] Request request)
        {
            try
            {
                IEnumerable<Item> items = itemService.ViewItemListFilterCategory(request.CategoryName, "Active", request.RowsOnPage, request.RequestPage);
                int count = itemService.NumberOfItemFilterCategory(request.CategoryName, "Active");
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = items
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Get Items List: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        //Search Item
        [HttpPost]
        [Route("ViewItemListBySearching")]
        public JsonResult ViewItemListBySearching([FromServices] IItemService itemService, [FromBody] Request request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<Item> items = itemService.ViewItemListBySearching(request.SearchValue, request.CategoryName, "Active", request.RowsOnPage, request.RequestPage);
                int count = itemService.NumberOfItemBySearching(request.SearchValue, request.CategoryName, "Active");
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = items
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Search Items: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        public class AddFeedbackObj
        {
            public string CustomerEmail { get; set; }
            public string RequestContent { get; set; }
        }

        [HttpPost]
        [Route("AddFeedback")]
        public async Task<IActionResult> AddFeedback([FromServices] IFeedbackService feedbackService, [FromServices] ISendMailService sendMailService, [FromBody] AddFeedbackObj obj)
        {
            try
            {
                if (obj.CustomerEmail == null && obj.RequestContent == null) return (new JsonResult(new { Message = "fail" }));

                int addFeedbackID = feedbackService.AddFeedback(obj.CustomerEmail, obj.RequestContent);
                string body = "Thank you for your feedback. We will respond soon. \n" +
                    "Your feedback content: \n " +
                    "- Your email: " + obj.CustomerEmail + "\n" +
                    "- Feedback content: <'" + obj.RequestContent + "'>\n" +
                    "We will respond soon.\n" +
                    "Best Regard,";
                await sendMailService.SendEmailAsync(obj.CustomerEmail, "Confirm Received Feedback", body);

                bool result = feedbackService.RespondFeedback(addFeedbackID.ToString());
                if (result) return new JsonResult(new { Message = "success" });
                else return (new JsonResult(new { Message = "fail" }));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Add Feedback: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        [Route("GetOrderListByID/{customerID}")]
        public JsonResult GetOrderListByID([FromServices] ICustomerOrderService customerOrderService, string customerID)
        {
            try
            {
                var orders = customerOrderService.GetOrderListByID(customerID);
                if (orders == null || orders.Count == 0) return new JsonResult(new { Message = "There is no order" });
                return new JsonResult(orders);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetOrderListByID: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }
        [Route("GetOrderDetailsByOrderID")]
        public JsonResult GetOrderDetailsByOrderID([FromServices] IOrderDetailsService orderDetailsService, Request request)
        {
            try
            {
                if (request.OrderID == null) return new JsonResult(new { Message = "Invalid OrderID" });
                var result = orderDetailsService.GetOrderDetails(request.OrderID);
                if (result == null) return new JsonResult(new { Message = "Invalid OrderID" });
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetOrderDetailsByOrderID: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

    }
}