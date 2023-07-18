using System.Runtime.InteropServices;
using Foody.Data.Data;

namespace Foody.Data.Repositories
{
    public abstract class ItemRepository<T> : Repository<T>, IItemRepository<T>
        where T : Item
    {
        public ItemRepository(FoodyDbContext context, ILogger logger) : base(context, logger) { }

        public bool Exists(int id) => (_dbSet?.Any(i => i.Id == id)).GetValueOrDefault();

        public async Task<bool> ExistsAsync(string name) => await _dbSet.AnyAsync(i => i.Name == name);

        public virtual Task UpdateAsync(T entity)
        {
            var existing = _dbSet.Find(entity.Id);

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
                _logger.LogDebug("Flagged Item for update {id}", existing.Id);
            }

            return Task.CompletedTask;
        }
    }
}

