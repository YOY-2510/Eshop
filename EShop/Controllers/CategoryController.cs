using EShop.Dto.CategoryModel;
using EShop.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost("create-category")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto request, CancellationToken cancellationToken)
        {
            var result = await _categoryService.CreateAsync(new Data.Category
            {
                Name = request.Name,
                Description = request.Description,
            }, cancellationToken);

            if (!result.Status)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetAllAsync(cancellationToken);

            if (!result.Status)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("Get-Category/{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetByIdAsync(id, cancellationToken);

            if (!result.Status)
                return NotFound(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-category/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateCategoryDto request, CancellationToken cancellationToken)
        {
            var category = new Data.Category()
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
            };

            var result = await _categoryService.UpdateAsync(category, cancellationToken);

            if (!result.Status) 
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-category/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var category = new Data.Category { Id = id };
            var result = await _categoryService.DeleteAsync(category, cancellationToken);

                if (!result.Status)
                     return BadRequest(result);

                return Ok(result);
        }
    }
}
