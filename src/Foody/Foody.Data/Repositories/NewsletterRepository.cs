using Foody.Data.Data;
using Foody.Entities.Models;

namespace Foody.Data.Repositories;

public sealed class NewsletterRepository : Repository<Newsletter>, INewsletterRepository
{
    public NewsletterRepository(FoodyDbContext context, ILogger logger) : base(context, logger)
    {
    }

    public bool Exists(string email)
    {
        return (_dbSet?.Any(s => s.Email == email)).GetValueOrDefault();
    }
}

