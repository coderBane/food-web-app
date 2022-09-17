using System;
using Foody.Data.Data;
using Foody.Data.Interfaces;
using Foody.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Foody.Data.Repositories
{
    public class ItemRepository<T> : Repository<T>, IItemRepository<T> where T : Item
    {
        public ItemRepository(FoodyDbContext context) : base(context) { }

        public bool Exists(string name) => (_dbSet?.Any(i => i.Name == name)).GetValueOrDefault();

        public bool Exists(int id) => (_dbSet?.Any(i => i.Id == id)).GetValueOrDefault();

        public virtual async Task Update(T entity)
        {
            var existing = await _dbSet.FirstOrDefaultAsync(i => i.Id == entity.Id);

            if (existing is not null)
            {
                existing.Name = entity.Name;
                existing.IsActive = entity.IsActive;

                if (entity.Image is not null)
                {
                    existing.Image = entity.Image;
                    existing.ImageUri = entity.ImageUri;
                }

                existing.Updated = DateTime.UtcNow;
            }
        }

        public override async Task<IEnumerable<T>> All(string search)
        {
            return await _dbSet.Where(i => i.State == 1)
                               .AsNoTracking()
                               .ToListAsync();
        }

        public override async Task<T?> Get(int id)
        {
            return await _dbSet.Where(i => i.State == 1)
                         .Include(f => f.Image)
                         .AsNoTracking()
                         .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}

