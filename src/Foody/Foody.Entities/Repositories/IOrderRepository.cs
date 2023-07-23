using Foody.Entities.Models;

namespace Foody.Entities.Repositories
{
	public interface IOrderRepository : IRepository<Order>
	{
		Task<Order?> GetByNoAsync(string orderNo);
	}
}

