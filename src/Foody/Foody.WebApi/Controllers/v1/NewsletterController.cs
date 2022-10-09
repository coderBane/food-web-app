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
            this._cached = "subcribers";
        }

        // GET: api/values
        [HttpGet]
        [HttpHead]
        [Route("Subcribers")]
        [ProducesResponseType(200, Type = typeof(Pagination<Newsletter>))]
        public async Task<IActionResult> Get()
        {
            var cachedData = GetCache<IEnumerable<Newsletter>>(_cached);

            if (cachedData is null)
            {
                cachedData = await _unitofWork.Subcribers.All(string.Empty);
                SetCache(_cached, cachedData);
            }

            return Ok(new Pagination<Newsletter>(cachedData));
        }

        // POST api/values
        [HttpPost]
        [Route("Subcribe")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Post([FromForm] NewsletterDto dto)
        {
            var result = new Result<dynamic>();
            try
            {
                var subcriber = _mapper.Map<Newsletter>(dto);

                var invalid = ValidateModel(subcriber, dto, result);
                if (invalid is not null) return invalid;

                if (_unitofWork.Subcribers.Duplicate(subcriber.Email))
                    return NoContent();

                await _unitofWork.Subcribers.Add(subcriber);
                await _unitofWork.CompleteAsync();
                SetCache($"{subcriber.Id}", subcriber, _cached);
            }
            catch (Exception)
            {
                result.Error = AddError(400,
                    ErrorsMessage.Generic.BadRequest,
                    ErrorsMessage.Generic.AddFailure);

                return BadRequest(result);
            }

            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("Unsubcribe/{email}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(string email)
        {
            var result = new Result<string>();
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

            try
            {
                await _unitofWork.Subcribers.Delete(subcriber.Value);
                await _unitofWork.CompleteAsync();
                DeleteCache(_cached);
            }
            catch (Exception)
            {
                result.Error = AddError(400,
                    ErrorsMessage.Generic.BadRequest,
                    ErrorsMessage.Generic.UnknownError);

                BadRequest(result);
            }

            return NoContent();
        }
    }
}

