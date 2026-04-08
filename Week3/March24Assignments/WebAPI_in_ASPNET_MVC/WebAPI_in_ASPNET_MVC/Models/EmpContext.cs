using Microsoft.EntityFrameworkCore;

namespace WebAPI_in_ASPNET_MVC.Models
{
    public class EmpContext : DbContext
    {
        public EmpContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Employee> employees { set; get; }
    }
}
