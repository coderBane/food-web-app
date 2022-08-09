using System;
using Foody.Data.Data;
using Foody.Data.Interfaces;
using Foody.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Foody.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(FoodyDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Category>> All()
    {
        return await _dbSet.Where(c => c.Status == 1)
                           .AsNoTracking()
                           .ToListAsync();
    }

    public bool Exists(int id)
    {
        return (_context.Categories?.Any(c => c.Id == id)).GetValueOrDefault();
    }

    public Task Update(Category category)
    {
        throw new NotImplementedException();
    }
}

