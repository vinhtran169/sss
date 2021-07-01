using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sss.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace sss.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public AccountController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
		public IActionResult Register()
        {
            return View();
        }


        public new bool IsPostBack
        {
            get
            {
                return
                Request.Form.Keys.Count > 0 && Request.Form.Keys.Count > 0 && HttpContext.Request.Method == "POST";
            }
        }

        [HttpPost]
        public IActionResult Login(IFormCollection forms)
        {
            string username = forms["username"];
            string password = forms["password"];

            using (sssContext dbContext = new sssContext())
            {
                bool existUser = dbContext.Accounts.Any(user => user.Username == username && user.Password == password);

                if (existUser)
                {
                    HttpContext.Session.SetString("username", username);
                    return Redirect("~/");
                }
                else
                {
                    ViewBag.error = "Invalid Account";
                    return View("Login");
                }
            }
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("login");
        }

        

        [Route("Account/Register")]
        [HttpPost]
        public IActionResult Register(IFormCollection forms)
        {
            string username = forms["username"];
            string password = forms["password"];

            using (sssContext dbContext = new sssContext())
            {
                Account objEmp = new Account();
                objEmp.Username = username;
                objEmp.Password = password;

                dbContext.Accounts.Add(objEmp);
                dbContext.SaveChanges();
                ViewData["success"] = "OK dang ki thanh cong";
            }
           
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
