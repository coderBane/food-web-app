using Foody.Entities.Models;

namespace Foody.Entities.Repositories;

public interface IItemRepository<T> : IRepository<T> where T : Item
{
    bool Exists(int id);

    Task<bool> ExistsAsync(string name);

    Task UpdateAsync(T entity);
}

public interface ICategoryRepository : IItemRepository<Category> { }

public interface IProductRepository : IItemRepository<Product> { }

