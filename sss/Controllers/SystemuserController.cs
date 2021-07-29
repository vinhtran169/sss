using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sss.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace sss.Controllers
{
	public class SystemuserController : Controller
	{
<<<<<<< HEAD
		
		public IActionResult Login()
		{
			return PartialView("_LoginForm");
		}

=======
>>>>>>> master

        public IActionResult Login()
        {
            return PartialView("_LoginForm");
        }

        [HttpPost]
        public IActionResult Login(IFormCollection form)
        {
            string username = form["username"];
            string password = form["password"];
            password = BitConverter.ToString(MD5.Create().ComputeHash(System.Text.Encoding.ASCII.GetBytes(password))).Replace("-", "");


            using (sssContext dbContext = new sssContext())
            {
                bool existUser = dbContext.Systemusers.Any(user => user.Username == username && user.Password == password);

                if (existUser)
                {
                    HttpContext.Session.SetString("username", username);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "False Login";
                    return PartialView("_LoginForm");
                }
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Systemuser");
        }
    }
}
