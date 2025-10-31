using EShop.Data;
using EShop.Dto;
using EShop.Dto.CategoryModel;
using EShop.Repositries;
using EShop.Repositries.Interface;
using EShop.Services.Interface;
using Serilog;

namespace EShop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<BaseResponse<bool>> CreateAsync(Category category, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Creating category {Category}", category);

                var result = await _categoryRepository.AddAsync(category, cancellationToken);

                if (!result)
                {
                    Log.Warning("Failed to create category with Name: {CategoryName}", category.Name);
                    return BaseResponse<bool>.FailResponse("Failed to create category.");
                }

                Log.Information("Category created successfully: {CategoryName}", category.Name);
                return BaseResponse<bool>.SuccessResponse(true, "Category created successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating category {CategoryName}", category.Name);
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Category category, CancellationToken cancellationToken)
        {
            Log.Information("Deleting category {Category}", category);

            try
            {
                var result = await _categoryRepository.DeleteAsync(category, cancellationToken);

                if (!result)
                {
                    Log.Warning("Failed to delete category with Name: {CategoryName}", category.Name);
                    return BaseResponse<bool>.FailResponse("Failed to delete category.");
                }

                Log.Information("Category deleted successfully: {CategoryName}", category.Name);
                return BaseResponse<bool>.SuccessResponse(true, "Category deleted successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting category: {CategoryName}", category.Name);
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching all categories...");

                var categories = await _categoryRepository.GetAllCategoriesAsync(cancellationToken);

                var categoryDtos = categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToList();

                Log.Information("Retrieved {Count} categories successfully.", categoryDtos.Count);
                return BaseResponse<IEnumerable<CategoryDto>>.SuccessResponse(categoryDtos, "Categories retrieved successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching categories");
                return BaseResponse<IEnumerable<CategoryDto>>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching category by ID: {CategoryId}", id);

                var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

                if (category == null)
                {
                    Log.Warning("Category wiyh ID {CategoryId} not found.", id);
                    return BaseResponse<CategoryDto>.FailResponse("Category not found.");
                }
                
                var categoryDto = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                Log.Information("Category retrieved successfully: {CategoryName}", category.Name);
                return BaseResponse<CategoryDto>.SuccessResponse(categoryDto, "Category retrieved successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching category by ID {CategoryId}", id);
                return BaseResponse<CategoryDto>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> UpdateAsync(Category category, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Attempt to update category: {Category}", category);

                var result = await _categoryRepository.UpdateAsync(category, cancellationToken);

                if (!result)
                {
                    Log.Warning("Failed to update category: {CategoryName}", category.Name);
                    return BaseResponse<bool>.FailResponse("Failed to update category.");
                }

                Log.Information("Category updated successfully: {CategoryName}", category.Name);
                return BaseResponse<bool>.SuccessResponse(true, "Category updated successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating category: {CategoryName}", category.Name);
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

    }
}
