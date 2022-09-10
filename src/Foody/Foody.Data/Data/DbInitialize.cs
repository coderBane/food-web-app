using System;
using Foody.Entities.Models;

namespace Foody.Data.Data
{
    public static class DbInitialize
    {
        public static void Initialize(FoodyDbContext context)
        {
            if (context.Categories.Any())
                return;

            new List<Category>()
            {
                new() { Name = "Rice", IsActive = true },
                new() { Name = "Protien" },
                new() { Name = "Sides" },
                new() { Name = "Drinks" },
                new() { Name = "Food Tray" }
            }.ForEach(x => context.Categories.Add(x));

            context.SaveChanges();

            new List<Product>()
            {
                new() { Name = "Jambalaya", CategoryId = 1, IsActive = true, Price = 3000, Description = Faker.Lorem.Sentence() },
                new() { Name = "Jollof Rice", CategoryId = 1, IsActive = true, Price = 1500, Description = "The Best party rice." },
                new() { Name = "Beef", CategoryId = 2, Price = 800 },
                new() { Name = "Chicken", CategoryId = 2, Price = 1000, Description = "Juicy lap" },
                new() { Name = "Asun", CategoryId = 3, IsActive = true, Price = 1500, Description = "Assorted meat (spicy)." },
                new() { Name = "puff puff", CategoryId = 3, IsActive = true, Price = 500, Description = "Sweet dough balls." },
                new() { Name = "Mango Juice", CategoryId = 4, Price = 1500},
                new() { Name = "Zobo", CategoryId = 4, IsActive = true, Price = 1000, Description = "Purple hibiscus drink" },

            }.ForEach(x => context.Products.Add(x));

            context.SaveChanges();
        }
    }
}

