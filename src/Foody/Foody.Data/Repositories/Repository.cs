using Foody.Data.Data;
using Foody.Data.Interfaces;
using System.Linq.Expressions;
using System.Runtime.InteropServices;


namespace Foody.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly FoodyDbContext _context;

    protected internal DbSet<T> _dbSet;

    public Repository(FoodyDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>() ?? throw new Exception("DbContext could not be set");
    }

    public virtual async Task<IEnumerable<T>> All([Optional] string? search)
    {
        return await _dbSet.AsNoTracking()
                           .ToListAsync();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public virtual async Task<T?> Get(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task Add(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task Delete(int id)
    {
        var entity = await Get(id);

        if (entity is null)
            throw new NullReferenceException($"{nameof(entity)} with given id does not exist");

        _dbSet.Remove(entity);
    }
}

