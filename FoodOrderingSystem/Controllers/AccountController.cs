using FoodOrderingSystem.Models.account;
using FoodOrderingSystem.Services.Implements;
using FoodOrderingSystem.Services.Interfaces;
using FoodOrderingSystem.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login([FromServices] IAccountService accountService, string email, string password)
        {
            if (email == null || email.Trim().Length == 0 || password == null || password.Length == 0)
            {
                ViewBag.Message = "Please input Email and Password !";
                return RedirectToAction("Login", "Account");
            }
            var user = accountService.Login(email, password);
            if (user == null)
            {
                ViewBag.Message = "Incorrect Email and Password !";
                return RedirectToAction("Login", "Account");
            }
            HttpContext.Session.SetString("USERID", user.userID);
            HttpContext.Session.SetString("FULLLNAME", user.fullname);
            HttpContext.Session.SetString("USERROLE", user.roleName);

            if (user.roleName.Equals("Customer")) return RedirectToAction("Index", "Home");
            if (user.roleName.Equals("Admin")) return RedirectToAction("Index", "AdminDashboard");
            if (user.roleName.Equals("Staff")) return RedirectToAction("Index", "StaffDashboard");
            return NotFound();
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register([FromServices] IAccountService accountService, string email, string password, string confirmPassword, string fullname, string phoneNumber, string address)
        {
            if (email == null || email.Trim().Length == 0 || password == null || password.Length == 0 || confirmPassword == null || confirmPassword.Length == 0 ||  fullname == null || fullname.Length == 0 || phoneNumber == null || phoneNumber.Length == 0 || address == null || address.Length == 0)
            {
                ViewBag.Message = "Please fill all required field !";
                return RedirectToAction("Register", "Account");
            }
            bool result = accountService.Register(email, password, fullname, phoneNumber, address);
            if (result)
            {
                return RedirectToAction("Login", "Account");
            }
            return NotFound();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
