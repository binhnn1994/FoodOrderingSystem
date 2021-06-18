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
    }
}
