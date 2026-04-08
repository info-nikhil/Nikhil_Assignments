using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstDemo.Models;


namespace CodeFirstDemo.Data
{
    class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<Course> Courses { set; get; }
        public DbSet<Student> Students { set; get; }
        public DbSet<Author> Authors { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-86VHIPO\\SQLEXPRESS;Database=CgEfDb1;" +
                "Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "John Doe" },
                new Author { Id = 2, Name = "Jane Smith" }
            );

            // Seed Courses (with AuthorId)
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Title = "C# Basics", Description = "Intro to C#", level = CourseLevel.Beginner, AuthorId = 1 },
                new Course { Id = 2, Title = "Advanced .NET", Description = "Deep dive into .NET", level = CourseLevel.Average, AuthorId = 1 },
                new Course { Id = 3, Title = "Azure Fundamentals", Description = "Cloud basics", level = CourseLevel.Beginner, AuthorId = 2 }
            );

            // Seed Students
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, Name = "Alice Johnson" },
                new Student { Id = 2, Name = "Bob Wilson" },
                new Student { Id = 3, Name = "Carol Davis" }
            );
        }
    }
}
