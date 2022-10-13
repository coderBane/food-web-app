using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Foody.Data.Interfaces;

public interface IRepository<T> where T : class
{
    /// <summary>Retrieve all from databse</summary>
    /// <param name="search"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> All([Optional]string? search);

    /// <summary>Find by Id</summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T?> Get(int id); 

    /// <summary>Find by Condition</summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    /// <summary>Add to database</summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Add(T entity);

    /// <summary>Delete from database</summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(int id);
}

