using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sss.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace sss.Controllers
{
    /// <summary>
    /// Provides functionality to the /admin/account
    /// </summary>
    public class UserController : Controller
    {
        static string currentUser = string.Empty;

        public UserController()
        {
            //currentUser = "admin"; // Get username from session value
        }

        // Check login role function
        public static bool CheckLoginRole(sssContext context, string currentUser, string role)
        {
            var userLogin = context.Systemusers.Where(s => s.Username.Trim() == currentUser.Trim()).FirstOrDefault();

            if (userLogin != null && userLogin.Role.Trim() == role.Trim())
            {
                return true;
            }
            return false;
        }

        [HttpGet]
        [Route("admin/account/create")]
        public IActionResult Create()
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value
            // Check is session exist
            if (currentUser != null)
            {
                using (sssContext dbContext = new sssContext())
                {
                    if(CheckLoginRole(dbContext,currentUser, "Admin"))
                    {
                        return View();
                    }
                    return View("~/Views/Shared/NotFound.cshtml");
                }
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [Route("admin/account/create")]
        public IActionResult Create(Systemuser user)
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Validate user
            if (!ValidateUser(user, "create"))
            {
                return View(user);
            }

            using (sssContext dbContext = new sssContext())
            {
                // Check login with role admin
                if(!CheckLoginRole(dbContext, currentUser, "Admin"))
                {
                    return View("~/Views/Shared/NotFound.cshtml");
                }

                // generate create date and convert pass to md5
                user.CreatedDate = DateTime.Now;
                user.Password = BitConverter.ToString(MD5.Create().ComputeHash(System.Text.Encoding.ASCII.GetBytes(user.Password))).Replace("-", "");

                dbContext.Systemusers.Add(user);
                dbContext.SaveChanges();

                Response.Redirect("list"); // Return path "admin/account/list"
            }

            return View();
        }

        [HttpGet]
        [Route("admin/account/list")]
        public IActionResult List(int page = 1, string orderby = "userid", bool dsc = false)
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            using (sssContext dbContext = new sssContext())
            {
                // Check login with role admin
                if (!CheckLoginRole(dbContext, currentUser, "Admin"))
                {
                    return View("~/Views/Shared/NotFound.cshtml");
                }

                var systemusers = dbContext.Systemusers.ToList();
                var model = Paging(systemusers, page, orderby, dsc);

                ViewData["Pages"] = model.pages;
                ViewData["Page"] = model.page;
                ViewData[orderby] = !dsc;
                ViewBag.Current = orderby;

                if (page > model.pages)
                {
                    return View("~/Views/Shared/NotFound.cshtml"); // Not Found
                }

                return View(model.systemusers);
            }
        }

        public static (List<Systemuser> systemusers, int pages, int page) Paging(List<Systemuser> Systemusers, int page, string orderby = "userid", bool dsc = false)
        {
            int size = 10;
            int pages = (int)Math.Ceiling((double)Systemusers.Count / size);
            var systemusers = Systemusers.Skip((page - 1) * size).Take(size).ToList();

            if (dsc)
            {
                switch (orderby)
                {
                    case "userid": systemusers = systemusers.OrderBy(s => s.Userid).ToList(); break;
                    case "username": systemusers = systemusers.OrderBy(s => s.Username).ToList(); break;
                    case "role": systemusers = systemusers.OrderBy(s => s.Role).ToList(); break;
                    case "department": systemusers = systemusers.OrderBy(s => s.Department).ToList(); break;
                    case "email": systemusers = systemusers.OrderBy(s => s.Email).ToList(); break;
                    case "createddate": systemusers = systemusers.OrderBy(s => s.CreatedDate.ToString()).ToList(); break;
                }
            }
            else
            {
                switch (orderby)
                {
                    case "userid": systemusers = systemusers.OrderByDescending(s => s.Userid).ToList(); break;
                    case "username": systemusers = systemusers.OrderByDescending(s => s.Username).ToList(); break;
                    case "role": systemusers = systemusers.OrderByDescending(s => s.Role).ToList(); break;
                    case "department": systemusers = systemusers.OrderByDescending(s => s.Department).ToList(); break;
                    case "email": systemusers = systemusers.OrderByDescending(s => s.Email).ToList(); break;
                    case "createddate": systemusers = systemusers.OrderByDescending(s => s.CreatedDate.ToString()).ToList(); break;
                }
            }

            return (systemusers, pages, page);
        }

        [HttpPost]
        public IActionResult Delete(string userid)
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (userid == null)
            {
                return RedirectToAction("List");
            }

            using (sssContext dbContext = new sssContext())
            {
                // Check login with role admin
                if (!CheckLoginRole(dbContext, currentUser, "Admin"))
                {
                    return View("~/Views/Shared/NotFound.cshtml");
                }

                var systemuser = dbContext.Systemusers.Where(s => s.Userid.Trim() == userid.Trim()).FirstOrDefault();

                if(systemuser == null)
                {
                    Response.Redirect("list"); // Return path "admin/account/list"
                }

                dbContext.Remove(systemuser);
                dbContext.SaveChanges();

                return RedirectToAction("List");
            }
        }

        [HttpGet]
        [Route("admin/account/detail")]
        public IActionResult Detail(string userid)
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (userid == null)
            {
                return RedirectToAction("List");
            }

            using (sssContext dbContext = new sssContext())
            {
                // Check login with role admin
                if (!CheckLoginRole(dbContext, currentUser, "Admin"))
                {
                    return View("~/Views/Shared/NotFound.cshtml");
                }

                var user = dbContext.Systemusers.Where(s => s.Userid.Trim() == userid.Trim()).FirstOrDefault();

                if(user == null)
                {
                    return RedirectToAction("List");
                }

                return View(user);
            }
        }

        [HttpGet]
        [Route("admin/account/edit")]
        public IActionResult Edit(string userid)
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (userid == null)
            {
                return RedirectToAction("List");
            }

            using (sssContext dbContext = new sssContext())
            {
                // Check login with role admin
                if (!CheckLoginRole(dbContext, currentUser, "Admin"))
                {
                    return View("~/Views/Shared/NotFound.cshtml");
                }

                var user = dbContext.Systemusers.Where(s => s.Userid.Trim() == userid.Trim()).FirstOrDefault();

                if (user == null)
                {
                    return RedirectToAction("List");
                }

                return View(user);
            }
        }

        [HttpPost]
        [Route("admin/account/edit")]
        public IActionResult Edit(Systemuser systemuser)
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (systemuser == null)
            {
                return RedirectToAction("List");
            }

            // Validate user
            if (!ValidateUser(systemuser, "edit"))
            {
                return View(systemuser);
            }

            using (sssContext dbContext = new sssContext())
            {
                // Check login with role admin
                if (!CheckLoginRole(dbContext, currentUser, "Admin"))
                {
                    return View("~/Views/Shared/NotFound.cshtml");
                }

                dbContext.Systemusers.Update(systemuser);
                dbContext.SaveChanges();
                return RedirectToAction("List");
            }
        }

        [HttpGet]
        [Route("admin/account/search")]
        public IActionResult Search(string term)
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (term == null || term == "")
            {
                return RedirectToAction("List");
            }

            using (sssContext dbContext = new sssContext())
            {
                // Check login with role admin
                if (!CheckLoginRole(dbContext, currentUser, "Admin"))
                {
                    return View("~/Views/Shared/NotFound.cshtml");
                }

                string search = term.ToLower();

                var users = dbContext.Systemusers.Where(s =>
                   s.Userid.ToLower().Contains(search) ||
                   s.Username.ToLower().Contains(search) ||
                   s.Role.ToLower().Contains(search) ||
                   s.Department.ToLower().Contains(search) ||
                   s.Email.ToLower().Contains(search) ||
                   s.CreatedDate.ToString().ToLower().Contains(search)).ToList();

                return View("List", users);
            }
        }

        [HttpGet]
        [Route("admin/account/resetpassword")]
        public IActionResult ResetPassword(string userid)
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (userid == null)
            {
                return RedirectToAction("List");
            }

            using (sssContext dbContext = new sssContext())
            {
                // Check login with role admin
                if (!CheckLoginRole(dbContext, currentUser, "Admin"))
                {
                    return View("~/Views/Shared/NotFound.cshtml");
                }

                var systemuser = dbContext.Systemusers.Where(s => s.Userid.Trim() == userid.Trim()).FirstOrDefault();

                if (systemuser == null)
                {
                    Response.Redirect("list"); // Return path "admin/account/list"
                }

                systemuser.Password = "25d55ad283aa400af464c76d713c07ad"; // Hash md5 of default password "12345678"
                dbContext.SaveChanges();

                return View(systemuser);
            }
        }
        private bool ValidateUser(Systemuser systemuser, string action)
        {
            currentUser = HttpContext.Session.GetString("username"); // Get session value

            bool check_valid = true;
            bool check_action = true;

            if(action == "create")
            {
                check_action = false;
            }

            string systemuser_userid = systemuser.Userid;
            string systemuser_username = systemuser.Username;
            string systemuser_department = systemuser.Department;
            string systemuser_email = systemuser.Email;
            string systemuser_password = systemuser.Password;


            using (sssContext dbContext = new sssContext())
            {
                var context = dbContext.Systemusers.ToList();

                if (check_action)
                {
                    var user_update = dbContext.Systemusers.Where(a => a.Userid.Trim() == systemuser_userid).FirstOrDefault();
                    context.Remove(user_update);
                }

                // Get value from database that same input user
                var user = context.Where(a => a.Userid.Trim() == systemuser_userid).FirstOrDefault();
                var username = context.Where(b => b.Username.Trim() == systemuser_username).FirstOrDefault();
                var email = context.Where(b => b.Email.Trim() == systemuser_email).FirstOrDefault();

                // Validate userid field
                if (systemuser_userid == null)
                {
                    ModelState.AddModelError("Userid", "Please fill user id for this user");
                    check_valid = false;
                }
                else if (systemuser_userid.Trim().Length > 10 || systemuser_userid.Trim().Length < 2)
                {
                    ModelState.AddModelError("Userid", "Please lengthen user id to 10 characters and more than 2");
                    check_valid = false;
                }
                else if (user != null)
                {
                    ModelState.AddModelError("Userid", "This user is exist");
                    check_valid = false;
                }

                // Validate username field
                if (systemuser_username == null || systemuser_username=="")
                {
                    ModelState.AddModelError("Username", "Please fill username for this user");
                    check_valid = false;
                }
                else if (systemuser_username.Trim().Length > 10 || systemuser_username.Trim().Length < 2)
                {
                    ModelState.AddModelError("Username", "Please lengthen username to 10 characters and more than 2");
                    check_valid = false;
                }
                else if (username != null)
                {
                    ModelState.AddModelError("Username", "This user is exist");
                    check_valid = false;
                }

                // Validate department field
                if (systemuser_department == null)
                {
                    ModelState.AddModelError("Department", "Please fill department for this user");
                    check_valid = false;
                }
                else if (systemuser_department.Trim().Length > 10 && systemuser_department.Trim().Length < 2)
                {
                    ModelState.AddModelError("Department", "Please lengthen department to 10 characters and more than 2");
                    check_valid = false;
                }

                // Validate email field
                if (systemuser_email == null)
                {
                    ModelState.AddModelError("Email", "Please fill email for this user");
                    check_valid = false;
                }
                else if (systemuser_email.Trim().Length > 50)
                {
                    ModelState.AddModelError("Email", "Please fill email less than 50");
                    check_valid = false;
                }
                else if (email != null)
                {
                    ModelState.AddModelError("Email", "This email is exist");
                    check_valid = false;
                }

                // Validate password field
                if (systemuser_password == null)
                {
                    ModelState.AddModelError("Password", "Please fill password for this user");
                    check_valid = false;
                }
            }

            return check_valid;
        }
    }
}
