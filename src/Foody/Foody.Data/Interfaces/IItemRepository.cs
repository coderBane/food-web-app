namespace Foody.Data.Interfaces;

public interface IItemRepository<T> : IRepository<T> where T : Item
{
    bool Exists(int id);

    Task Update(T entity);

    Task<bool> Exists(string name);
}

public interface ICategoryRepository : IItemRepository<Category> { }

public interface IProductRepository : IItemRepository<Product>
{
    Task<IReadOnlyDictionary<string, List<ProdCategoryDto>>?> ProducstByCategory();
}

