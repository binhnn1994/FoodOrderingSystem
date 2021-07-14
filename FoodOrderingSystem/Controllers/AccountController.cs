using FoodOrderingSystem.Models.account;
using FoodOrderingSystem.Services.Implements;
using FoodOrderingSystem.Services.Interfaces;
using FoodOrderingSystem.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Controllers
{
    public class AccountController : Controller
    {

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromServices] IAccountService accountService, [FromBody] LoginForm form)
        {
            if(form.Email == null || form.Email.Trim().Length == 0 || form.Password == null || form.Password.Length == 0)
            {
                ViewBag.Message = "Please input Email and Password !";
                return RedirectToAction("Home/Index/");
            }
            var user = accountService.Login(form.Email, form.Password);
            if(user == null) ViewBag.Message = "Incorrect Email and Password !";
            return RedirectToAction("Home/Index/");
        }

        public class LoginForm
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
