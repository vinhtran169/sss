using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sss.Models;

namespace sss.Controllers
{
    public class SuggestionController : Controller
    {
        [Route("home/suggest/list")]
        public IActionResult ListSuggestion()
        {
            HttpContext.Session.SetString("username", "admin"); //temp session
            string username = HttpContext.Session.GetString("username");

            if (username != null)
                using (sssContext dbContext = new sssContext())
                {
                    var listSuggest = dbContext.Suggestions.ToList();
                    return View(listSuggest);
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