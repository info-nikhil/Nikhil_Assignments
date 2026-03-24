namespace CodeFIrst_EF_in_ASPNET.Models
{
    public class Post
    {
        public int Id { set; get; }
        public DateTime DatePublished { set; get; }

        public string Title { set; get; }

        public string Body { set; get; }
    }
}
