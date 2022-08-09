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
                new() { Name = "drinks" },
                new() { Name = "Food Tray" }
            }.ForEach(x => context.Categories.Add(x));

            context.SaveChanges();
        }
    }
}

