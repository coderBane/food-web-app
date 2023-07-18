using Foody.Data.Data;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Foody.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class, IEntity
{
    private readonly FoodyDbContext _context;

    protected internal DbSet<T> _dbSet;

    protected internal ILogger _logger;

    public Repository(FoodyDbContext context, ILogger logger)
    {
        _logger = logger;
        _context = context;
        _dbSet = _context.Set<T>() ?? throw new Exception("DbContext could not be set");
        _logger.LogDebug("Initialize repo - {repo}", typeof(T).Name);
    }

    public IQueryable<T> Table => _dbSet;

    public virtual async Task<bool> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        _logger.LogDebug("Entity flagged for add");
        return true;
    }

    public virtual async Task<IEnumerable<T>> AllAsync([Optional] string search)
    {
        return await _dbSet.AsNoTracking().ToListAsync() ?? Enumerable.Empty<T>();
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null) {
            _logger.LogDebug("no record of entity {id}", id);
            return false;
        }

        _dbSet.Remove(entity);
        _logger.LogDebug("Entity flagged for deletion");
        return true;
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate) ?? Enumerable.Empty<T>();
    }

    public virtual async Task<T?> GetAsync(int id)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }
}

