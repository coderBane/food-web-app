using Foody.Data.Data;
using Foody.Data.Interfaces;
using Foody.Entities.Models;

namespace Foody.Data.Repositories;

public sealed class NewsletterRepository : Repository<Newsletter>, INewsletterRepository
{
    public NewsletterRepository(FoodyDbContext context) : base(context)
    {
    }

    public bool Duplicate(string email)
    {
        return (_dbSet?.Any(s => s.Email == email)).GetValueOrDefault();
    }
}

