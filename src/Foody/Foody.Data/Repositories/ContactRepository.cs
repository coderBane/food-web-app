using Foody.Data.Data;
using Foody.Data.Interfaces;
using System.Runtime.InteropServices;


namespace Foody.Data.Repositories
{
    public sealed class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(FoodyDbContext context) : base(context) { }

        public override async Task<IEnumerable<Contact>> All([Optional] string? search)
        {
            return string.IsNullOrEmpty(search) ? await base.All(search) :
                await _dbSet.Where(c => c.Name.ToLower().Contains(search.ToLower()))
                            .AsNoTracking()
                            .ToListAsync();
        }

        public override Task<Contact?> Get(int id)
        {
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ToggleRead(int id)
        {
            var inquiry = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

            if (inquiry is null)
                return false;

            inquiry.Read = !inquiry.Read;
            return true;
        }
    }
}

