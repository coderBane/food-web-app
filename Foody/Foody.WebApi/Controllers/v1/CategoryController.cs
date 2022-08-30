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

        // GET: v1/Categories
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        public async Task<IActionResult> Get()
        { 
            var result = await _unitofWork.Categories.All(string.Empty);
            var _dto = from c in result
                      select _mapper.Map<CategoryDto>(c);

            return _unitofWork.Categories is not null ? Ok(_dto) :
                Problem($"Entity set '{nameof(Category)}' is null", statusCode: 500);
        }

        // GET: v1/Category/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CategoryDetailDto))]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _unitofWork.Categories.Get(id);
            var _dto = _mapper.Map<CategoryDetailDto>(result);

            return result is null ? NotFound() : Ok(_dto);
        }

        // POST: v1/Category
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDetailDto))]
        public async Task<IActionResult> Post([FromForm] CategoryModDto categoryModDto)
        {
            var category = _mapper.Map<Category>(categoryModDto);
            await Upload(category, categoryModDto.ImageUpload);

            ModelState.ClearValidationState(nameof(categoryModDto));
            if (!TryValidateModel(category))
            {
                return UnprocessableEntity(ModelState);
            }

            _unitofWork.Categories.Add(category);
            await _unitofWork.CompleteAsync();

            var _dto = _mapper.Map<CategoryDetailDto>(category);

            return CreatedAtAction(nameof(Get), new { id = category.Id}, _dto);
        }

        // PUT: v1/Category/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Put(int id, [FromForm] CategoryModDto categoryModDto)
        {
            var category = await _unitofWork.Categories.Get(id);

            if (category is null)
                return NotFound();

            _mapper.Map(categoryModDto, category);
            await Upload(category, categoryModDto.ImageUpload);

            ModelState.ClearValidationState(nameof(categoryModDto));
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

        // DELETE v1/Category/5
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
    }
}
