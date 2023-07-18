namespace Foody.Entities.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Categories { get; }

    IProductRepository Products { get; }

    IContactRepository Inquiries { get; }

    INewsletterRepository Subcribers { get; }

    IOrderRepository Orders { get; }

    IRefreshTokenRepository RefreshToken { get; }

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}

