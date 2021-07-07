using FoodOrderingSystem.Models.account;
using FoodOrderingSystem.Models.item;
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
            public string UserID { get; set; }
            public string Note { get; set; }
            public string CategoryName { get; set; }
            public string Status { get; set; }
            public string ItemID { get; set; }
            public string SearchValue { get; set; }
        }

        [HttpPost]
        [Route("ViewStaffsList")]
        public JsonResult ViewStaffsList([FromServices] IAccountService accountService, [FromBody] Request request)
        {
            try
            {
                IEnumerable<Account> accounts = accountService.ViewStaffsList(request.RowsOnPage, request.RequestPage);
                int count = accountService.NumberOfStaffs();
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

        [Route("InactiveAccount")]
        public JsonResult InactiveAccount([FromServices] IAccountService accountService, [FromForm] Request request)
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
        public JsonResult ActiveAccount([FromServices] IAccountService accountService, [FromForm] Request request)
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
        public JsonResult ReasonAccountInactived([FromServices] IAccountService accountService, [FromBody] Request request)
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
        public JsonResult ViewItemList([FromServices] IItemService itemService, [FromBody] Request request)
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
                bool result = itemService.CreateItem(item.itemName, item.categoryName, item.unitPrice, item.availableQuantity, item.foodCoin);
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
                bool result = itemService.UpdateItemInformation(item.itemID, item.itemName, item.categoryName, item.unitPrice, item.availableQuantity, item.foodCoin);
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
        public JsonResult InactiveItem([FromServices] IItemService itemService, [FromForm] Request request)
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
        public JsonResult ActiveItem([FromServices] IItemService itemService, [FromForm] Request request)
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
        public JsonResult ReasonItemInactived([FromServices] IItemService itemService, [FromBody] Request request)
        {
            try
            {
                string note = itemService.GetDetailOfItem(request.ItemID).note;
                if (note.Trim().Length == 0 || note == null) return new JsonResult(new
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

        //Search
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
                _logger.LogInformation("Get Items List: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

    }
}
