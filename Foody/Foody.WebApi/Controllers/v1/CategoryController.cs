using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foody.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Foody.WebApi.Controllers.v1
{
    public class CategoryController : BaseController
    {
        public CategoryController(IUnitofWork unitofWork) : base(unitofWork)
        {
        }

        // GET: api/values
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> Get()
        {
            return _unitofWork.Categories is not null ? Ok(await _unitofWork.Categories.All()) :
                Problem("Entity set 'FoodyDbContext' is null");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

