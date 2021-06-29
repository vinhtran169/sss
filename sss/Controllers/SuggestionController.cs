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
            if (!ValidateSuguestion(suggestion))
            {
                return View(suggestion);
            }

            using (sssContext dbContext = new sssContext())
            {
                var user = dbContext.Systemusers.Where(a => a.Username == currentUser).FirstOrDefault(); // get user
                var router = dbContext.Systemusers.Where(b => b.Role == "Router").FirstOrDefault(); //get router

                if (router == null)
                {
                    suggestion.Userid = null;
                }
                else
                {
                    suggestion.Userid = router.Userid;
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

        private bool ValidateSuguestion(Suggestion suggestion)
        {
            bool check_valid = true;

            using (sssContext dbContext = new sssContext())
            {
                var title = dbContext.Suggestions.Where(a => a.Title == suggestion.Title).FirstOrDefault();
                var description = dbContext.Suggestions.Where(b => b.Description == suggestion.Description).FirstOrDefault();

                if(title != null)
                {
                    ModelState.AddModelError("Title", "This title suggestion is exist");
                    check_valid = false;
                }

                if(description != null)
                {
                    ModelState.AddModelError("Description", "This description suggestion is exist");
                    check_valid = false;
                }

                if(DateTime.Compare(DateTime.Now, (DateTime)suggestion.ImplementDate) >= 0)
                {
                    ModelState.AddModelError("ImplementDate", "Please fill in a date in the future");
                    check_valid = false;
                }
            }

            return check_valid;
        }
    }
}
