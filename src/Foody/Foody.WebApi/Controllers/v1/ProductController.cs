using AutoMapper;
using Foody.Data.Services;
using Foody.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Foody.WebApi.Controllers.v1
{
    public sealed class ProductController : BaseController
    {
        public ProductController(IUnitofWork unitofWork, IMapper mapper, ICacheService cacheService)
            : base(unitofWork, mapper, cacheService)
        {
            this._cached = "products";
        }

        // GET: v1/Product
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(200, Type = typeof(Pagination<ProductDto>))]
        public async Task<IActionResult> Get(string? search)
        {
            bool cacheEnable = string.IsNullOrEmpty(search);

            var cacheData = cacheEnable ? await GetCache<IEnumerable<ProductDto>>(_cached) : null;
            if (cacheData is null)
            {
                var products = await _unitofWork.Products.All(search);
                var _dto = _mapper.Map<List<ProductDto>>(products);
                cacheData = _dto;
                if (cacheEnable)
                    await SetCache(_cached, cacheData);
            }

            return Ok(new Pagination<ProductDto>(cacheData));
        }

        // GET v1/Product/ByCategory
        [HttpGet]
        [Route("ByCategory")]
        [ProducesResponseType(200 , Type = typeof(Result<Dictionary<string, List<ProdCategoryDto>>>))]
        public async Task<IActionResult> ByCategory()
        {
            var result = new Result<dynamic>();

            var dict = await _unitofWork.Products.ProducstByCategory();
            if (dict is null)
            {
                result.Error = AddError(500,
                    ErrorsMessage.Generic.NullSet,
                    ErrorsMessage.Generic.UnknownError);

                return StatusCode(500, result);
            }

            result.Content = dict;
            return Ok(result);
        }

        // GET v1/Product/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Result<ProductDetailDto>))]
        public async Task<IActionResult> Get(int id)
        {
            string key = $"{id}";
            var result = new Result<ProductDetailDto>();

            var cacheData = await GetCache<ProductDetailDto>(key);
            if (cacheData is null)
            {
                var product = await _unitofWork.Products.Get(id);

                if (product is null)
                {
                    result.Error = AddError(404,
                        ErrorsMessage.Generic.NotFound,
                        ErrorsMessage.Product.NotExist);

                    return NotFound(result);
                }

                var _dto = _mapper.Map<ProductDetailDto>(product);
                cacheData = _dto;
                await SetCache(key, _dto);
            }

            result.Content = cacheData;
            return Ok(result); 
        }

        // POST v1/Product
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Result<ProductDto>))]
        public async Task<IActionResult> Post([FromForm] ProductModDto productModDto)
        {
            var result = new Result<ProductDto>();

            if (!await _unitofWork.Products.Exists(productModDto.Name))
            {
                var product = _mapper.Map<Product>(productModDto);
                await Upload(product, productModDto.ImageUpload);

                var invalid = ValidateModel(product, productModDto, result);
                if (invalid is not null) return invalid;

                await _unitofWork.Products.Add(product);
                await _unitofWork.CompleteAsync();

                var _dto = _mapper.Map<ProductDto>(product);
                //await SetCache($"{_dto.Id}", _dto, _cached);

                result.Content = _dto;
                return CreatedAtAction(nameof(Get), new { id = product.Id }, result); 
            }

            result.Error = AddError(409,
                   ErrorsMessage.Generic.ValidationError,
                   ErrorsMessage.Product.Exists);

            return Conflict(result);
        }

        // PUT v1/Product/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Put(int id, [FromForm] ProductModDto productModDto)
        {
            var result = new Result<dynamic>();

            var product = await _unitofWork.Products.Get(id);
            if (product is null || product.Id != id)
            {
                result.Error = AddError(404,
                        ErrorsMessage.Generic.NotFound,
                        ErrorsMessage.Product.NotExist);

                return NotFound(result);
            }

            if (await _unitofWork.Categories.Exists(productModDto.Name) && productModDto.Name != product.Name)
            {
                result.Error = AddError(409,
                   ErrorsMessage.Generic.ValidationError,
                   ErrorsMessage.Product.Exists);

                return Conflict(result);
            }

            try
            {
                _mapper.Map(productModDto, product);
                await Upload(product, productModDto.ImageUpload);

                var invalid = ValidateModel(product, productModDto, result);
                if (invalid is not null) return invalid;

                await _unitofWork.Products.Update(product);
                await _unitofWork.CompleteAsync();
                //await SetCache($"{}", null, _cached);
            }
            catch (DbUpdateConcurrencyException) when (!_unitofWork.Products.Exists(id))
            {
                result.Error = AddError(404,
                        ErrorsMessage.Generic.NotFound,
                        ErrorsMessage.Product.NotExist);

                return NotFound(result);
            }

            return NoContent();
        }

        // DELETE v1/Product/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _unitofWork.Products.Delete(id);
                await _unitofWork.CompleteAsync();
                await DeleteCache($"{id}");
                await DeleteCache(_cached);
            }
            catch (NullReferenceException)
            {
                Result<dynamic> result = new();
                result.Error = AddError(404,
                    ErrorsMessage.Generic.NotFound,
                    ErrorsMessage.Product.NotExist);

                return NotFound(result);
            }

            return NoContent();
        }
    }
}

