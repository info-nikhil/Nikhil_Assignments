using Microsoft.AspNetCore.Mvc;
using ResortBookingUI.MVC.Models;
using ResortBookingUI.MVC.Services;

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
        try
        {
            var role = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetString("UserId");

            // Admin → See all reviews
            if (role == "Admin")
            {
                var all = await _api.GetAsync<List<Review>>("api/review");
                return View(all);
            }

            // Customer → See only own reviews
            if (!string.IsNullOrEmpty(userId))
            {
                var mine = await _api.GetAsync<List<Review>>($"api/review/user/{userId}");
                return View(mine);
            }

            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(new List<Review>());
        }
    }

    // ---------------- CREATE (GET) ----------------
    public IActionResult Create(int? bookingId, int? resortId)
    {
        if (!bookingId.HasValue || !resortId.HasValue)
        {
            TempData["Error"] = "Invalid access. Please create review from booking.";
            return RedirectToAction("Index", "Booking");
        }

        var review = new Review
        {
            BookingId = bookingId.Value,
            ResortId = resortId.Value
        };

        return View(review);
    }

    // ---------------- CREATE (POST) ----------------
    [HttpPost]
    public async Task<IActionResult> Create(Review review)
    {
        try
        {
            // GET USER FROM SESSION
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            review.UserId = Convert.ToInt32(userId);

            // BASIC VALIDATION
            if (string.IsNullOrWhiteSpace(review.Body))
                throw new Exception("Comment is required");

            if (review.Rating < 1 || review.Rating > 5)
                throw new Exception("Rating must be between 1 and 5");

            await _api.PostAsync("api/review", review);

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(review);
        }
    }
}