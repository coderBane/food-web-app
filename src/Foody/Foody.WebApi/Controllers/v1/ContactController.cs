using Foody.Data.Services;
using Microsoft.AspNetCore.Mvc;


namespace Foody.WebApi.Controllers.v1
{
    public sealed class ContactController : BaseController
    {
        public ContactController(IUnitOfWork unitofWork, IMapper mapper, ICacheService cacheService)
            : base(unitofWork, mapper, cacheService)
        {
            _cached = "inquiries";
        }

        // GET: api/values
        [HttpGet]
        [Route("Inquiries")]
        [ProducesResponseType(200, Type = typeof(Pagination<Contact>))]
        public async Task<IActionResult> Get(string? search)
        {
            bool cacheEnable = string.IsNullOrWhiteSpace(search);

            var data = cacheEnable ? await GetCache<IEnumerable<Contact>>(_cached) : default;

            if (data is null)
            {
                data = await _unitofWork.Inquiries.AllAsync(search!);
                if (cacheEnable && data.Any())
                    await SetCache(_cached, data);
            }

            return Ok(new Pagination<Contact>(data));
        }

        // GET api/values/5
        [HttpGet("Inquiry/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<Contact>))]
        public async Task<IActionResult> Get(int id)
        {
            string key =  $"{_cached}-{id}";
            var result = new Result<Contact>();

            var cacheData = await GetCache<Contact>(key);
            if (cacheData is null)
            {
                var message = await _unitofWork.Inquiries.GetAsync(id);

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

            await _unitofWork.Inquiries.AddAsync(message);
            await _unitofWork.CommitAsync();
            await SetCache($"{_cached}-{message.Id}", message, _cached);

            result.Content = "Message Sent!";

            return Ok(result);
        }

        [HttpPost("Read/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> ToggleRead(int id)
        {
            var result = new Result<object>();
            bool marked = await _unitofWork.Inquiries.ToggleRead(id);

            if (!marked)
            {
                result.Error = AddError(404, ErrorsMessage.Generic.NotFound,
                                            ErrorsMessage.Generic.NullSet);
                    
                return NotFound(result);
            }
            await _unitofWork.CommitAsync();
            await DeleteCache($"{_cached}-{id}");

            return NoContent();
        }

        // [HttpPost("Reply/{email}")]
        // public async Task<IActionResult> Post(string email)
        // {
        //     // var mail = new EmailDto
        //     // {
        //     //     Subject = "Newsletter Subriction",
        //     //     From = "foody.support@example.com",
        //     //     To = email,
        //     //     Body = await System.IO.File.ReadAllTextAsync("wwwroot/html/signup.html"),
        //     // };

        //     // await _emailService.SendMail(mail);

        //     return Ok();
        // }
    }
}

