﻿using FoodOrderingSystem.Models.item;
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

    }
}
