namespace Foody.WebApi.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<OrderModDto, Order>();
        CreateMap<OrderDetailDto, OrderDetail>();
    }
}