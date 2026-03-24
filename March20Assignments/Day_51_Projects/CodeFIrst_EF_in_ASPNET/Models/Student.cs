namespace CodeFIrst_EF_in_ASPNET.Models
{
    public class Student
    {
        public int Id { get; set; }

        public List<Course> Courses { get; set; }
    }
}
