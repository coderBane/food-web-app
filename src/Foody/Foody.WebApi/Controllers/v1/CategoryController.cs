﻿using AutoMapper;
using Foody.Data.Services;
using Foody.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Foody.WebApi.Controllers.v1
{
    public sealed class CategoryController : BaseController
    {
        public CategoryController(IUnitofWork unitofWork, IMapper mapper, ICacheService cacheService)
            : base(unitofWork, mapper, cacheService)
        {
            this._cached = "categories";
        }

        // GET: v1/Categories
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(200, Type = typeof(Pagination<CategoryDto>))]
        public async Task<IActionResult> Get()
        {
            var cacheData = await GetCache<IEnumerable<CategoryDto>>(_cached);

            if (cacheData is null)
            {
                var categories = await _unitofWork.Categories.All();
                var _dto = _mapper.Map<List<CategoryDto>>(categories);
                cacheData = _dto;
                await SetCache(_cached, cacheData);
            }

            return _unitofWork.Categories is not null ? Ok(new Pagination<CategoryDto>(cacheData)) :
                Problem(ErrorsMessage.Generic.NullSet, statusCode: 500, type: "Server Error");
        }

        // GET: v1/Category/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Result<CategoryDetailDto>))]
        public async Task<IActionResult> Get(int id)
        {
            string key = $"{id}";
            var result = new Result<CategoryDetailDto>();

            var cacheData = await GetCache<CategoryDetailDto>(key);
            if (cacheData is null)
            {
                var category = await _unitofWork.Categories.Get(id);
                if (category is null)
                {
                    result.Error = AddError(404,
                        ErrorsMessage.Generic.NotFound,
                        ErrorsMessage.Category.NotExist);

                    return NotFound(result);
                }
                var _dto = _mapper.Map<CategoryDetailDto>(category);
                cacheData = _dto;
                await SetCache(key, cacheData);
            }

            result.Content = cacheData;
            return Ok(result);
        }

        // POST: v1/Category
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Result<CategoryDto>))]
        public async Task<IActionResult> Post([FromForm] CategoryModDto categoryModDto)
        {
            var result = new Result<CategoryDto>();

            if (await _unitofWork.Categories.Exists(categoryModDto.Name))
            {
                result.Error = AddError(409,
                   ErrorsMessage.Generic.ValidationError,
                   ErrorsMessage.Category.Exists);

                return Conflict(result);
            }

            var category = _mapper.Map<Category>(categoryModDto);
            await Upload(category, categoryModDto.ImageUpload);

            var invalid = ValidateModel(category, categoryModDto, result);
            if (invalid is not null) return invalid;

            await _unitofWork.Categories.Add(category);
            await _unitofWork.CompleteAsync();
            //await SetCache($"{category.Id}", category, _cached);

            var _dto = _mapper.Map<CategoryDto>(category);
            result.Content = _dto;

            return CreatedAtAction(nameof(Get), new { id = category.Id }, result);
        }

        // PUT: v1/Category/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Put(int id, [FromForm] CategoryModDto categoryModDto)
        {
            var result = new Result<dynamic>();

            var category = await _unitofWork.Categories.Get(id);
            if (category is null || category.Id != id)
            {
                result.Error = AddError(404,
                    ErrorsMessage.Generic.NotFound,
                    ErrorsMessage.Category.NotExist);

                return NotFound(result);
            }

            if (await _unitofWork.Categories.Exists(categoryModDto.Name)
                && (category.Name != categoryModDto.Name))
            {
                result.Error = AddError(409,
                   ErrorsMessage.Generic.ValidationError,
                   ErrorsMessage.Category.Exists);

                return Conflict(result);
            }

            try
            {
                _mapper.Map(categoryModDto, category);
                await Upload(category, categoryModDto.ImageUpload);

                var invalid = ValidateModel(category, categoryModDto, result);
                if (invalid is not null) return invalid;

                await _unitofWork.Categories.Update(category);
                await _unitofWork.CompleteAsync();
                //await SetCache($"{category.Id}", category, _cached);
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
            try
            {
                await _unitofWork.Categories.Delete(id);
                await _unitofWork.CompleteAsync();
                await DeleteCache($"{id}");
                await DeleteCache(_cached);
            }
            catch (NullReferenceException)
            {
                Result<dynamic> result = new();
                result.Error = AddError(404,
                    ErrorsMessage.Generic.NotFound,
                    ErrorsMessage.Category.NotExist);

                return NotFound(result);
            }

            return NoContent();
        }
    }
}

