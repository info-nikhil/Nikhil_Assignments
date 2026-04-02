using Microsoft.AspNetCore.Mvc;
using DBFirst_EF_DOTNET.Models;

namespace DBFirst_EF_DOTNET.Controllers
{
    public class NorthWindController : Controller
    {
        public IActionResult SpainCustomers()
        {
            NorthWindContext cnt = new NorthWindContext();
            var spainCustomer = cnt.Customers.Where(x => x.Country == "Spain").
                Select(x => new SpainCustomerViewModel
                {
                    Cid = x.CustomerId,
                    companyname = x.CompanyName,
                    Cname = x.ContactName
                }).ToList();

            return View(spainCustomer);
        }

        public IActionResult SearchCustomer(string contactName)
        {
            NorthWindContext cnt = new NorthWindContext();
            var searchCustomer = from customer in cnt.Customers
                                 where customer.ContactName == contactName
                                 select new Customer
                                 {
                                     ContactName = customer.ContactName,
                                     ContactTitle = customer.ContactTitle,
                                     CompanyName = customer.CompanyName
                                 };

            var searchCustomer2 = cnt.Customers.Where(x => x.ContactName == contactName)
                .Select(x => new Customer
                {
                    ContactName = x.ContactName,
                    ContactTitle = x.ContactTitle,
                    CompanyName = x.CompanyName
                });

            var query1 = searchCustomer.Single(); // can also use searchCustomer2
            return View(query1);
        }

        public ActionResult ProductsInCategory(string categoryname)
        {
            NorthWindContext cnt = new NorthWindContext();
            var productsincategory = cnt.Products
                .Where(x => x.Category.CategoryName == categoryname)
                .Select(x => new ProdCat
                 {
                    prodname = x.ProductName,
                    catname = x.Category.CategoryName
                 }).ToList();
            return View(productsincategory);
        }

        public ActionResult OrderRange(string range)
        {
            NorthWindContext cnt = new NorthWindContext();
            //var range1 = Convert.ToInt16(range);
            var range1 = string.IsNullOrEmpty(range) ? 0 : Convert.ToInt16(range);
            var custOrderCount = cnt.Customers
                .Where(x => x.Orders.Count > range1)
                .Select(x => new Customer
                {
                    CustomerId = x.CustomerId,
                    ContactName = x.ContactName,    
                });
            return View(custOrderCount);
        }

        public IActionResult CustomerOrderDetails(string id)
        {
            NorthWindContext cnt = new NorthWindContext();

            var orders = cnt.Orders
                .Where(o => o.CustomerId == id)
                .Select(o => new Order
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    RequiredDate = o.RequiredDate,
                    ShippedDate = o.ShippedDate
                }).ToList();

            ViewBag.CustomerId = id;

            return View(orders);
        }


        //public IActionResult SpainCustomers()
        //{
        //    NorthWindContext cnt = new NorthWindContext();
        //    var spainCustomer = cnt.Customers.Where(x => x.Country == "Spain").
        //        Select( x => new Customer
        //        {
        //            CustomerId = x.CustomerId,
        //            CompanyName = x.CompanyName,
        //            ContactName = x.ContactName,
        //            ContactTitle = x.ContactTitle
        //        }).ToList();

        //    return View(spainCustomer);
        //}
    }
}
