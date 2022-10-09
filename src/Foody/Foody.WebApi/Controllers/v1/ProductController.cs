using AutoMapper;
using Foody.Entities.DTOs;
using Foody.Entities.Models;
using Foody.Data.Services;
using Foody.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Foody.WebApi.Controllers.v1
{
    public class ProductController : BaseController
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
            var products = await _unitofWork.Products.All(search);
            var _dto = _mapper.Map<List<ProductDto>>(products);
 
            return _unitofWork.Products is not null ? Ok(new Pagination<ProductDto>(_dto)) :
                Problem("Entity set 'FoodyDbContext' is null.");
        }

        // GET v1/Product/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Result<ProductDetailDto>))]
        public async Task<IActionResult> Get(int id)
        {
            string key = $"{id}";
            var result = new Result<ProductDetailDto>();

            var cacheData = GetCache<ProductDetailDto>(key);
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
                SetCache(key, _dto);
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

            try
            {
                var product = _mapper.Map<Product>(productModDto);
                await Upload(product, productModDto.ImageUpload);

                var invalid = ValidateModel(product, productModDto, result);
                if (invalid is not null) return invalid;

                await _unitofWork.Products.Add(product);
                await _unitofWork.CompleteAsync();

                var _dto = _mapper.Map<ProductDetailDto>(product);

                result.Content = _dto;
                return CreatedAtAction(nameof(Get), new { id = product.Id }, result);
            }
            catch(DbUpdateException) when (_unitofWork.Products.Exists(productModDto.Name))
            {
                result.Error = AddError(404,
                        ErrorsMessage.Generic.NotFound,
                        ErrorsMessage.Product.Exists);

                return Conflict(result);
            }
            catch (Exception) { }

            result.Error = AddError(404,
                        ErrorsMessage.Generic.BadRequest,
                        ErrorsMessage.Generic.UnknownError);

            return BadRequest(result);
        }

        // PUT v1/Product/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Put(int id, [FromForm] ProductModDto productModDto)
        {
            var result = new Result<dynamic>();

            var product = await _unitofWork.Products.Get(id);
            if (product is null)
            {
                result.Error = AddError(404,
                        ErrorsMessage.Generic.NotFound,
                        ErrorsMessage.Product.NotExist);
                return NotFound();
            }

            try
            {
                _mapper.Map(productModDto, product);
                await Upload(product, productModDto.ImageUpload);

                var invalid = ValidateModel(product, productModDto, result);
                if (invalid is not null) return invalid;

                await _unitofWork.Products.Update(product);
                await _unitofWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException) when (!_unitofWork.Products.Exists(id))
            {
                result.Error = AddError(404,
                        ErrorsMessage.Generic.NotFound,
                        ErrorsMessage.Product.NotExist);

                return NotFound(result);
            }
            catch (DbUpdateException) when (_unitofWork.Products.Exists(productModDto.Name))
            {
                result.Error = AddError(404,
                        ErrorsMessage.Generic.NotFound,
                        ErrorsMessage.Product.NotExist);

                return Conflict(result);
            }
            catch (Exception)
            {
                result.Error = AddError(404,
                        ErrorsMessage.Generic.BadRequest,
                        ErrorsMessage.Generic.UnknownError);

                return BadRequest(result);
            }

            return NoContent();
        }

        // DELETE v1/Product/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = new Result<dynamic>();

            try
            {
                await _unitofWork.Products.Delete(id);
                await _unitofWork.CompleteAsync();
                DeleteCache($"{id}");
            }
            catch (NullReferenceException)
            {
                result.Error = AddError(404,
                    ErrorsMessage.Generic.NotFound,
                    ErrorsMessage.Product.NotExist);

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

