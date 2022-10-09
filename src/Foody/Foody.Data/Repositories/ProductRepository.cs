using Foody.Data.Data;
using Foody.Entities.Models;
using Foody.Data.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Foody.Data.Repositories
{
    public sealed class ProductRepository : ItemRepository<Product>, IProductRepository
    {
        public ProductRepository(FoodyDbContext context) : base(context)
        { 
        }

        public override async Task<IEnumerable<Product>> All(string? search)
        {
            var products = _dbSet.Where(p => p.State == 1)
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                string s = search.ToLower();
                products = products.Where(p =>
                    p.Name.ToLower().Contains(s) || p.Category.Name.ToLower().Contains(s));
            }

            return await products.AsNoTracking().ToListAsync();
        }

        public override async Task<Product?> Get(int id)
        {
            return await _dbSet.Where(p => p.State == 1)
                .Include(f => f.Image)
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task Update(Product product)
        {
            var existing = await _dbSet.FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existing is not null)
            {
                existing.Name = product.Name;
                existing.IsActive = product.IsActive;
                existing.Price = product.Price;
                existing.Quantity = product.Quantity;
                existing.CategoryId = product.CategoryId;
                existing.Description = product.Description;

                if (product.Image is not null)
                {
                    existing.Image = product.Image;
                    existing.ImageUri = product.ImageUri;
                }

                existing.Updated = DateTime.UtcNow;
            }
        }
    }
}

