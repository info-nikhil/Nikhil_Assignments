using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StateManagement.Models;

namespace StateManagement.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var cookieOptions = new CookieOptions
                //{
                //    Expires = DateTime.Now.AddMinutes(1)
                //};
                //Response.Cookies.Append("UserName", model.UserName, cookieOptions);

                HttpContext.Session.SetString("UserName", model.UserName);
                return RedirectToAction("Welcome");
            }
            return View(model);
        }

        public IActionResult Welcome()
        {
            var username = HttpContext.Session.GetString("UserName");
            //if (Request.Cookies.ContainsKey("UserName"))
            //{
            //    string username = Request.Cookies["UserName"];
            //    ViewBag.UserName = username;
            //}

            if (!string.IsNullOrEmpty(username))
            {
                ViewBag.UserName = username;
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            //Response.Cookies.Delete("UserName"); //For Cookies


            //HttpContext.Session.Remove("UserName");  //For Session


            HttpContext.Session.Clear();  //Added by me
            Response.Cookies.Delete(".AspNetCore.Session"); //Added by me



            return RedirectToAction("Login");
        }
    }
}
