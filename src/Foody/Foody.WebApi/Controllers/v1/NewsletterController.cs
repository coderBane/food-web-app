using AutoMapper;
using Foody.Data.Interfaces;
using Foody.Data.Services;
using Microsoft.AspNetCore.Mvc;


namespace Foody.WebApi.Controllers.v1
{
    public sealed class NewsletterController : BaseController
    {
        public NewsletterController(IUnitofWork unitofWork, IMapper mapper, ICacheService cacheService)
            : base(unitofWork, mapper, cacheService)
        {
            _cached = "subcribers";
        }

        // GET: api/values
        [HttpGet]
        [HttpHead]
        [Route("Subcribers")]
        [ProducesResponseType(200, Type = typeof(Pagination<Newsletter>))]
        public async Task<IActionResult> Get()
        {
            var cachedData = await GetCache<IEnumerable<Newsletter>>(_cached);
            if (cachedData is null)
            {
                cachedData = await _unitofWork.Subcribers.All();
                await SetCache(_cached, cachedData);
            }
            
            return Ok(new Pagination<Newsletter>(cachedData));
        }

        // POST api/values
        [HttpPost]
        [Route("Subcribe")]
        [ProducesResponseType(200, Type = typeof(Result<string>))]
        public async Task<IActionResult> Post([FromForm] NewsletterDto dto)
        {
            var result = new Result<string>();
            
            var subcriber = _mapper.Map<Newsletter>(dto);

            var invalid = ValidateModel(subcriber, dto, result);
            if (invalid is not null) return invalid;

            if (_unitofWork.Subcribers.Duplicate(subcriber.Email))
                return NoContent();

            await _unitofWork.Subcribers.Add(subcriber);
            await _unitofWork.CompleteAsync();
            await SetCache($"{subcriber.Id}", subcriber, _cached);
            
            result.Content = "Added to mailing list!";
            return Ok(result);
        }

        // DELETE api/values/5
        [HttpDelete("Unsubcribe/{email}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string email)
        {
            var result = new Result<dynamic>();
            var subcriber = _unitofWork.Subcribers
                .Find(s => s.Email == email)
                .AsParallel()
                .FirstOrDefault()?.Id;

            if (subcriber is null)
            {
                result.Error = AddError(404,
                    ErrorsMessage.Generic.NotFound,
                    ErrorsMessage.Newsletter.NotExist);

                return NotFound(result);
            }
           
            await _unitofWork.Subcribers.Delete(subcriber.Value);
            await _unitofWork.CompleteAsync();
            await DeleteCache(_cached);

            return NoContent();
        }
    }
}

