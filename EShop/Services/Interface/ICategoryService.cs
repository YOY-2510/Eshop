using EShop.Data;
using EShop.Dto;
using EShop.Dto.CategoryModel;

namespace EShop.Services.Interface
{
    public interface ICategoryService
    {
        Task<BaseResponse<bool>> CreateAsync(Category category, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteAsync(Category category, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> UpdateAsync(Category category, CancellationToken cancellationToken);
        Task<BaseResponse<IEnumerable<CategoryDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<BaseResponse<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
