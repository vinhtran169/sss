using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sss.Models;

namespace sss.Controllers
{
    public class SuggestionController : Controller
    {
        [Route("home/suggest/list")]
        public IActionResult ListSuggestion(string sortOrder, string searchString)
        {
            HttpContext.Session.SetString("username", "admin"); //temp session
            string username = HttpContext.Session.GetString("username");

            if (username != null)
                using (sssContext dbContext = new sssContext())
                {
                    ViewBag.TitleSort = "title";
                    ViewBag.CreatedSort = "created";
                    ViewBag.UpdatedSort = "updated";
                    var listSuggest = from s in dbContext.Suggestions select s;
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        listSuggest = listSuggest.Where(s => s.Title.Contains(searchString)
                                                             || s.Description.Contains(searchString)
                                                             || s.Creator.Contains(searchString));
                    }
                    switch (sortOrder)
                    {
                        case "title":
                            listSuggest = listSuggest.OrderBy(s => s.Title);
                            break;
                        case "created":
                            listSuggest = listSuggest.OrderBy(s => s.CreatedDate);
                            break;
                        case "updated":
                            listSuggest = listSuggest.OrderBy(s => s.UpdatedDate);
                            break;
                    }
                    return View(listSuggest.ToList());
                }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult EditSuggestion()
        {
            return View();
        }
        
        public IActionResult DetailsSuggestion()
        {
            return View();
        }
        
        public IActionResult DeleteSuggestion()
        {
            return View();
        }
    }
}