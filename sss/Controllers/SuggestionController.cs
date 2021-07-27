using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;
using sss.Models;
using Microsoft.EntityFrameworkCore;

namespace sss.Controllers
{
    /// <summary>
    /// Provides functionality to the /home/suggest.
    /// </summary>
    public class SuggestionController : Controller
    {
        string currentUser = string.Empty; 

        public SuggestionController()
        {
            currentUser = "admin"; // Get username from session value
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("home/suggest/create")]
        public IActionResult Create()
        {
            // Check is session exist
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
            // Validate suggestion
            if (!ValidateSuguestion(suggestion))
            {
                return View(suggestion);
            }

            using (sssContext dbContext = new sssContext())
            {
                var user = dbContext.Systemusers.Where(a => a.Username == currentUser).FirstOrDefault(); // get user with session value
                var router = dbContext.Systemusers.Where(b => b.Role == "Router").FirstOrDefault(); // get router from list user

                // Assign router for sugguestion
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

                Response.Redirect("list"); // Return path "home/suggest/list"
            }

            return View();
        }

        // This function validate suguestion and return a bool value
        private bool ValidateSuguestion(Suggestion suggestion)
        {
            bool check_valid = true;

            string suggest_title = suggestion.Title;
            string suggest_description = suggestion.Description;

            using (sssContext dbContext = new sssContext())
            {
                // Get value from database that same input suguest
                var title = dbContext.Suggestions.Where(a => a.Title == suggestion.Title).FirstOrDefault();
                var description = dbContext.Suggestions.Where(b => b.Description == suggestion.Description).FirstOrDefault();

                // Validate title title field
                if(suggest_title == null)
                {
                    ModelState.AddModelError("Title", "Please fill title for this suggestion");
                    check_valid = false;
                }
                else if(suggest_title.Trim().Length < 10)
                {
                    ModelState.AddModelError("Title", "Please lengthen title to 10 characters or more");
                    check_valid = false;
                }
                else if(title != null)
                {
                    ModelState.AddModelError("Title", "This title suggestion is exist");
                    check_valid = false;
                }

                // Validate description field
                if (suggest_description == null)
                {
                    ModelState.AddModelError("Description", "Please fill description for this suggestion");
                    check_valid = false;
                }
                else if (suggest_description.Trim().Length < 10)
                {
                    ModelState.AddModelError("Description", "Please lengthen description to 10 characters or more");
                    check_valid = false;
                }
                else if (description != null)
                {
                    ModelState.AddModelError("Description", "This description suggestion is exist");
                    check_valid = false;
                }

                // Validate ImplementDate field
                if(suggestion.ImplementDate == null)
                {
                    ModelState.AddModelError("ImplementDate", "Please fill implement date for this suggestion");
                    check_valid = false;
                }
                else if(DateTime.Compare(DateTime.Now, (DateTime)suggestion.ImplementDate) >= 0)
                {
                    ModelState.AddModelError("ImplementDate", "Please fill in a date in the future");
                    check_valid = false;
                }
            }

            return check_valid;
        }
        
        [Route("home/suggest/list")]
        public IActionResult List(string sortOrder, string searchString, string currentFilter, int? page)
        {
            HttpContext.Session.SetString("username", "admin"); //temp session
            string username = HttpContext.Session.GetString("username");

            if (username == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            using (sssContext dbContext = new sssContext())
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.TitleSort = "title";
                ViewBag.DescriptionSort = "description";
                ViewBag.CreatorSort = "creator";
                ViewBag.ImplementSort = "implement";
                ViewBag.CreatedSort = "created";
                ViewBag.UpdatedSort = "updated";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;

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
                    case "description":
                        listSuggest = listSuggest.OrderBy(s => s.Description);
                        break;
                    case "creator":
                        listSuggest = listSuggest.OrderBy(s => s.Creator);
                        break;
                    case "implement":
                        listSuggest = listSuggest.OrderByDescending(s => s.ImplementDate);
                        break;
                    case "created":
                        listSuggest = listSuggest.OrderByDescending(s => s.CreatedDate);
                        break;
                    default:
                        listSuggest = listSuggest.OrderByDescending(s => s.UpdatedDate);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(listSuggest.ToPagedList(pageNumber, pageSize));
            }
        }
        //[Route("home/suggest/views/id")]
        public IActionResult Details(int? id)
		{
            using (sssContext dbContext = new sssContext())
			{
                Suggestion details = dbContext.Suggestions.Find(id);
                return View(details);
			}
                
		}
       // [Route("home/suggest/edit/id")]
        public IActionResult Edit(int? id)
		{

            using (sssContext dbContext = new sssContext())
            {
                Suggestion edit = dbContext.Suggestions.Find(id);
                return View(edit);
            }
        }

        [HttpPost]
        public IActionResult Edit(Suggestion suggestion)
		{
            sssContext dbContext = new sssContext();

            if (ModelState.IsValid)
			{
                    dbContext.Entry(suggestion).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return RedirectToAction("List");

            }
            return View(suggestion);
		}
    }
}