using Foody.Entities.Models;

namespace Foody.Data.Interfaces
{
    public interface IItemRepository<T> : IRepository<T> where T : Item
    {
        bool Exists(int id);

        bool Exists(string name);

        Task Update(T entity);
    }
}

