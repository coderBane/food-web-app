using AutoMapper;
using Foody.Entities.DTOs;
using Foody.Entities.Models;
using Foody.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foody.WebApi.Controllers.v1
{
    public class CategoryController : BaseController
    {
        public CategoryController(IUnitofWork unitofWork, IMapper mapper) : base(unitofWork, mapper)
        {
        }

        // GET: v/Categories
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        public async Task<IActionResult> Get()
        { 
            var result = await _unitofWork.Categories.All();
            var _dto = from c in result
                      select _mapper.Map<CategoryDto>(c);

            return _unitofWork.Categories is not null ? Ok(_dto) :
                Problem("Entity set 'FoodyDbContext' is null");
        }

        // GET: v/Category/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CategoryDetailDto))]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _unitofWork.Categories.Get(id);
            var _dto = _mapper.Map<CategoryDetailDto>(result);

            return result is null ? NotFound() : Ok(_dto);
        }

        // POST: v/Category
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDetailDto))]
        public async Task<IActionResult> Post([FromForm] CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            Upload(ref category, categoryDto.ImageUpload);

            ModelState.ClearValidationState(nameof(categoryDto));
            if (!TryValidateModel(category))
            {
                return UnprocessableEntity(ModelState);
            }

            _unitofWork.Categories.Add(category);
            await _unitofWork.CompleteAsync();

            var _dto = _mapper.Map<CategoryDetailDto>(category);

            return CreatedAtAction(nameof(Get), new { id = category.Id}, _dto);
        }

        // PUT: v/Category/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Put(int id, [FromForm] CategoryDto categoryDto)
        {
            var category = await _unitofWork.Categories.Get(id);

            if (category is null)
                return NotFound();

            _mapper.Map(categoryDto, category);
            Upload(ref category, categoryDto.ImageUpload);

            ModelState.ClearValidationState(nameof(categoryDto));
            try
            {
                if (!TryValidateModel(category))
                    return UnprocessableEntity(ModelState);

                await _unitofWork.Categories.Update(category);
                await _unitofWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException) when (!_unitofWork.Categories.Exists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE v/Category/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
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

        private static void Upload(ref Category category, IFormFile? file)
        {
            if (file is not null)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);

                category.ImageUri = Guid.NewGuid() + "-" + file.FileName;
                category.ImageData = ms.ToArray();

                ms.Close();
                ms.Dispose();
            }
        }
    }
}
