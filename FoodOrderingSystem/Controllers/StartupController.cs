using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN_GroceryStoreManagement.Controllers
{
    public class StartupController : Controller
    {
        public IActionResult Index()
        {
            string role = HttpContext.Session.GetString("ROLENAME");
            if (role == "Admin")
            {
                return Redirect("../AdminDashboard/Index");
            }
            else if (role == "Customer")
            {
                return Redirect("../CustomerDashboard/Index");
            } 
            else if (role == "Staff")
            {
                return Redirect("../StaffDashboard/Index");
            }
            else
            {
                return View("../Home/Index");
            }
        }
    }
}