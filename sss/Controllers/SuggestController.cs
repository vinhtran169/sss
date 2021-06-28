using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sss.Models;
namespace sss.Controllers
{
    public class SuggestController : Controller
    {
        [HttpGet]
        [Route("Home/suggest/create")]
        public IActionResult Create()
        {
            HttpContext.Session.SetString("username","admin"); // create a session demo

            string username = HttpContext.Session.GetString("username");
            if(username != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        [Route("Home/suggest/create")]
        public IActionResult Create(Suggestion suggestion)
        {

            if (suggestion.Title != null && suggestion.Description!= null && HttpContext.Session.GetString("username")!= null)
            {
                using (sssContext dbContext = new sssContext())
                {
                    var user = dbContext.Systemusers.Where(a => a.Username == HttpContext.Session.GetString("username")).FirstOrDefault(); // get user
                    var router = dbContext.Systemusers.Where(b => b.Role == "Router").FirstOrDefault(); //get router suitable

                    suggestion.CreatedDate = DateTime.Now;
                    suggestion.UpdatedDate = DateTime.Now;
                    suggestion.ImplementDate = DateTime.Now;
                    suggestion.Creator = HttpContext.Session.GetString("username");
                    suggestion.Userid = router.Userid;

                    dbContext.Suggestions.Add(suggestion);
                    dbContext.SaveChanges();
                }
                Response.Redirect("list");
            }

            return View();
        }


    }
}
