using System.Runtime.InteropServices;
using Foody.Data.Data;

namespace Foody.Data.Repositories;

public sealed class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(FoodyDbContext context, ILogger logger) : base(context, logger)
    {
    }

    public override async Task<IEnumerable<Order>> AllAsync([Optional] string search)
    {
        if (string.IsNullOrWhiteSpace(search)) 
            return await base.AllAsync();

        search = search.ToLower();
        var order = _dbSet.Where(o => o.State == 1
            && (o.OrderNo.ToLower().Contains(search) || o.Lastname.ToLower().Contains(search)));

        return await order.AsNoTracking().ToListAsync() ?? Enumerable.Empty<Order>();
    }

    public override async Task<Order?> GetAsync(int id)
    {
        return await _dbSet.Include(o => o.OrderDetails).AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order?> GetByNoAsync(string orderNo)
    {
        return await _dbSet.Include(o => o.OrderDetails)
                           .ThenInclude(od => od.Product)
                           .FirstOrDefaultAsync(o => o.OrderNo == orderNo);
    }
}