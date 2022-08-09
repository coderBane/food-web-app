using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foody.Entities.DTOs;
using Foody.Entities.Models;
using Foody.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Foody.WebApi.Controllers.v1
{
    public class CategoryController : BaseController
    {
        public CategoryController(IUnitofWork unitofWork) : base(unitofWork)
        {
        }

        // GET: Categories
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> Get()
        { 
            var result = await _unitofWork.Categories.All();
            var dto = from c in result
                      select new CategoryDto()
                      {
                          Name = c.Name,
                          IsActive = c.IsActive
                      };

            return _unitofWork.Categories is not null ? Ok(dto) :
                Problem("Entity set 'FoodyDbContext' is null");
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CategoryDetailDto))]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _unitofWork.Categories.Get(id);

            return result is null ? NotFound() : Ok(new CategoryDetailDto
            {
                Name = result.Name,
                IsActive = result.IsActive,
                ImageUri = result.ImageUri,
                ImageData = result.ImageData,
                AddedOn = result.AddedOn,
                Updated = result.Updated
            });
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDetailDto))]
        public async Task<IActionResult> Post([FromForm] CategoryDto categoryDto, IFormFile? formFile)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                IsActive = categoryDto.IsActive,
            };

            Upload(ref category, formFile);
            if (!TryValidateModel(category, nameof(category)))
            {
                return BadRequest();
            }

            _unitofWork.Categories.Add(category);
            await _unitofWork.CompleteAsync();

            return CreatedAtAction(nameof(Get), new { id = category.Id},
            new CategoryDetailDto
            {
                Name = category.Name,
                IsActive = category.IsActive,
                ImageUri = category.ImageUri,
                ImageData = category.ImageData,
                AddedOn = category.AddedOn,
                Updated = category.Updated
            });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _unitofWork.Categories.Delete(id);
                await _unitofWork.CompleteAsync();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }

        private void Upload(ref Category category, IFormFile? file)
        {
            if (file is not null)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);

                if (ms.Length > 3145728)
                    ModelState.AddModelError("formFile", "Image should be 3MB or less");

                category.ImageUri = Guid.NewGuid() + "-" + file.FileName;
                category.ImageData = ms.ToArray();

                ms.Close();
                ms.Dispose();
            }
        }
    }
}

