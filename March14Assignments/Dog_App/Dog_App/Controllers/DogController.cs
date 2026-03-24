using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dog_App.Models;
namespace Dog_App.Controllers
{
    public class DogController : Controller
    {
        private static List<Dog> dogs = new List<Dog>();
        private readonly IWebHostEnvironment _environment;

        public DogController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        // GET: DogController
        public ActionResult Index(string? search)
        {
            var filtered = string.IsNullOrEmpty(search) ? dogs : dogs.Where(d => d.Name != null && d.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            return View(filtered);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            return View(dogs.FirstOrDefault(d => d.ID == id));
        }

        // GET: DogController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog d, IFormFile imagefile)
        {
            Guid rr = Guid.NewGuid();
            if (ModelState.IsValid)
            {
                if (imagefile != null && imagefile.Length > 0)
                {
                    var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imagefile.FileName);
                    var path = Path.Combine(_environment.WebRootPath, "images", imageName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        imagefile.CopyTo(stream);
                    }

                    d.ImagePath = "/images/" + imageName;
                }

                dogs.Add(d);
                return RedirectToAction("Index");
            }

            return View(d);
        }

        // GET: DogController/Edit/5
        public ActionResult Edit(int id)
        {
            var dog = dogs.FirstOrDefault(d => d.ID == id);
            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Dog d, IFormFile imagefile)
        {
            if (ModelState.IsValid)
            {
                var existing = dogs.FirstOrDefault(x => x.ID == d.ID);
                if (existing != null)
                {
                    existing.Name = d.Name;
                    existing.Age = d.Age;
                    existing.Description = d.Description;
                    // If new image uploaded
                    if (imagefile != null && imagefile.Length > 0)
                    {
                        var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imagefile.FileName);
                        var path = Path.Combine(_environment.WebRootPath, "images", imageName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            imagefile.CopyTo(stream);
                        }

                        existing.ImagePath = "/images/" + imageName;
                    }
                }
                return RedirectToAction("Index");
            }

            return View(d);
        }

        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(dogs.FirstOrDefault(d => d.ID == id));
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Dog d)
        {
            var dog = dogs.FirstOrDefault(x => x.ID == d.ID);
            if(dog != null)
            {
                dogs.Remove(dog);
            }
            return RedirectToAction("Index");
        }
    }
}
