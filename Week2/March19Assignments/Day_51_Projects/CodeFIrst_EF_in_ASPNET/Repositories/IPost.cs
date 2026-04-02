using CodeFIrst_EF_in_ASPNET.Models;

namespace CodeFIrst_EF_in_ASPNET.Repositories
{
    public interface IPost
    {
        List<Post> GetPosts();

        Post GetPostByID(int postid);

        void InsertPost(Post post);

        void DeletePost(int postid);

        void UpdatePost(Post post);

        void save();
    }
}
