using EShop.Data;

namespace EShop.Repositries.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllProductAsync(CancellationToken cancellationToken);
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> AddAsync(Category product, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Category product, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Category category, CancellationToken cancellationToken);
    }
}
