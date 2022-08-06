using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Foody.Entities.Models;

namespace Foody.Data.Data
{
    public class FoodyDbContext : IdentityDbContext
    {
        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(
            builder => { builder.AddConsole(); });

        public DbSet<Item> Items => Set<Item>();
        public DbSet<AppUser> AppUsers => Set<AppUser>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();

        public FoodyDbContext(DbContextOptions<FoodyDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder) =>
            optionbuilder.UseLoggerFactory(_loggerFactory);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Item>(b =>
            {
                b.HasIndex(i => i.Name)
                 .IsUnique();

                b.HasDiscriminator();
            });
        }
    }
}

