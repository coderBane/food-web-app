using Foody.Entities.Models;

namespace Foody.Entities.Repositories;

public interface IContactRepository : IRepository<Contact>
{
    Task<bool> ToggleRead(int id);
}

