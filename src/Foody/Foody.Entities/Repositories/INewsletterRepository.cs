using Foody.Entities.Models;

namespace Foody.Entities.Repositories;

public interface INewsletterRepository : IRepository<Newsletter>
{
    bool Exists(string email);
}

