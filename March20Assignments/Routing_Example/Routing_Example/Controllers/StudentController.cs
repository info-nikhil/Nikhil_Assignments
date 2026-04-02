using Microsoft.AspNetCore.Mvc;
using Routing_Example.Models;

namespace Routing_Example.Controllers
{
    public class StudentController : Controller
    {
        List<Student> studList = new List<Student>() 
        {
            new Student {Id=101, Name="Niksh", Class="6"},
            new Student {Id=102, Name="Ayra", Class="7"},
            new Student {Id=103, Name="Akanksha", Class="8"}

        };

        [Route("studs")]
        public IActionResult GetAllStudents()
        {
            return View(studList);
        }

        [Route("studs/{id}")]
        public IActionResult GetStudent(int id)
        {
            var student = studList.FirstOrDefault(x => x.Id == id);
            return View(student);
        }

        [Route("studsfew")]
        public IActionResult FewColumns()
        {
            var fewColumns = studList.Select(x => new Student
            {
                Class = x.Class,
                Name = x.Name,
            });
            return View(fewColumns);
        }
    }
}
