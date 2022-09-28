using AutoMapper;
using Foody.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Foody.WebApi.Controllers.v1
{
    public sealed class ContactController : BaseController
    {
        public ContactController(IUnitofWork unitofWork, IMapper mapper) : base(unitofWork, mapper)
        {
        }

        // GET: api/values
        [HttpGet]
        [Route("Inquiries")]
        public IActionResult Get()
        {
            return Ok();
        }

        // GET api/values/5
        [HttpGet("Inquiry/{id}")]
        public IActionResult Get(int id)
        {
            return Ok();
        }

        // POST api/values
        [HttpPost]
        [Route("Inquire")]
        public void Post([FromBody] string value)
        {
        }
    }
}

