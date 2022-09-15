using System;
using Foody.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace Foody.Data.Data
{
    public static class DbInitialize
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, string pw)
        {
            using var context = serviceProvider.GetRequiredService<FoodyDbContext>();

            var userId = await NewUser(serviceProvider, pw);
            await NewRole(serviceProvider, userId);

            SeedData(context);
        }

        public static void SeedData(FoodyDbContext context)
        {
            if (context.Items.Any())
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
                new() { Name = "Jambalaya", CategoryId = 101, IsActive = true, Price = 3000, Description = Faker.Lorem.Sentence() },
                new() { Name = "Jollof Rice", CategoryId = 101, IsActive = true, Price = 1500, Description = "The Best party rice." },
                new() { Name = "Beef", CategoryId = 102, Price = 800 },
                new() { Name = "Chicken", CategoryId = 102, Price = 1000, Description = "Juicy lap" },
                new() { Name = "Asun", CategoryId = 103, IsActive = true, Price = 1500, Description = "Assorted meat (spicy)." },
                new() { Name = "puff puff", CategoryId = 103, IsActive = true, Price = 500, Description = "Sweet dough balls." },
                new() { Name = "Mango Juice", CategoryId = 104, Price = 1500},
                new() { Name = "Zobo", CategoryId = 104, IsActive = true, Price = 1000, Description = "Purple hibiscus drink" },

            }.ForEach(x => context.Products.Add(x));

            context.SaveChanges();
        }

        public static async Task<string> NewUser(IServiceProvider serviceProvider, string pass)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            if (userManager is null)
                throw new NullReferenceException($"{nameof(userManager)} is null");

            var user = await userManager.FindByNameAsync("admin");

            if (user is null)
            {
                user = new IdentityUser("admin")
                {
                    Email = "admin@foody.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, pass);
            }   

            return user.Id;
        }

        public static async Task NewRole(IServiceProvider serviceProvider, string uid)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (roleManager is null)
                throw new NullReferenceException($"{nameof(roleManager)} is null");

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            if (userManager is null)
                throw new NullReferenceException($"{nameof(userManager)} is null");

            var user = await userManager.FindByIdAsync(uid);

            if (user is null)
                throw new NullReferenceException("User not found!");

            if (!await userManager.IsInRoleAsync(user, "Admin") ) 
                await userManager.AddToRoleAsync(user, "Admin");
            if (!await userManager.IsInRoleAsync(user, "User"))
                await userManager.AddToRoleAsync(user, "User");
        }
    }
}

