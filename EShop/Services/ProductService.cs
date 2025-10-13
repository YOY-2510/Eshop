using EShop.Context;
using EShop.Data;
using Microsoft.EntityFrameworkCore;
using EShop.Dto.CategoryModel;
using EShop.Dto;
using EShop.Services.Interface;
using EShop.Repositries.Interface;
using EShop.Dto.ProductModel;

namespace EShop.Services
{
    public class ProductService(IProductRepository productRepository, ILogger<ProductService> logger) : IProductService
    {
        public async Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateProductDto request, CancellationToken cancellationToken)
        {
            try
            {
                var product = await productRepository.GetProductByIdAsync(id, CancellationToken.None);
                if (product == null)
                    return BaseResponse<bool>.FailResponse("Product not found.");

                product.Name = request.Name;
                product.Description = request.Description;
                product.SellingPrice = request.SellingPrice;
                product.CostPrice = request.CostPrice;
                product.StockQuantity = request.StockQuantiy;
                product.CategoryId = request.CategoryId;

                var result = await productRepository.UpdateAsync(product, CancellationToken.None);

                if (!result)
                    return BaseResponse<bool>.FailResponse("Failed to update product");
                return BaseResponse<bool>.SuccessResponse(true, "Product updated successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await productRepository.GetProductByIdAsync(id, CancellationToken.None);
                if (product == null)
                    return BaseResponse<bool>.FailResponse("Product not found.");

                var result = await productRepository.DeleteAsync(product, CancellationToken.None);

                if (!result)
                    return BaseResponse<bool>.FailResponse("Failed to delete product");

                return BaseResponse<bool>.SuccessResponse(true, "Product deleted successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An Error occured while deleting product");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var products = await productRepository.GetAllProductAsync(CancellationToken.None);

                var productDtos = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.SellingPrice,
                    CostPrice = p.CostPrice,
                    Description = p.Description,
                    Category = p.Category != null ? p.Category.Name : "Notcategorized",
                    ExpiryDate = p.ExpiryDate,
                    DateCreated = p.CreatedAt,
                    StockQuantity = p.StockQuantity
                }).ToList();
                return BaseResponse<IEnumerable<ProductDto>>.SuccessResponse(productDtos, "Product retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An Error occurred while fetching products");
                return BaseResponse<IEnumerable<ProductDto>>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> CreateAsync(CreateProductDto request, CancellationToken cancellationToken)
        {
            try
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    SellingPrice = request.SellingPrice,
                    CostPrice = request.CostPrice,
                    StockQuantity = request.StockQuantiy,
                    CategoryId = request.CategoryId
                };
                var result = await productRepository.AddAsync(product, CancellationToken.None);

                if (!result)
                    return BaseResponse<bool>.FailResponse("Failed to create Product");

                return BaseResponse<bool>.SuccessResponse(true, "Product created successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await productRepository.GetProductByIdAsync(id, CancellationToken.None);
                if (product == null)
                    return BaseResponse<ProductDto>.FailResponse("Product not found");

                var dto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.SellingPrice,
                    Category = product.Category?.Name ?? "Uncategorized"
                };
                return BaseResponse<ProductDto>.SuccessResponse(dto, "Product retrieved successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while fetching products");
                return BaseResponse<ProductDto>.FailResponse($"Error: {ex.Message}");
            }
        }
    }
}