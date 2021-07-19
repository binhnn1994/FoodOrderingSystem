using FoodOrderingSystem.Models.account;
using FoodOrderingSystem.Models.category;
using FoodOrderingSystem.Models.item;
using FoodOrderingSystem.Models.saleReport;
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

        public class AdminDashboardRequest
        {
            public int RowsOnPage { get; set; }
            public int RequestPage { get; set; }
            public string UserID { get; set; }
            public string Note { get; set; }
            public string CategoryName { get; set; }
            public string Status { get; set; }
            public string ItemID { get; set; }
            public string SearchValue { get; set; }
        }

        [HttpPost]
        [Route("ViewStaffsList")]
        public JsonResult ViewStaffsList([FromServices] IAccountService accountService, [FromBody] AdminDashboardRequest request)
        {
            try
            {
                IEnumerable<Account> accounts = accountService.ViewAccountListByRole("Staff", request.RowsOnPage, request.RequestPage);
                int count = accountService.NumberOfAccountByRole("Staff");
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountsList: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("ViewCustumerList")]
        public JsonResult ViewCustumerList([FromServices] IAccountService accountService, [FromBody] AdminDashboardRequest request)
        {
            try
            {
                IEnumerable<Account> accounts = accountService.ViewAccountListByRole("Customer", request.RowsOnPage, request.RequestPage);
                int count = accountService.NumberOfAccountByRole("Customer");
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAccountsList: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        [Route("CreateStaff")]
        public async Task<IActionResult> CreateStaff([FromServices] IAccountService accountService, [FromServices] ISendMailService sendMailService, [FromForm] Account account)
        {
            try
            {
                bool result = accountService.CreateStaff(account.userEmail, account.hashedPassword, account.fullname, account.phoneNumber, account.address);
                string body = "Dear, " + account.fullname + "\n" + "\nYour password is: " + account.hashedPassword + "\n" +
                    "Thanks.";
                await sendMailService.SendEmailAsync(account.userEmail, "Create New Account", body);
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
                bool result = accountService.UpdateStaffInformation(account.userID, account.fullname, account.phoneNumber, account.address);
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

        [Route("InactiveAccount")]
        public JsonResult InactiveAccount([FromServices] IAccountService accountService, [FromForm] AdminDashboardRequest request)
        {
            try
            {
                bool result = accountService.InactiveAccount(request.UserID, request.Note);
                var message1 = new
                {
                    message = "success"
                };
                var message2 = new
                {
                    message = "fail"
                };
                if (result) return new JsonResult(message1); else return new JsonResult(message2);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Inactive an account: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("ActiveAccount")]
        public JsonResult ActiveAccount([FromServices] IAccountService accountService, [FromForm] AdminDashboardRequest request)
        {
            try
            {
                bool result = accountService.ActiveAccount(request.UserID);
                var message1 = new
                {
                    message = "success"
                };
                var message2 = new
                {
                    message = "fail"
                };
                if (result) return new JsonResult(message1); else return new JsonResult(message2);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Active an account: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("ReasonAccountInactived")]
        public JsonResult ReasonAccountInactived([FromServices] IAccountService accountService, [FromBody] AdminDashboardRequest request)
        {
            try
            {
                string note = accountService.GetDetailOfAccount(request.UserID).note;
                if (note.Trim().Length == 0 || note == null) return new JsonResult(new {
                    message = "fail"
                });
                var message = new
                {
                    reason = note
                };
                return new JsonResult(message);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Reason Inactived an account: " + e.Message);
                var message = new
                {
                    message = "fail"
                };
                return new JsonResult(message);
            }
        }

        [HttpPost]
        [Route("ViewItemList")]
        public JsonResult ViewItemList([FromServices] IItemService itemService, [FromBody] AdminDashboardRequest request)
        {
            try
            {
                IEnumerable<Item> items = itemService.ViewItemListFilterCategory(request.CategoryName, request.Status, request.RowsOnPage, request.RequestPage);
                int count = itemService.NumberOfItemFilterCategory(request.CategoryName, request.Status);
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

        [Route("CreateItem")]
        public IActionResult CreateItem([FromServices] IItemService itemService, [FromForm] Item item)
        {
            try
            {
                bool result = itemService.CreateItem(item.itemName, item.categoryName, item.unitPrice, item.image, item.description);
                var message = new
                {
                    message = "success"
                };
                return new JsonResult(message);
            }
            catch (Exception e)
            {
                _logger.LogInformation("CreateItem: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("UpdateItemInformation")]
        public IActionResult UpdateItemInformation([FromServices] IItemService itemService, [FromForm] Item item)
        {
            try
            {
                bool result = itemService.UpdateItemInformation(item.itemID, item.itemName, item.categoryName, item.unitPrice, item.image, item.description);
                if (result)
                {
                    var message = new
                    {
                        message = "success"
                    };
                    return new JsonResult(message);
                }
                else
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
                _logger.LogInformation("Update Item Information: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("InactiveItem")]
        public JsonResult InactiveItem([FromServices] IItemService itemService, [FromForm] AdminDashboardRequest request)
        {
            try
            {
                bool result = itemService.InactiveItem(request.ItemID, request.Note);
                var message1 = new
                {
                    message = "success"
                };
                var message2 = new
                {
                    message = "fail"
                };
                if (result) return new JsonResult(message1); else return new JsonResult(message2);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Inactive an item: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("ActiveItem")]
        public JsonResult ActiveItem([FromServices] IItemService itemService, [FromForm] AdminDashboardRequest request)
        {
            try
            {
                bool result = itemService.ActiveItem(request.ItemID);
                var message1 = new
                {
                    message = "success"
                };
                var message2 = new
                {
                    message = "fail"
                };
                if (result) return new JsonResult(message1); else return new JsonResult(message2);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Active an item: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("ReasonItemInactived")]
        public JsonResult ReasonItemInactived([FromServices] IItemService itemService, [FromBody] AdminDashboardRequest request)
        {
            try
            {
                string note = itemService.GetDetailOfItem(request.ItemID).note;
                if (note == null || note.Trim().Length == 0) return new JsonResult(new
                {
                    message = "fail"
                });
                var message = new
                {
                    reason = note
                };
                return new JsonResult(message);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Reason Inactived an item: " + e.Message);
                var message = new
                {
                    message = "fail"
                };
                return new JsonResult(message);
            }
        }

        //Search Item
        [HttpPost]
        [Route("ViewItemListBySearching")]
        public JsonResult ViewItemListBySearching([FromServices] IItemService itemService, [FromBody] AdminDashboardRequest request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<Item> items = itemService.ViewItemListBySearching(request.SearchValue, request.CategoryName, request.Status, request.RowsOnPage, request.RequestPage);
                int count = itemService.NumberOfItemBySearching(request.SearchValue, request.CategoryName, request.Status);
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

        //Search customer//staff accounts.

        [HttpPost]
        [Route("ViewCustomerListBySearching")]
        public JsonResult ViewCustomerListBySearching([FromServices] IAccountService accountService, [FromBody] AdminDashboardRequest request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<Account> accounts = accountService.ViewAccountListBySearching(request.SearchValue, "Customer", request.RowsOnPage, request.RequestPage);
                int count = accountService.NumberOfAccountBySearching(request.SearchValue, "Customer");
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Search customer: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("ViewStaffListBySearching")]
        public JsonResult ViewStaffListBySearching([FromServices] IAccountService accountService, [FromBody] AdminDashboardRequest request)
        {
            try
            {
                var message = new
                {
                    message = "Empty searchValue"
                };
                if (request.SearchValue.Trim() == "") return new JsonResult(message);
                IEnumerable<Account> accounts = accountService.ViewAccountListBySearching(request.SearchValue, "Staff", request.RowsOnPage, request.RequestPage);
                int count = accountService.NumberOfAccountBySearching(request.SearchValue, "Staff");
                double totalPage = (double)count / (double)request.RowsOnPage;
                var result = new
                {
                    TotalPage = (int)Math.Ceiling(totalPage),
                    Data = accounts
                };
                return (new JsonResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Search staff: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        [Route("ViewAccountDetail")]
        public JsonResult ViewAccountDetail([FromServices] IAccountService accountService, [FromBody] AdminDashboardRequest request)
        {
            try
            {
                Account account = accountService.GetDetailOfAccount(request.UserID);
                return new JsonResult(account);
            }
            catch (Exception e)
            {
                _logger.LogInformation("ViewAccountDetail: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("ViewItemDetail")]
        public JsonResult ViewItemDetail([FromServices] IItemService itemService, [FromBody] AdminDashboardRequest request)
        {
            try
            {
                Item item = itemService.GetDetailOfItem(request.ItemID);
                return new JsonResult(item);
            }
            catch (Exception e)
            {
                _logger.LogInformation("ViewItemDetail: " + e.Message);
                var message = new
                {
                    message = e.Message
                };
                return new JsonResult(message);
            }
        }

        [Route("GetCategories")]
        public JsonResult GetCategories([FromServices] ICategoryService categoryService)
        {
            try
            {
                IEnumerable<Category> categories = categoryService.GetCategories();
                return new JsonResult(categories);
            }
            catch (Exception e)
            {
                _logger.LogInformation("GetCategories: " + e.Message);
                var message = new
                {
                    message = "error"
                };
                return new JsonResult(message);
            }
        }

        public class FromDateToDate
        {
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
        }
        [Route("SaleReport")]
        public JsonResult SaleReport([FromServices] ISaleReportService saleReportService,[FromBody] FromDateToDate dateObj)
        {
            try
            {
                if (dateObj.FromDate.ToString() == null || dateObj.ToDate.ToString() == null) return new JsonResult(new { message = "fail" });
                IList<SaleReportObj> saleReportObjList = saleReportService.ListSaleReport(dateObj.FromDate, dateObj.ToDate);
                if (saleReportObjList == null) return new JsonResult(new { Message = "There is no order during this time !" });
                return new JsonResult(saleReportObjList);
            }
            catch (Exception e)
            {
                _logger.LogInformation("GetCategories: " + e.Message);
                var message = new
                {
                    message = "error"
                };
                return new JsonResult(message);
            }
        }
    }
}
