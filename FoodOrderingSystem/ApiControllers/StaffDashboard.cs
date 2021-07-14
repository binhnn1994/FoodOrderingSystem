using FoodOrderingSystem.Models.account;
using FoodOrderingSystem.Models.customerOrder;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffDashboard : ControllerBase
    {
        private readonly ILogger<StaffDashboard> _logger;

        public StaffDashboard(ILogger<StaffDashboard> logger)
        {
            _logger = logger;
        }
        
        public partial class StaffDashboardRequest
        {
            public string OrderID { get; set; }
            public string ConfirmButton { get; set; }
            public string FeedbackID { get; set; }
            public string CustomerEmail { get; set; }
            public string Content { get; set; }
            public string Status { get; set; }

        }


        [Route("ConfirmOrder")]
        public JsonResult ConfirmOrder([FromServices] ICustomerOrderService customerOrderService , [FromBody] StaffDashboardRequest request)
        {
            try
            {
                string status = null;
                if (request.ConfirmButton.ToLower().Equals("accept")) status = "Accepted";
                else if (request.ConfirmButton.ToLower().Equals("decline")) status = "Rejected";
                bool result = false;
                if(status != null) result = customerOrderService.ConfirmUpdate(request.OrderID, status);
                
                if (result) return new JsonResult(new { Message = "Confirm Successfully" });
                return new JsonResult(new { Message = "An error occured! Please try again !" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Active an account: " + ex.Message);
                return new JsonResult(new { Message = ex.Message});
            }
        }
        public class CustomerOrder_Account
        {
            public CustomerOrder CustomerOrder { get; set; }
            public Account Account { get; set; }
        }

        [Route("GetOrders/{Status}")]
        public JsonResult GetOrders([FromServices] ICustomerOrderService customerOrderService, [FromServices] IAccountService accountService, string Status)
        {
            try
            {
                var Orders = customerOrderService.GetCustomerOrdersByStatus(Status);
                if (Orders == null || Orders.Count == 0) return new JsonResult(new { Message = "There is no pending order at this time" });
                var customerOrder_Account = new List<CustomerOrder_Account>();
                foreach (var ord in Orders)
                {
                    var acc = accountService.GetDetailOfAccount(ord.CustomerID);
                    customerOrder_Account.Add(new CustomerOrder_Account
                    {
                        CustomerOrder = ord,
                        Account = acc
                    });
                }
                return new JsonResult(customerOrder_Account);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetPendingOrder: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("GetOrderByID")]
        public JsonResult GetOrderByID([FromServices] ICustomerOrderService customerOrderService, [FromBody] StaffDashboardRequest request)
        {
            try
            {
                if (request.OrderID == null) return new JsonResult(new { Message = "Invalid OrderID" });
                var pendingOrder = customerOrderService.GetCustomerOrderByID(request.OrderID);
                if (pendingOrder == null ) return new JsonResult(new { Message = "Invalid OrderID" });
                return new JsonResult(pendingOrder);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetPendingOrder: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("RespondFeedback")]
        [HttpPost]
        public async Task<IActionResult> RespondFeedback([FromServices] IFeedbackService feedbackService, [FromServices] ISendMailService sendMailService, [FromBody] StaffDashboardRequest request)
        {
            try
            {
                if(request.CustomerEmail != null && request.Content != null & request.Content.Trim().Length != 0)
                {
                    string body = "From " + request.CustomerEmail + "------" + "FeedbackID: " + request.FeedbackID + "\n" + request.Content;
                    await sendMailService.SendEmailAsync(request.CustomerEmail, "Replied to FeedbackID: ", body);

                    bool result = feedbackService.RespondFeedback(request.FeedbackID);
                    if (result) return new JsonResult(new { Message = "Forward successful" });
                }
                return new JsonResult(new { Message = "An Error Occurred" });
            }
            catch(Exception ex)
            {
                _logger.LogInformation("RespondFeedback: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }

        [Route("GetFeedbackByID")]
        public JsonResult GetFeedbackByID([FromServices] IFeedbackService feedbackService, [FromBody] StaffDashboardRequest request)
        {
            try
            {
                if (request.FeedbackID == null) return new JsonResult(new { Message = "Invalid FeedbackID" });
                var feedback = feedbackService.GetFeedback(request.FeedbackID);
                if (feedback == null) return new JsonResult(new { Message = "Invalid FeedbackID" });
                return new JsonResult(feedback);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetFeedbackByID: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }


        [Route("GetFeedbackByStatus")]
        public JsonResult GetFeedbackByStatus([FromServices] IFeedbackService feedbackService, [FromBody] StaffDashboardRequest request)
        {
            try
            {
                if (request.Status == null) return new JsonResult(new { Message = "Invalid status" });
                var feedbacks = feedbackService.GetFeedbackByStatus(request.Status);
                if (feedbacks == null) return new JsonResult(new { Message = "There is no feedback" });
                return new JsonResult(feedbacks);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetFeedbackByStatus: " + ex.Message);
                return new JsonResult(new { Message = ex.Message });
            }
        }
    }
}
