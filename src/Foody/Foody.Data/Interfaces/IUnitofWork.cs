namespace Foody.Data.Interfaces;

public interface IUnitofWork
{
    ICategoryRepository Categories { get; }

    IProductRepository Products { get; }

    IOrderRepository Orders { get; }

    IRefreshTokenRepository RefreshToken { get; }

    INewsletterRepository Subcribers { get; }

    IContactRepository Contacts { get; }

    Task CompleteAsync();
}

