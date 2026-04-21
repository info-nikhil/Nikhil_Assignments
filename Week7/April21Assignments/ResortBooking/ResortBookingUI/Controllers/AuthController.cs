using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using ResortBookingUI.Models;
using ResortBookingUI.MVC.Models;
using System.Text.Json;


public class AuthController : Controller
{
    private readonly ApiService _api;

    public AuthController(ApiService api)
    {
        _api = api;
    }

    // ---------------- LOGIN ----------------
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        try
        {
            var response = await _api.PostWithResponseAsync<LoginResponse>(
                "api/login",
                new { email, password }
            );

            if (response == null || string.IsNullOrEmpty(response.Token))
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            // Store token + role + userId
            HttpContext.Session.SetString("JWT", response.Token);
            HttpContext.Session.SetString("Role", response.Role);
            HttpContext.Session.SetString("UserId", response.UserId.ToString());

            return RedirectToAction("Index", "Resort");
        }
        catch
        {
            ViewBag.Error = "Login failed";
            return View();
        }
    }

    // ---------------- REGISTER ----------------
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        await _api.PostAsync("api/register", user);
        return RedirectToAction("Login");
    }

    // ---------------- LOGOUT CONFIRM PAGE ----------------
    [HttpGet]
    public IActionResult LogoutConfirm()
    {
        return View();
    }

    // ---------------- LOGOUT ----------------
    [HttpPost]
    public IActionResult LogoutConfirmPost()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}