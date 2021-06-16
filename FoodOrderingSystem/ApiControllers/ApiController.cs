using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.ApiControllers
{
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        public IActionResult Index()
        {
            return new JsonResult("");
        }
    }
}
