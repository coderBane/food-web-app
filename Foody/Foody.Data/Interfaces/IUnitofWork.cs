namespace Foody.Data.Interfaces;

public interface IUnitofWork
{
    ICategoryRepository Categories { get; }

    IProductRepository Products { get; }

    IRefreshTokenRepository RefreshToken { get; }

    Task CompleteAsync();
}

