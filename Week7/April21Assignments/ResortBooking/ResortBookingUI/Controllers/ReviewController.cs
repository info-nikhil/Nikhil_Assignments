using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ResortBookingUI.MVC.Models;

public class ReviewController : Controller
{
    private readonly ApiService _api;

    public ReviewController(ApiService api)
    {
        _api = api;
    }

    // ---------------- INDEX ----------------
    public async Task<IActionResult> Index()
    {
        var data = await _api.GetAsync<List<Review>>("api/review");
        return View(data);
    }

    // ---------------- CREATE ----------------
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Review review)
    {
        await _api.PostAsync("api/review", review);
        return RedirectToAction("Index");
    }
}