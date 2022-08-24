using Foody.Entities.Models;

namespace Foody.Data.Interfaces;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<bool> Update(RefreshToken storedToken);
}

