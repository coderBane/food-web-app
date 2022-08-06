using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Foody.Entities.Models;

namespace Foody.Data.Data
{
    public class FoodyDbContext : IdentityDbContext
    {
        public DbSet<Item> Items => Set<Item>();

        public FoodyDbContext(DbContextOptions<FoodyDbContext> options) : base(options) { }

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

