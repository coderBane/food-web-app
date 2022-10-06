using AutoMapper;
using Foody.Entities.DTOs;
using Foody.Entities.Models;
using Foody.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Foody.Data.Services;

namespace Foody.WebApi.Controllers.v1
{
    public class ProductController : BaseController
    {
        public ProductController(IUnitofWork unitofWork, IMapper mapper, ICacheService cacheService)
            : base(unitofWork, mapper, cacheService)
        {
        }


        // GET: v1/Product
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(200, Type = typeof(ProductDto))]
        public async Task<IActionResult> Get(string? search)
        {
            var products = await _unitofWork.Products.All(search!);
            var _dto = _mapper.Map<List<ProductDto>>(products);
            return products is not null ? Ok(_dto) :
                Problem("Entity set 'FoodyDbContext' is null.");
        }

        // GET v1/Product/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ProductDetailDto))]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _unitofWork.Products.Get(id);
            var _dto = _mapper.Map<ProductDetailDto>(result);

            return result is not null ? Ok(_dto) : NotFound();
        }

        // POST v1/Product
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDetailDto))]
        public async Task<IActionResult> Post([FromForm] ProductModDto productModDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productModDto);
                //await Upload(product, productModDto.ImageUpload);

                ModelState.ClearValidationState(nameof(productModDto));
                if (!TryValidateModel(product))
                    return UnprocessableEntity(ModelState);

                _unitofWork.Products.Add(product);
                await _unitofWork.CompleteAsync();

                var _dto = _mapper.Map<ProductDetailDto>(product);
                return CreatedAtAction(nameof(Get), new { id = product.Id }, _dto);
            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }
        }

        // PUT v1/Product/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Put(int id, [FromForm] ProductModDto productModDto)
        {
            var product = await _unitofWork.Products.Get(id);

            if (product is null)
                return NotFound();

            try
            {
                _mapper.Map(productModDto, product);
                //await Upload(product, productModDto.ImageUpload);

                ModelState.ClearValidationState(nameof(productModDto));
                if (!TryValidateModel(product))
                    return UnprocessableEntity(ModelState);

                await _unitofWork.Products.Update(product);
                await _unitofWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException) when (!_unitofWork.Products.Exists(id))
            {
                return NotFound();
            }
            catch (Exception ex) { return BadRequest(ex.ToString()); }

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
            }
            catch (NullReferenceException) { return NotFound(); }
            catch (Exception) { return BadRequest(); }

            return NoContent();
        }
    }
}
