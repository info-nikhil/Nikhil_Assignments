using CodeFirstDemo.Data;
using CodeFirstDemo.Models;
using Microsoft.EntityFrameworkCore;

var context = new AppDbContext();

// create category

//var electronics = new Category { Name = "Electronics" };

//context.Categories.Add(electronics);
//await context.SaveChangesAsync();

//context.Products.AddRange(
//    new Product { Name = "Laptop", Price = 999.78M, category = electronics },
//    new Product { Name = "Mouse", Price = 189.24M, category = electronics }
//);

//await context.SaveChangesAsync();



// update command

//var laptop = await context.Products.FirstAsync(p => p.Name == "Laptop");
//laptop.Price = 786.62M;
//await context.SaveChangesAsync();



// delete command

//context.Products.Remove(laptop);
//context.SaveChangesAsync();



// Querring author with courses

var authors = await context.Authors.Include(x => x.Courses).ToListAsync();
foreach(var author in authors)
{
    Console.WriteLine($"Author: {author.Name}");
    foreach(var course in author.Courses)
    {
        Console.WriteLine($"{course.Title} -- {course.Description} -- {course.level}");
    }
}