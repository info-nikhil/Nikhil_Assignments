using Microsoft.AspNetCore.Mvc;
using ResortBookingUI.MVC.Models;
using ResortBookingUI.MVC.Services;

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

    // ---------------- CREATE (GET) ----------------
    public async Task<IActionResult> Create(int? resortId)
    {
        // Load all resorts
        var resorts = await _api.GetAsync<List<Resort>>("api/resort");
        ViewBag.Resorts = resorts;

        var booking = new Booking();

        if (resortId.HasValue)
        {
            // From "Start Booking"
            booking.ResortId = resortId.Value;
            ViewBag.IsFromResort = true;
        }
        else
        {
            // From "New Booking"
            ViewBag.IsFromResort = false;
        }

        return View(booking);
    }

    // ---------------- CREATE (POST) ----------------
    [HttpPost]
    public async Task<IActionResult> Create(Booking booking)
    {
        try
        {
            // 🔥 Attach UserId from session
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not logged in");

            booking.UserId = Convert.ToInt32(userId);

            await _api.PostAsync("api/booking", booking);

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Reload dropdown (VERY IMPORTANT)
            var resorts = await _api.GetAsync<List<Resort>>("api/resort");
            ViewBag.Resorts = resorts;

            // Maintain UI state
            ViewBag.IsFromResort = booking.ResortId != 0;

            ViewBag.Error = ex.Message;
            return View(booking);
        }
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

    // ---------------- DETAILS ----------------
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var booking = await _api.GetAsync<Booking>($"api/booking/{id}");
            return View(booking);
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return RedirectToAction("Index");
        }
    }
}