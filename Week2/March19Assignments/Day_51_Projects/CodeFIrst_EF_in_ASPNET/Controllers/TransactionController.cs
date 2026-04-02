using CodeFIrst_EF_in_ASPNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CodeFIrst_EF_in_ASPNET.Controllers
{
    public class TransactionController : Controller
    {
        private readonly EventContext _context;
        public TransactionController(EventContext context)
        {
            _context = context;
        }
        public IActionResult CreateCustomer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            ModelState.Clear();
            ModelState.Remove(nameof(customer.CustomerId));
            if (ModelState.IsValid)
            {                
                _context.Customers.Add(customer);
                _context.SaveChanges();
                //return Content("Customer Added");
                return RedirectToAction("CreateProduct", new {customerId=customer.CustomerId});
            }
            return View(customer);
        }
        
        public IActionResult CreateProduct(int? customerId=null)
        {
            var cid = customerId ?? 0;
            ViewBag.customerId = cid;
            ViewBag.CustomerList = new SelectList(_context.Customers, "CustomerId", "CustomerName", cid);
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            ModelState.Clear();
            ModelState.Remove(nameof(product.ProductId));
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("CreateProduct", new { customerId = product.CustomerId });
            }
            ViewBag.customerId = product.CustomerId;
            ViewBag.CustomerList = new SelectList(_context.Customers, "CustomerId", "CustomerName", product.CustomerId);
            return View(product);
        }

        public IActionResult Summary(int customerId)
        {
            var customer = _context.Customers
                .Include(c => c.Products)
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null || !customer.Products.Any())
            {
                return RedirectToAction("CreateProduct", new { customerId });
            }

            return View(customer);
        }
    }
}
