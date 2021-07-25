using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sss.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sss.Controllers
{
	public class SystemuserController : Controller
	{
		
		public IActionResult Login()
		{
			return View();
		}



        [HttpPost]
        public IActionResult Login(IFormCollection form)
        {
            string username = form["username"];
            string password = form["password"];

            using (sssContext dbContext = new sssContext())
            {
                bool existUser = dbContext.Systemusers.Any(user => user.Username == username && user.Password == password);

                if (existUser)
                {
                    HttpContext.Session.SetString("username", username);
                    //return Redirect("~/");
                    TempData["username"] = username;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = existUser;
                    return View("Login");
                }
            }
        }
    }
}
