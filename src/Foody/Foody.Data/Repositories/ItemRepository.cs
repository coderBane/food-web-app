using Foody.Data.Data;
using Foody.Data.Interfaces;

namespace Foody.Data.Repositories
{
    public abstract class ItemRepository<T> : Repository<T>, IItemRepository<T>
        where T : Item
    {
        public ItemRepository(FoodyDbContext context) : base(context) { }

        public bool Exists(int id) => (_dbSet?.Any(i => i.Id == id)).GetValueOrDefault();

        public async Task<bool> Exists(string name) => await _dbSet.AnyAsync(i => i.Name == name);

        public virtual async Task Update(T entity)
        {
            var existing = await base.Get(entity.Id);

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
    }
}

