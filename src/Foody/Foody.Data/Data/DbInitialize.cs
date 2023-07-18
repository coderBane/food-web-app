﻿using Foody.Data.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace Foody.Data.Data
{
    public static class DbInitialize
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, string pw)
        {
            using var context = serviceProvider.GetRequiredService<FoodyDbContext>();

            if (await context.Database.CanConnectAsync())
            {
                var userId = await NewUser(serviceProvider, pw);
                await NewRole(serviceProvider, userId);

                SeedData(context);
            }
        }

        public static void SeedData(FoodyDbContext context)
        {
            if (!context.Items.Any())
            {
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
            }

            if (!context.Subcribers.Any())
            {
                Enumerable.Range(0, 5).Select(_ =>
                {
                    var name = Faker.Name.First();
                    return new Newsletter
                    {
                        Name = name,
                        Email = Faker.Internet.Email(name)
                    };
                }).ToList().ForEach(x => context.Subcribers.Add(x));
            }

            if (!context.Inquiries.Any())
                Enumerable.Range(0, 3).Select(_ => 
                {
                    var name = _ % 2 == 0 ?  Faker.Name.FullName() : Faker.Name.First();
                    return new Contact
                    {
                        Name = name,
                        Email = Faker.Internet.Email(name),
                        Subject = Faker.Lorem.Sentence(5),
                        Message = string.Join(Environment.NewLine ,Faker.Lorem.Sentences(10)),
                        Tel = _ is 1 ? Faker.Phone.Number() : default
                    };
                }).ToList().ForEach(x => context.Inquiries.Add(x));

            context.SaveChanges();
        }

        public static async Task<string> NewUser(IServiceProvider serviceProvider, string pass)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            if (userManager is null)
                throw new NullReferenceException($"{nameof(userManager)} is null");

            var user = await userManager.FindByNameAsync(UserConstants.Administrator.username);

            if (user is null)
            {
                user = new IdentityUser(UserConstants.Administrator.username)
                {
                    Email = UserConstants.Administrator.email,
                    EmailConfirmed = true
                };
                var res = await userManager.CreateAsync(user, pass);
                if (!res.Succeeded) throw new Exception("Failed to initialise admin user");
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

            var user = await userManager.FindByIdAsync(uid) ?? throw new NullReferenceException("User not found!");

            if (!await userManager.IsInRoleAsync(user, "Admin") ) 
                await userManager.AddToRoleAsync(user, "Admin");
            if (!await userManager.IsInRoleAsync(user, "User"))
                await userManager.AddToRoleAsync(user, "User");
        }
    }
}

