using Microsoft.AspNetCore.Mvc;
using ResortBookingUI.MVC.Models;

public class ResortController : Controller
{
    private readonly ApiService _api;

    public ResortController(ApiService api)
    {
        _api = api;
    }

    // ---------------- INDEX ----------------
    public async Task<IActionResult> Index()
    {
        var data = await _api.GetAsync<List<Resort>>("api/resort");
        return View(data);
    }

    // ---------------- CREATE ----------------
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Resort resort)
    {
        await _api.PostAsync("api/resort", resort);
        return RedirectToAction("Index");
    }

    // ---------------- EDIT ----------------
    public async Task<IActionResult> Edit(int id)
    {
        var resort = await _api.GetAsync<Resort>($"api/resort/{id}");
        return View(resort);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Resort resort)
    {
        await _api.PutAsync($"api/resort/{id}", resort);
        return RedirectToAction("Index");
    }

    // ---------------- DELETE ----------------
    public async Task<IActionResult> Delete(int id)
    {
        await _api.DeleteAsync($"api/resort/{id}");
        return RedirectToAction("Index");
    }
    // ---------------- DETAILS ----------------
    public async Task<IActionResult> Details(int id)
    {
        var resort = await _api.GetAsync<Resort>($"api/resort/{id}");
        return View(resort);
    }
}