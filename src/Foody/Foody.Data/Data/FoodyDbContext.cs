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
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Contact> Inquiries => Set<Contact>();
        public DbSet<Newsletter> Subcribers => Set<Newsletter>();
        public virtual DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public FoodyDbContext(DbContextOptions<FoodyDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseLoggerFactory(_loggerFactory);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>(b =>
            {
                b.Property(i => i.Id)
                 .HasIdentityOptions(startValue: 101);

                b.HasIndex(i => i.Name)
                 .HasDatabaseName("ItemNameIndex")
                 .IsUnique();

                b.HasDiscriminator();

                b.HasComment("Table which implements table-per-heirachy inheritance (TPH)" + "\n" +
                    "Contains data for both Categories and Product.");
            });

            modelBuilder.Entity<Newsletter>()
                .HasComment("Newsletter subcribers.");

            modelBuilder.HasDefaultSchema("api");
        }
    }
}

