using EShop.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShop.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProductController(ApplicationDbContext DbContext)
        {
            DbContext = DbContext;
        }
        public async Task<ActionResult<IEnumerable<ProductController>>> GetProducts()
        {
            return await DbContext.Product
                .Include(p => p.Category)
                .ToListAsync();
        }
        public async Task<ActionResult<ProductController>> GetProducts(int id)
        {
            var product = await DbContext.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(product => p.ProductID == id);
            if (product == null)
                return NotFound();

            return product;
        }
        public async Task<ActionResult<ProductController>> AddProducts(ProductController product)
        {
            DbContext.Products.Add(product);
            await DbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducts), new { id = product.ProductID }, product);
        }
        public async Task<IActionResult> UpdateProduct(int id, ProductController product)
        {
            if (id != product.ProductID)
                return BadRequest();
            DbContext.Entry(product).State = EntityState.Modified;

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DbContext.Products.Any(p => p.ProductID == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }     
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await DbContext.Products
        }
    }
}
