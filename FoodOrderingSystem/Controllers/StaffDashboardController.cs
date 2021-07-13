using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
