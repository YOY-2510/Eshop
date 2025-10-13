using EShop.Context;
using EShop.Data;
using EShop.Repositries.Interface;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repositries
{
    public class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
    {
        public async Task<bool> AddAsync(Product product, CancellationToken cancellationToken)
        {
            await dbContext.AddAsync(product);
            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> DeleteAsync(Product product, CancellationToken cancellationToken)
        {
            dbContext.Remove(product);
            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Products.Include(p => p.Category).ToListAsync(cancellationToken);
        }

        async Task<Product?> IProductRepository.GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await dbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            dbContext.Update(product);
            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Products.ToListAsync(cancellationToken);
        }
    }
}
