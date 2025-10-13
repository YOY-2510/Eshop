using EShop.Dto;
using EShop.Dto.ProductModel;

namespace EShop.Services.Interface
{
    public interface IProductService
    {
        Task<BaseResponse<bool>> CreateAsync(CreateProductDto request, CancellationToken cancellationToken);   
        Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateProductDto request, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<BaseResponse<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<BaseResponse<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken);
    }
}
