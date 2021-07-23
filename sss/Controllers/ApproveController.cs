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

        [HttpGet]
        [Route("home/approve/list")]
        public IActionResult List(int page = 1, string orderby = "topic", bool dsc = true)
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Systemuser");
            }

            using (sssContext dbContext = new sssContext())
            {
                var user = dbContext.Systemusers.Where(a => a.Username == currentUser).FirstOrDefault(); // get user with session value

                if (user.Role.Trim() == "Suggestor")
                {
                    return View("~/Views/Shared/NotFound.cshtml");
                }

                var suggestions = dbContext.Suggestions.Where(s => s.Userid == user.Userid && s.StatusType == null).ToList(); // get list suggestion suitable
                var model = Paging(suggestions, page, orderby.Trim(), dsc);

                ViewData["Pages"] = model.pages;
                ViewData["Page"] = model.page;
                ViewData[orderby.Trim()] = !dsc;
                ViewBag.Current = orderby.Trim();

                if (page > model.pages && model.pages != 0)
                {
                    return View("~/Views/Shared/NotFound.cshtml"); // Not Found
                }

                return View(model.suggestions);
            }
        }

        public static (List<Suggestion> suggestions, int pages, int page) Paging(List<Suggestion> Suggestions, int page, string orderby= "topic", bool dsc = true)
        {
            int size = 10;
            int pages = (int)Math.Ceiling((double)Suggestions.Count / size);
            var suggestions = Suggestions.Skip((page - 1) * size).Take(size).ToList();

            if (dsc)
            {
                switch (orderby)
                {
                    case "topic": suggestions = suggestions.OrderBy(s => s.Title).ToList(); break;
                    case "description": suggestions = suggestions.OrderBy(s => s.Description).ToList(); break;
                    case "implementdate": suggestions = suggestions.OrderBy(s => s.ImplementDate).ToList(); break;
                    case "creator": suggestions = suggestions.OrderBy(s => s.Creator).ToList(); break;
                    case "createddate": suggestions = suggestions.OrderBy(s => s.CreatedDate).ToList(); break;
                    case "updateddate": suggestions = suggestions.OrderBy(s => s.UpdatedDate.ToString()).ToList(); break;
                }
            }
            else
            {
                switch (orderby)
                {
                    case "topic": suggestions = suggestions.OrderByDescending(s => s.Title).ToList(); break;
                    case "description": suggestions = suggestions.OrderByDescending(s => s.Description).ToList(); break;
                    case "implementdate": suggestions = suggestions.OrderByDescending(s => s.ImplementDate).ToList(); break;
                    case "creator": suggestions = suggestions.OrderByDescending(s => s.Creator).ToList(); break;
                    case "createddate": suggestions = suggestions.OrderByDescending(s => s.CreatedDate).ToList(); break;
                    case "updateddate": suggestions = suggestions.OrderByDescending(s => s.UpdatedDate.ToString()).ToList(); break;
                }
            }

            return (suggestions, pages, page);
        }

        [HttpGet]
        [Route("home/approve/search")]
        public IActionResult Search(string term)
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Systemuser");
            }

            if (term == null || term == "")
            {
                return RedirectToAction("List");
            }

            using (sssContext dbContext = new sssContext())
            {
                var user = dbContext.Systemusers.Where(a => a.Username == currentUser).FirstOrDefault(); // get user with session value

                if (user.Role.Trim() == "Suggestor")
                {
                    return View("~/Views/Shared/NotFound.cshtml");
                }

                string search = term.ToLower();

                var suggestions = dbContext.Suggestions.Where(s =>
                   s.Title.ToLower().Contains(search) ||
                   s.Description.ToLower().Contains(search) ||
                   s.Creator.ToLower().Contains(search) ||
                   s.CreatedDate.ToString().ToLower().Contains(search) ||
                   s.UpdatedDate.ToString().ToLower().Contains(search) ||
                   s.ImplementDate.ToString().ToLower().Contains(search)).Where(s => s.Userid == user.Userid && s.StatusType == null).ToList();

                return View("List", suggestions);
            }
        }

        [HttpGet]
        [Route("home/approve/actions")]
        public IActionResult Actions(int id)
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Systemuser");
            }

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
            currentUser = HttpContext.Session.GetString("username"); // Get session value

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
            currentUser = HttpContext.Session.GetString("username"); // Get session value

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
