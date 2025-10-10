using EShop.Context;
using EShop.Data;
using EShop.Repositries.Interface;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repositries
{
    public class CategoryRepository(ApplicationDbContext dbContext) : ICategoryRepository
    {
        public async Task<bool> AddAsync(Category category, CancellationToken cancellationToken)
        {
            await dbContext.Categories.AddAsync(category, cancellationToken);
            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> DeleteAsync(Category category, CancellationToken cancellationToken)
        {
            dbContext.Categories.Remove(category);
            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<Category>> GetAllProductAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Categories.ToListAsync();
        }

        public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Category product, CancellationToken cancellationToken)
        {
            dbContext.Categories.Update();
        }

        Task<bool> ICategoryRepository.UpdateAsync(Category product, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
