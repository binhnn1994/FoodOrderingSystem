using FoodOrderingSystem.Models.account;
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
    public class AdminDashboard : ControllerBase
    {
        private readonly ILogger<AdminDashboard> _logger;

        public AdminDashboard(ILogger<AdminDashboard> logger)
        {
            _logger = logger;
        }

        public class Request
        {
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
        }

        [HttpPost]
        [Route("ViewStaffsList")]
        public JsonResult ViewStaffsList([FromServices] IAccountService accountService,
                                [FromBody] Request request)
        {
            try
            {
                IEnumerable<Account> accounts = accountService.ViewStaffsList(request.RowsOnPage, request.RequestPage);
                int count = accountService.NumberOfStaffs();
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountsList: " + ex.Message);
                return new JsonResult("Error occur");
            }
        }

        [Route("CreateStaff")]
        public IActionResult CreateStaff([FromServices] IAccountService accountService, [FromForm] Account account)
        {
            try
            {
                bool result = accountService.CreateStaff(account.userEmail, account.hashedPassword, account.fullname, account.phoneNumber, account.dateOfBirth, account.address);
                var message = new
                {
                    message = "success"
                };
                return new JsonResult(message);
            }
            catch (Exception e)
            {
                _logger.LogInformation("CreateStaff: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("UpdateStaffInformation")]
        public IActionResult UpdateStaffInformation([FromServices] IAccountService accountService, [FromForm] Account account)
        {
            try
            {
                bool result = accountService.UpdateStaffInformation(account.userID, account.fullname, account.phoneNumber, account.dateOfBirth, account.address);
                if (result) {
                    var message = new
                    {
                        message = "success"
                    };
                    return new JsonResult(message);
                } else
                {
                    var message = new
                    {
                        message = "fail"
                    };
                    return new JsonResult(message);
                }
                    

            }
            catch (Exception e)
            {
                _logger.LogInformation("UpdateStaffInformation: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }
    }
}
