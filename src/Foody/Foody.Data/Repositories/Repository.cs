using System;
using Foody.Data.Data;
using Foody.Data.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Foody.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class  
{
    protected readonly FoodyDbContext _context;
    internal DbSet<T> _dbSet;

    public Repository(FoodyDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public virtual async Task<IEnumerable<T>> All(string search)
    {
       return await _dbSet.ToListAsync();
    }

    public virtual async Task Delete(int id)
    {
        var entity = await Get(id);

        if (entity is null)
            throw new NullReferenceException(nameof(entity) + " Was not found");

        _dbSet.Remove(entity);
    }

    public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return  _dbSet.Where(predicate);
    }

    public virtual async Task<T?> Get(int id)
    {
        return await _dbSet.FindAsync(id);
    }
}

