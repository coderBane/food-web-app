using System;
using Foody.Data.Data;
using Foody.Entities.Models;
using Foody.Data.Interfaces;

namespace Foody.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(FoodyDbContext context) : base(context)
        { 
        }

        public bool Exists(int id)
        {
            return (_context.Products?.Any(p => p.Id == id)).GetValueOrDefault();
        }

        public Task Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}

