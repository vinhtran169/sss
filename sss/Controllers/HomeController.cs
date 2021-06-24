using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sss.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace sss.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult test()
        {
            return View();
        }
        
        //public IActionResult ListSuggest()
        //{
        //    using (sssContext dbContext = new sssContext())
        //    {
        //        var suggestList = dbContext.Suggestions.ToList();
        //        return View(suggestList);
        //    }
        //}

        //[HttpGet]
        //public IActionResult CreateSuggest()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult CreateSuggest(IFormCollection forms)
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
