using Foody.Data.Data;

namespace Foody.Data.Repositories;

public sealed class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(FoodyDbContext context, ILogger logger) : base(context, logger)
    {
    }
}