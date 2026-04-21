using Microsoft.AspNetCore.Mvc;
using ResortBookingUI.MVC.Models;

public class BookingController : Controller
{
    private readonly ApiService _api;

    public BookingController(ApiService api)
    {
        _api = api;
    }

    // ---------------- INDEX ----------------
    public async Task<IActionResult> Index()
    {
        var role = HttpContext.Session.GetString("Role");

        if (role == "Admin")
        {
            var all = await _api.GetAsync<List<Booking>>("api/booking");
            return View(all);
        }
        else
        {
            var userId = HttpContext.Session.GetString("UserId");
            var mine = await _api.GetAsync<List<Booking>>($"api/booking/user/{userId}");
            return View(mine);
        }
    }

    // ---------------- CREATE ----------------
    public IActionResult Create(int? resortId)
    {
        var booking = new Booking();

        if (resortId.HasValue)
        {
            booking.ResortId = resortId.Value;
        }

        return View(booking);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Booking booking)
    {
        await _api.PostAsync("api/booking", booking);
        return RedirectToAction("Index");
    }

    // ---------------- DELETE ----------------
    public async Task<IActionResult> Delete(int id)
    {
        await _api.DeleteAsync($"api/booking/{id}");
        return RedirectToAction("Index");
    }

    // ---------------- UPDATE STATUS (ADMIN) ----------------
    [HttpGet]
    public async Task<IActionResult> UpdateStatus(int id)
    {
        var booking = await _api.GetAsync<Booking>($"api/booking/{id}");
        return View(booking);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        await _api.PutRawAsync($"api/booking/{id}", $"\"{status}\"");
        return RedirectToAction("Index");
    }
}