using System;
using System.Linq.Expressions;

namespace Foody.Data.Interfaces;

/// <summary>Generic Repository</summary>
/// <typeparam name="T">Model</typeparam>
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> All();

    Task<T?> Get(int id);

    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    void Add(T entity);

    Task Delete(int id);
}

