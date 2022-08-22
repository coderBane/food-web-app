using Foody.Data.Data;
using Foody.Data.Interfaces;
using Foody.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Foody.Data.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(FoodyDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<RefreshToken>> All()
        {
            try
            {
                return await _dbSet.Where(rt => rt.Status == 1)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception) { return Enumerable.Empty<RefreshToken>(); }
        }
    }
}
