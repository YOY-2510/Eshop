using Azure.Core;
using EShop.Context;
using EShop.Dto.ProductModel;
using EShop.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.CreateAsync(request, cancellationToken);

            if (!result.Status)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("get-all-products")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _productService.GetAllAsync( cancellationToken);

            if (!result.Status)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("get-product/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _productService.GetByIdAsync(id, cancellationToken);

            if (!result.Status)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("update-product/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateProductDto request, CancellationToken cancellationToken)
        {
            var result = await _productService.UpdateAsync(id, request, cancellationToken);

            if (!result.Status)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("delete-product/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _productService.DeleteAsync(id, cancellationToken);

            if (!result.Status)
                 return BadRequest(result);

            return Ok(result);
        }
    }
}
