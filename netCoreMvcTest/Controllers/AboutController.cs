using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace netCoreMvcTest.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TellMeMore(string id = "")
        {
            return View();
            return new JsonResult(new { name = "Tell me more", content = id });
            return Content($"ur content is {id}");
        }
        
    }
}