using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sss.Models;
using Microsoft.AspNetCore.Http;
namespace sss.Controllers
{
    public class SuggestionController : Controller
    {
        string currentUser = string.Empty;

        public SuggestionController()
        {
            currentUser = "admin";
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("home/suggest/create")]
        public IActionResult Create()
        {
            if (currentUser != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [Route("home/suggest/create")]
        public IActionResult Create(Suggestion suggestion)
        {
            if (suggestion.Title == null || suggestion.Description == null || currentUser == null || ModelState.IsValid == false)
            {
                return View(suggestion);
            }
            using (sssContext dbContext = new sssContext())
            {
                var user = dbContext.Systemusers.Where(a => a.Username == currentUser).FirstOrDefault(); // get user
                var router = dbContext.Systemusers.Where(b => b.Role == "Router"); //get list router
                if (router.FirstOrDefault() == null)
                {
                    suggestion.Userid = null;
                }
                else
                {
                    var listRouter = router.ToList();
                    var random = new Random();
                    suggestion.Userid = listRouter[random.Next(listRouter.Count)].Userid; //get router random
                }
                suggestion.CreatedDate = DateTime.Now;
                suggestion.UpdatedDate = DateTime.Now;
                suggestion.Creator = currentUser;
                dbContext.Suggestions.Add(suggestion);
                dbContext.SaveChanges();
                Response.Redirect("list");
            }
            return View();
        }
    }
}
