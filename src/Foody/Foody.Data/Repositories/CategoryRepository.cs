using Foody.Data.Data;
using Foody.Data.Interfaces;
using System.Runtime.InteropServices;


namespace Foody.Data.Repositories
{
    public sealed class CategoryRepository : ItemRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(FoodyDbContext context) : base(context) { }


        public override async Task<IEnumerable<Category>> All([Optional] string? search)
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

        public override async Task<Category?> Get(int id)
        {
            try
            {
                return await  _dbSet.Where(c => (c.State == 1 && c.Id == id))
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
}
