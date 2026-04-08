using Microsoft.AspNetCore.Mvc;
using System.Text;
using ASPNET_Products_Frontend.Models;
using Newtonsoft.Json;

namespace ASPNET_Products_Frontend.Controllers
{
    public class ProductController : Controller
    {
        // GET: /Product
        public IActionResult Index()
        {
            return View();   // Data will come via AJAX
        }

        // GET: /Product/Create
        public IActionResult Create()
        {
            return View();   // Form handled via AJAX
        }

        // GET: /Product/Edit/5
        public IActionResult Edit(int id)
        {
            ViewBag.ProductId = id; // pass id to view
            return View();
        }

        // GET: /Product/Delete/5 (optional confirmation page)
        public IActionResult Delete(int id)
        {
            ViewBag.ProductId = id;
            return View();
        }
    }
}
