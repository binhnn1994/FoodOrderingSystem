using FoodOrderingSystem.Models.account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Controllers
{
    public class StaffDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
