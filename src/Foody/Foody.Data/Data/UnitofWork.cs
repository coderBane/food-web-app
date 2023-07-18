using Foody.Data.Repositories;
using System.Diagnostics.CodeAnalysis;


namespace Foody.Data.Data;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly FoodyDbContext _context;

    private readonly ILogger _logger;

    public UnitOfWork(FoodyDbContext context, ILoggerFactory loggerFactory)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = loggerFactory.CreateLogger<UnitOfWork>();

        Categories = new CategoryRepository(_context, _logger);
        Products = new ProductRepository(_context, _logger);
        Orders = new OrderRepository(_context, _logger);
        RefreshToken = new RefreshTokenRepository(_context, _logger);
        Subcribers = new NewsletterRepository(_context, _logger);
        Inquiries = new ContactRepository(_context, _logger);
    }

    public ICategoryRepository Categories { get; private set; }

    public IProductRepository Products { get; private set; }

    public IOrderRepository Orders { get; private set; }

    public IRefreshTokenRepository RefreshToken { get; private set; }

    public INewsletterRepository Subcribers { get; private set; }

    public IContactRepository Inquiries { get; private set; }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Persisting changes to database...");
        return await _context.SaveChangesAsync(cancellationToken);
    }

    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
    public void Dispose()
    {
        _context.Dispose();
    }
}

