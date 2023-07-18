using Foody.Data.Data;

namespace Foody.Data.Repositories
{
    public sealed class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(FoodyDbContext context, ILogger logger) : base(context, logger) { }
         
        public async Task<bool> UpdateAsync(RefreshToken storedToken)
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
