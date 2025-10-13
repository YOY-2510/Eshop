using EShop.Data;
using EShop.Dto;
using EShop.Dto.CategoryModel;
using EShop.Repositries.Interface;
using EShop.Services.Interface;

namespace EShop.Services
{
    public class CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger) : ICategoryService
    {
        public async Task<BaseResponse<bool>> CreateAsync(Category category, CancellationToken cancellationToken)
        {
            try
            {
                var result = await categoryRepository.AddAsync(category, cancellationToken);

                if (!result)
                    return BaseResponse<bool>.FailResponse("Failed to create category.");

                return BaseResponse<bool>.SuccessResponse(true, "Category created successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating category");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Category category, CancellationToken cancellationToken)
        {
            try
            {
                var result = await categoryRepository.DeleteAsync(category, cancellationToken);

                if (!result)
                    return BaseResponse<bool>.FailResponse("Failed to delete category.");

                return BaseResponse<bool>.SuccessResponse(true, "Category deleted successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting category");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var categories = await categoryRepository.GetAllCategoriesAsync(cancellationToken);

                var categoryDtos = categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                });
                return BaseResponse<IEnumerable<CategoryDto>>.SuccessResponse(categoryDtos, "Categories retrieved successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching categories");
                return BaseResponse<IEnumerable<CategoryDto>>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var category = await categoryRepository.GetByIdAsync(id, cancellationToken);

                if (category == null)
                    return BaseResponse<CategoryDto>.FailResponse("Category not found.");

                var categoryDto = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                return BaseResponse<CategoryDto>.SuccessResponse(categoryDto, "Category retrieved successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching category by ID");
                return BaseResponse<CategoryDto>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> UpdateAsync(Category category, CancellationToken cancellationToken)
        {
            try
            {
                var result = await categoryRepository.UpdateAsync(category, cancellationToken);

                if (!result)
                    return BaseResponse<bool>.FailResponse("Failed to update category.");

                return BaseResponse<bool>.SuccessResponse(true, "Category updated successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating category");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        Task<BaseResponse<IEnumerable<CategoryDto>>> ICategoryService.GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
