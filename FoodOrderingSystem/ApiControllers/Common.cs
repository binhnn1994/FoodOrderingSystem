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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace FoodOrderingSystem.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Common : ControllerBase
    {
        private readonly ILogger<Common> _logger;

        public Common(ILogger<Common> logger)
        {
            _logger = logger;
        }

        public partial class LoginCredential
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        private async Task<bool> SignIn(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.userID),
                new Claim(ClaimTypes.Role, account.roleName),
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return true;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromServices] IAccountService accountService, [FromBody] LoginCredential loginCredential)
        {
            try
            {
                string email = loginCredential.Email;
                string password = loginCredential.Password;
                var user = accountService.Login(email, password);

                if (user == null)
                {
                    return new JsonResult(new
                    {
                        role = "Unauthenticated"
                    });
                }

                System.Diagnostics.Debug.WriteLine("Role is: " + user.roleName);
                HttpContext.Session.SetString("USERID", user.userID);
                HttpContext.Session.SetString("FULLLNAME", user.fullname);
                HttpContext.Session.SetString("ROLENAME", user.roleName);

                var x = await SignIn(user);

                return new JsonResult(new
                {
                    role = user.roleName
                });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("CheckLogin: " + ex.Message);
                return new JsonResult(new
                {
                    message = ex.Message
                });
            }
        }

        [Route("Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
