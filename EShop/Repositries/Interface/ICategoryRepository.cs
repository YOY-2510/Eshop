using EShop.Data;

namespace EShop.Repositries.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllProductAsync(CancellationToken cancellationToken);
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> AddAsync(Category category, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Category category, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Category category, CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken);
    }
}
