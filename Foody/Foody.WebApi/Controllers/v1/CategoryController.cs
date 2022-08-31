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
        [ProducesResponseType(200, Type = typeof(Pagination<CategoryDto>))]
        public async Task<IActionResult> Get()
        { 
            var categories = await _unitofWork.Categories.All(string.Empty);
            var _dto = _mapper.Map<List<CategoryDto>>(categories);

            return _unitofWork.Categories is not null ? Ok(new Pagination<CategoryDto>(_dto)) :
                Problem(ErrorsMessage.Generic.NullSet, statusCode: 404, type: ErrorsMessage.Generic.NotFound);
        }

        // GET: v1/Category/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Result<CategoryDetailDto>))]
        public async Task<IActionResult> Get(int id)
        {
            var result = new Result<CategoryDetailDto>();
            var category = await _unitofWork.Categories.Get(id);

            if (category is null)
            {
                result.Error = AddError(404,
                    ErrorsMessage.Generic.NotFound,
                    ErrorsMessage.Category.NotExist);

                return NotFound(result);
            }

            var _dto = _mapper.Map<CategoryDetailDto>(category);

            result.Content = _dto;
            return Ok(result);
        }

        // POST: v1/Category
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDetailDto))]
        public async Task<IActionResult> Post([FromForm] CategoryModDto categoryModDto)
        {
            var result = new Result<CategoryDetailDto>();
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
            result.Content = _dto;

            return CreatedAtAction(nameof(Get), new { id = category.Id}, result);
        }

        // PUT: v1/Category/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Put(int id, [FromForm] CategoryModDto categoryModDto)
        {
            var result = new Result<Type>();
            var category = await _unitofWork.Categories.Get(id);

            if (category is null)
            {
                result.Error = AddError(404,
                    ErrorsMessage.Generic.NotFound,
                    ErrorsMessage.Category.NotExist);

                return NotFound(result);
            }

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
                result.Error = AddError(404,
                    ErrorsMessage.Generic.NotFound,
                    ErrorsMessage.Category.NotExist);

                return NotFound(result);
            }

            return NoContent();
        }

        // DELETE v1/Category/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = new Result<Type>();
            try
            {
                await _unitofWork.Categories.Delete(id);
                await _unitofWork.CompleteAsync();
            }
            catch (NullReferenceException)
            {
                result.Error = AddError(404,
                    ErrorsMessage.Generic.NotFound,
                    ErrorsMessage.Category.NotExist);

                return NotFound(result);
            }
            catch (Exception)
            {
                result.Error = AddError(400,
                    ErrorsMessage.Generic.BadRequest,
                    ErrorsMessage.Generic.UnknownError);

                return BadRequest(result);
            }

            return NoContent();
        }
    }
}
