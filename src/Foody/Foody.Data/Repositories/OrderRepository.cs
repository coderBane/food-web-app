using Foody.Data.Data;
using Foody.Data.Interfaces;

namespace Foody.Data.Repositories;

public sealed class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(FoodyDbContext context) : base(context)
    {
    }
}