namespace CodeFIrst_EF_in_ASPNET.Models
{
    public class Author1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Course1> Courses { get; set; }
    }
}
