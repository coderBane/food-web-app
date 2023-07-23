using Foody.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Foody.WebApi.Controllers.v1;

public sealed class OrderController : BaseController
{
    public OrderController(IUnitOfWork unitofWork, IMapper mapper, ICacheService cacheService) 
        : base(unitofWork, mapper, cacheService)
    { 
        _cached = "orders";
    }

    [HttpGet]
    public async Task<IActionResult> Get(string? search) //FIXME
    {
        bool cacheEnable = string.IsNullOrWhiteSpace(search);
        var cacheData = await GetCache<IEnumerable<OrderDto>>(_cached);

        if (cacheData is null)
        {
            var orders = await _unitofWork.Orders.AllAsync(search!);
            var _dto = _mapper.Map<List<OrderDto>>(orders);
            if (cacheEnable && _dto.Any())
                await SetCache(_cached, _dto);
            
            cacheData = _dto;
        }

        return Ok(new Pagination<OrderDto>(cacheData));
    }

    [HttpGet("[action]")]
    public IActionResult Pending()
    {
        var pending = _unitofWork.Orders.Find(x => x.Status == 0)
                                        .AsParallel()
                                        .OrderBy(x => x.AddedOn)
                                        .ToList();

        var _dto = _mapper.Map<List<OrderDetailsDto>>(pending);
        return Ok(_dto);
    }

    [HttpGet("[action]/{OrderNo}")]
    public async Task<IActionResult> Details(string orderNo)
    {
        var result = new Result<OrderDetailsDto>();

        string ky = $"{_cached}-{orderNo}";
        var data = await GetCache<OrderDetailsDto>(ky);

        if (data is null)
        {
            var order = _unitofWork.Orders.GetByNoAsync(orderNo);
            if (order is null)
            {
                result.Error = AddError(404, 
                    ErrorsMessage.Generic.NotFound,
                    ErrorsMessage.Generic.NullSet);

                return NotFound(result);
            }

            data = _mapper.Map<OrderDetailsDto>(order);
            await SetCache(ky, data);
        }

        result.Content = order;
        return Ok(result);
    }

    [HttpPost("[action]")] //TODO: configure implemetation
    public IActionResult Create(OrderModDto order)
    {
        var result = new Result<dynamic>();
        
        return Ok(order);
    }
}