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

        public IActionResult CreateSuggest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSuggest(Suggestion suggestion)
        {
            
            if(suggestion.Title != null && suggestion.Description != null)
            {
                suggestion.CreatedDate = DateTime.Now;
                using (sssContext dbContext = new sssContext())
                {
                    dbContext.Add(suggestion);
                    dbContext.SaveChanges();
                }
                return RedirectToAction("ListSuggest");
            }
            else
            {
                return RedirectToAction("CreateSuggest");
            }
            
        }

        public IActionResult DetailSuggest(int id)
        {
            using (sssContext dbContext = new sssContext())
            {
                var sg = dbContext.Suggestions.Where(a => a.Id == id).FirstOrDefault();
                if (sg != null)
                {
                    return View(sg);
                }
            }
            return RedirectToAction("ListSuggest");
        }


        public IActionResult EditSuggest(int id)
        {
            using (sssContext dbContext = new sssContext())
            {
                var sg = dbContext.Suggestions.Where(a => a.Id == id).FirstOrDefault();
                if(sg != null)
                {
                    return View(sg);
                }
            }
            return RedirectToAction("ListSuggest");
        }


        [HttpPost]
        public IActionResult EditSuggest(int id, Suggestion suggestion)
        {
            if(suggestion.Title != null && suggestion.Description != null)
            {
                using (sssContext dbContext = new sssContext())
                {
                    var sg = dbContext.Suggestions.Where(a => a.Id == id).FirstOrDefault();
                    sg.Title = suggestion.Title;
                    sg.Description = suggestion.Description;
                    sg.UpdatedDate = DateTime.Now;
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("ListSuggest");
        }

        [HttpPost]
        public IActionResult DeleteSuggest(int id)
        {
            using (sssContext dbContext = new sssContext())
            {
                var sg = dbContext.Suggestions.Where(a => a.Id == id).FirstOrDefault();
                dbContext.Remove(sg);
                dbContext.SaveChanges();
                return RedirectToAction("ListSuggest");
            }
        }

        public IActionResult ListSuggest()
        {
            using (sssContext dbContext = new sssContext())
            {
                var suggestList = dbContext.Suggestions.ToList();
                return View(suggestList);
            }
        }


        [HttpPost]
        public IActionResult CreateSuggest(IFormCollection forms)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
