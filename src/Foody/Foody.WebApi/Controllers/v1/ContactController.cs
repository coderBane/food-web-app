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
        public async Task<IActionResult> Get(string? search)
        {
            var cacheData = GetCache<IEnumerable<Contact>>(_cached);

            if (cacheData is null)
            {
                cacheData = await _unitofWork.Contacts.All(search);
                SetCache(_cached, cacheData);
            }

            return Ok(new Pagination<Contact>(cacheData));
        }

        // GET api/values/5
        [HttpGet("Inquiry/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            string key = $"{id}";
            var result = new Result<Contact>();

            var cacheData = GetCache<Contact>(key);
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
                SetCache(key, message, _cached);
            }

            result.Content = cacheData;
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        [Route("Inquire")]
        public async Task<IActionResult> Post([FromForm] ContactDto dto)
        {
            var result = new Result<dynamic>();

            try
            {
                var message = _mapper.Map<Contact>(dto);

                var invalid = ValidateModel(message, dto, result);
                if (invalid is not null) return invalid;

                await _unitofWork.Contacts.Add(message);
                await _unitofWork.CompleteAsync();
                SetCache($"{message.Id}", message);

                result.Content = "Message Sent!";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result.Error = AddError(400,
                    ErrorsMessage.Generic.BadRequest,
                    ErrorsMessage.Generic.UnknownError);

                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Reply/{email}")]
        public IActionResult Post(string email)
        {
            return Ok();
        }
    }
}

