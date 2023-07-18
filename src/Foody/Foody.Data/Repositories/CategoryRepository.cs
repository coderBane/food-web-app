using Foody.Data.Data;
using System.Runtime.InteropServices;


namespace Foody.Data.Repositories;

public sealed class CategoryRepository : ItemRepository<Category>, ICategoryRepository
{
    public CategoryRepository(FoodyDbContext context, ILogger logger) : base(context, logger) { }


    public override async Task<IEnumerable<Category>> AllAsync([Optional] string search)
    {
        try
        {
            return await _dbSet.Where(c => c.State == 1)
                               .AsNoTracking()
                               .ToListAsync();
        }
        catch (Exception)
        {
            
        }

        return Enumerable.Empty<Category>();
    }

    public override async Task<Category?> GetAsync(int id)
    {
        try
        {
            return await _dbSet.Where(c => c.State == 1 && c.Id == id)
                               .Include(c => c.Image)
                               .AsNoTracking()
                               .FirstOrDefaultAsync();   
        }
        catch (Exception)
        {
            
        }

        return null;
    }
}
