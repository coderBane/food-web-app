using Foody.Entities.Models;

namespace Foody.Entities.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<bool> UpdateAsync(RefreshToken storedToken);
}

