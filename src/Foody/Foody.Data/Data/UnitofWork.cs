using System;
using Foody.Data.Interfaces;
using Foody.Data.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace Foody.Data.Data;

public class UnitofWork : IUnitofWork, IDisposable
{
    private readonly FoodyDbContext _context;

    public UnitofWork(FoodyDbContext context)
    {
        _context = context;
        Categories = new CategoryRepository(_context);
        Products = new ProductRepository(_context);
        RefreshToken = new RefreshTokenRepository(_context);
        Subcribers = new NewsletterRepository(_context);
        Contacts = new ContactRepository(_context);
    }

    public ICategoryRepository Categories { get; private set; }

    public IProductRepository Products { get; private set; }

    public IRefreshTokenRepository RefreshToken { get; private set; }

    public INewsletterRepository Subcribers { get; private set; }

    public IContactRepository Contacts { get; private set; }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
    public void Dispose()
    {
        _context.Dispose();
    }
}

