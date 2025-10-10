using EShop.Dto;
using EShop.Dto.CategoryModel;

namespace EShop.Services.Interface
{
    public interface ICategoryService
    {
        Task<BaseResponse<bool>> CreateAsync(CreateCategoryDto request);
        Task<BaseResponse<bool>> DeleteAsync(Guid id);
        Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateCategoryDto request);
    }
}
