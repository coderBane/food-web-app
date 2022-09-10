using System;
using System.Linq.Expressions;

namespace Foody.Data.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> All(string search);

    Task<T?> Get(int id);

    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    void Add(T entity);

    Task Delete(int id);
}

