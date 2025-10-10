using EShop.Context;
using EShop.Data;
using EShop.Dto.CategoryModel;
using EShop.Dto;
using EShop.Repositries;
using EShop.Services.Interface;
using EShop.Repositries.Interface;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services
{
    public class CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger) : ICategoryService
    {
        public async Task<Dto.BaseResponse<bool>> CreateAsync(CreateCategoryDto request)
        {
            try
            {
                var category = new Data.Category
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                };

                var result = await categoryRepository.AddAsync(category, CancellationToken.None);

                if (!result)
                    return BaseResponse<bool>.FailResponse("Failed to create.");
                return BaseResponse<bool>.SuccessResponse(true, "Category created successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }    
        }

        public Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateCategoryDto request)
        {
            throw new NotImplementedException();
        }
    }
}
