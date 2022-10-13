using AutoMapper;
using Foody.Data.Interfaces;
using Foody.Data.Services;
using Microsoft.AspNetCore.Mvc;


namespace Foody.WebApi.Controllers.v1
{
    public sealed class ContactController : BaseController
    {
        public ContactController(IUnitofWork unitofWork, IMapper mapper, ICacheService cacheService)
            : base(unitofWork, mapper, cacheService)
        {
            this._cached = "inquiries";
        }

        // GET: api/values
        [HttpGet]
        [Route("Inquiries")]
        [ProducesResponseType(200, Type = typeof(Pagination<Contact>))]
        public async Task<IActionResult> Get(string? search)
        {
            bool cacheEnable = string.IsNullOrEmpty(search);

            var cacheData = cacheEnable ? await GetCache<IEnumerable<Contact>>(_cached) : null;
            if (cacheData is null)
            {
                cacheData = await _unitofWork.Contacts.All(search);
                if (cacheEnable)
                    await SetCache(_cached, cacheData);
            }

            return Ok(new Pagination<Contact>(cacheData));
        }

        // GET api/values/5
        [HttpGet("Inquiry/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<Contact>))]
        public async Task<IActionResult> Get(int id)
        {
            string key = $"{id}";
            var result = new Result<Contact>();

            var cacheData = await GetCache<Contact>(key);
            if (cacheData is null)
            {
                var message = await _unitofWork.Contacts.Get(id);

                if (message is null)
                {
                    result.Error = AddError(404,
                        ErrorsMessage.Generic.NotFound,
                        ErrorsMessage.Generic.NullSet);

                    return NotFound(result);
                }

                cacheData = message;
                await SetCache(key, message);
            }

            result.Content = cacheData;
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        [Route("Inquire")]
        [ProducesResponseType(200, Type = typeof(Result<string>))]
        public async Task<IActionResult> Post([FromForm] ContactDto dto)
        {
            var result = new Result<string>();

            var message = _mapper.Map<Contact>(dto);

            var invalid = ValidateModel(message, dto, result);
            if (invalid is not null) return invalid;

            await _unitofWork.Contacts.Add(message);
            await _unitofWork.CompleteAsync();
            await SetCache($"{message.Id}", message, _cached);

            result.Content = "Message Sent!";

            return Ok(result);
        }

        [HttpPost("Reply/{email}")]
        public IActionResult Post(string email)
        {
            return Ok();
        }
    }
}

