using System;
using Foody.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Foody.Data.Data
{
    public class FoodyDbContext : IdentityDbContext
    {
        public virtual DbSet<Category> Categories => Set<Category>();
        public virtual DbSet<Product> Products => Set<Product>();
        public virtual DbSet<AppUser> AppUsers => Set<AppUser>();
        public virtual DbSet<Item> Items => Set<Item>();

        public FoodyDbContext(DbContextOptions<FoodyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Item>()
                   .HasDiscriminator();
        }
    }
}

