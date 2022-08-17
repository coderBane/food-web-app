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
        return (_dbSet?.Any(c => c.Id == id)).GetValueOrDefault();
    }

    public async Task Update(Category category)
    {
        var existing = await _dbSet.Where(x => x.Id == category.Id).FirstOrDefaultAsync();
        if (existing is not null)
        {
            existing.Name = category.Name;
            existing.IsActive = category.IsActive;
            existing.ImageUri = category.ImageUri;
            existing.ImageData = category.ImageData;
            existing.Updated = DateTime.UtcNow;
        }
    }
}
