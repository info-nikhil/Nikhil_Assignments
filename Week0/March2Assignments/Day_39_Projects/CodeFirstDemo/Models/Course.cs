using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstDemo.Models
{
    class Course
    {
        public int Id { get; set; }
        public string Title { set; get;  }
        public string Description { set; get; }
        public CourseLevel level { set; get; }
        public List<Student> Students { set; get; }
        public Author Author { set; get; }
        public int AuthorId { get; set; }
    }
    public enum CourseLevel
    {
        Beginner = 1,
        Average = 2,
        Difficult = 3
    }
}
