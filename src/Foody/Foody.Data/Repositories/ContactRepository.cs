using Foody.Data.Data;
using System.Runtime.InteropServices;


namespace Foody.Data.Repositories;

public sealed class ContactRepository : Repository<Contact>, IContactRepository
{
    public ContactRepository(FoodyDbContext context, ILogger logger) : base(context, logger) { }

    public override async Task<IEnumerable<Contact>> AllAsync([Optional] string search)
    {
        return string.IsNullOrWhiteSpace(search) ? await base.AllAsync() :
            await _dbSet.Where(c => c.Name.ToLower().Contains(search.ToLower()))
                        .AsNoTracking()
                        .ToListAsync() ?? Enumerable.Empty<Contact>();
    }

    public async Task<bool> ToggleRead(int id)
    {
        var inquiry = await _dbSet.FindAsync(id);

        if (inquiry is null)
            return false;

        inquiry.Read = !inquiry.Read;
        return true;
    }
}

