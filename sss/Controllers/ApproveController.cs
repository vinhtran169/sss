using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sss.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace sss.Controllers
{
    /// <summary>
    /// Provides functionality to the /home/approve.
    /// </summary>
    public class ApproveController : Controller
    {
        string currentUser = string.Empty;

        public ApproveController()
        {
            currentUser = "nguyenvana"; // Get username from session value
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]       
        [Route("home/approve/list")]
        public IActionResult List(string sortOrder, string searchString, string currentFilter, int? page)
        {
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            using (sssContext dbContext = new sssContext())
            {
                var user = dbContext.Systemusers.Where(a => a.Username == currentUser).FirstOrDefault(); // get user with session value

                if(user.Role.Trim() == "Suggestor")
                {
                    return View("~/Views/Shared/NotFound.cshtml");
                }

                ViewBag.CurrentSort = sortOrder;
                ViewBag.TitleSort = "title";
                ViewBag.DescriptionSort = "description";
                ViewBag.CreatorSort = "creator";
                ViewBag.ImplementSort = "implement";
                ViewBag.CreatedSort = "created";
                ViewBag.UpdatedSort = "updated";
                ViewBag.Role = user.Role.Trim();
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
                    listSuggest = listSuggest.Where(s => s.Title.Contains(searchString) || s.Description.Contains(searchString) || s.Creator.Contains(searchString));
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
                
                listSuggest = listSuggest.Where(s => s.Userid == user.Userid && s.StatusType == null);
     
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(listSuggest.ToPagedList(pageNumber, pageSize));
            }
        }

        [HttpGet]
        [Route("home/approve/actions")]
        public IActionResult Actions(int id)
        {
            using (sssContext dbContext = new sssContext())
            {
                var suggestion = dbContext.Suggestions.Where(a => a.Id == id).FirstOrDefault(); // get suggestion value
                var user = dbContext.Systemusers.Where(a => a.Username == currentUser).FirstOrDefault(); // get user with session value

                if (suggestion == null || suggestion.Userid != user.Userid || user.Role.Trim() == "Suggestor")
                {
                    return View("~/Views/Shared/NotFound.cshtml"); // Not Found
                }
                else
                {
                    if(user.Role.Trim() == "Router") // role ROUTER
                    {
                        var list_managers = dbContext.Systemusers.Where(a => a.Role == "Manager").ToList();

                        if(list_managers.Count == 0)
                        {
                            ModelState.AddModelError("Userid", "There is no manager data in the database");
                        }

                        ViewBag.Managers = list_managers;
                        ViewBag.Role = "Router";
                        return View(suggestion);
                    }
                    else // role MANAGER
                    {
                        ViewBag.Role = "Manager";
                        return View(suggestion);
                    }
                } 
            }
        }

        [HttpPost]
        [Route("home/approve/actions")]
        public IActionResult Actions(Suggestion suggestion)
        {
            using (sssContext dbContext = new sssContext())
            {
                var user = dbContext.Systemusers.Where(a => a.Username == currentUser).FirstOrDefault(); // get user with session value
                var suggestion_temp = dbContext.Suggestions.Where(a => a.Id == suggestion.Id).FirstOrDefault();

                ViewBag.Role = user.Role.Trim(); // Pass role user to View

                if (!ValidateSuguestion(suggestion, user.Role)){
                    return View(suggestion);
                }

                if (user.Role.Trim() == "Router") // role ROUTER
                {
                    suggestion_temp.Userid = suggestion.Userid.Trim();
                    dbContext.SaveChanges();
                    return RedirectToAction("List", "Approve");
                }
                else // role MANAGER
                {
                    var creator = dbContext.Systemusers.Where(a => a.Username == suggestion.Creator).FirstOrDefault(); // get creator

                    suggestion_temp.RewardMoney = suggestion.RewardMoney ?? null;
                    suggestion_temp.RemarkFromApprover = suggestion.RemarkFromApprover ?? null;
                    suggestion_temp.StatusType = suggestion.StatusType;
                    suggestion_temp.Userid = creator.Userid;
                    dbContext.SaveChanges();

                    return RedirectToAction("List", "Approve");
                }
            }
        }

        // Validate suggestion with role
        private bool ValidateSuguestion(Suggestion suggestion, string role)
        {
            bool check_valid = true;

            using (sssContext dbContext = new sssContext())
            {
                if (role.Trim() == "Manager")
                {
                    if(suggestion.RewardMoney < 0)
                    {
                        ModelState.AddModelError("RewardMoney", "Please fill reward money that greater or equal 0");
                        check_valid = false;
                    }
                }else if(role.Trim() == "Router")
                {
                    if(suggestion.Userid == null)
                    {
                        ModelState.AddModelError("Userid", "Please fill manager for this approve");
                        check_valid = false;
                    }
                }
            }

            return check_valid;
        }
    }
}
