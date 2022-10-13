using Foody.Data.Data;
using Foody.Data.Interfaces;


namespace Foody.Data.Repositories
{
    public sealed class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(FoodyDbContext context) : base(context) { }

         
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
