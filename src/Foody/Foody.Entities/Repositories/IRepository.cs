using Foody.Entities.Models;
using System.Linq.Expressions;
using System.Runtime.InteropServices;


namespace Foody.Entities.Repositories;

public interface IRepository<T> where T : class, IEntity
{
    IQueryable<T> Table { get; }

    /// <summary>Retrieve all rows from databse</summary>
    /// <param name="search"></param>
    /// <returns>List of objects</returns>
    Task<IEnumerable<T>> AllAsync([Optional] string search);

    /// <summary>Retrieve a single entity from database</summary>
    /// <param name="id">object id</param>
    /// <returns>The object</returns>
    Task<T?> GetAsync(int id);

    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    /// <summary>Flag entity for addition on save changes</summary>
    /// <param name="entity"></param>
    /// <returns>true if flagged successfully</returns>
    Task<bool> AddAsync(T entity);

    /// <summary>Flag entity for deletion on save changes</summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(int id);
}

