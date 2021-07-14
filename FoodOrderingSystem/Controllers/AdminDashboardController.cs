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
    public class AdminDashboardController : Controller
    {
        /*private readonly HttpClient client = null;
        private string ApiUrl = "";
        public AdminDashboardController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUrl = "/api/AdminDashboard/ViewStaffsList";
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl);
            string stringData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Account> listAccount = JsonSerializer.Deserialize<List<Account>>(stringData, options);
            return View(listAccount);
        }*/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CustomerManagement()
        {
            return View();
        }

        public IActionResult StaffManagement()
        {
            return View();
        }

		public IActionResult Profile()
        {
            return View();
        }
    }
}
