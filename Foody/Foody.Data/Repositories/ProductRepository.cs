using System;
using Foody.Data.Data;
using Foody.Entities.Models;
using Foody.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Foody.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(FoodyDbContext context) : base(context)
        { 
        }

        public override async Task<IEnumerable<Product>> All()
        {
            var products = _dbSet.Where(p => p.Status == 1)
                .Include(p => p.Category)
                .AsQueryable();

            return await products.AsNoTracking().ToListAsync();
        }

        public override async Task<Product?> Get(int id)
        {
            return await _dbSet.Where(p => p.Status == 1)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return (_context.Products?.Any(p => p.Id == id)).GetValueOrDefault();
        }

        public async Task Update(Product product)
        {
            var existing = await _dbSet.Where(x => x.Id == product.Id).FirstOrDefaultAsync();

            if (existing is not null)
            {
                existing.Name = product.Name;
                existing.IsActive = product.IsActive;
                existing.Price = product.Price;
                existing.Quantity = product.Quantity;
                existing.CategoryId = product.CategoryId;
                existing.ImageUri = product.ImageUri;
                existing.ImageData = product.ImageData;
                existing.Description = product.Description;
                existing.Updated = DateTime.UtcNow;
            }
        }
    }
}

