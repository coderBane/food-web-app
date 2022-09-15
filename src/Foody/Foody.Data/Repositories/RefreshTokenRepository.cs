using Foody.Data.Data;
using Foody.Data.Interfaces;
using Foody.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Foody.Data.Repositories
{
    public sealed class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(FoodyDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<RefreshToken>> All(string? search)
        {
            try
            {
                return await _dbSet.Where(rt => rt.State == 1)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception) { return Enumerable.Empty<RefreshToken>(); }
        }

        public async Task<bool> Update(RefreshToken storedToken)
        {
            try
            {
                var token = await _dbSet.Where(rt => rt.Token == storedToken.Token)
                    .SingleOrDefaultAsync();

                token!.IsUsed = true;
                return true;
            }
            catch(Exception) { return false; }
        }
    }
}
