using EShop.Context;
using EShop.Data;
using Microsoft.EntityFrameworkCore;
using EShop.Dto.CategoryModel;
using EShop.Dto;
using EShop.Services.Interface;
using EShop.Repositries.Interface;
using EShop.Dto.ProductModel;
using Serilog;

namespace EShop.Services
{
    public class ProductService(IProductRepository _productRepository, CloudinaryService cloudinaryService) : IProductService
    {
        public async Task<BaseResponse<bool>> CreateAsync(CreateProductDto request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Creating product {Product}", request);

                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    SellingPrice = request.SellingPrice,
                    CostPrice = request.CostPrice,
                    StockQuantity = request.StockQuantiy,
                    CategoryId = request.CategoryId,
                    CreatedAt = DateTime.UtcNow,
                };

                if (request.ImageFile != null)
                {
                    var imageUrl = await cloudinaryService.UploadImageAsync(request.ImageFile, cancellationToken);
                    product.ImageUrl = imageUrl;
                }

                var result = await _productRepository.AddAsync(product, CancellationToken.None);

                if (!result)
                {
                    Log.Warning("Failed to create product {ProductName}", request.Name);
                    return BaseResponse<bool>.FailResponse("Failed to create Product");
                }

                Log.Information("Product {ProductName} created successfully", request.Name);
                return BaseResponse<bool>.SuccessResponse(true, "Product created successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating product {ProductName}", request.Name);
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateProductDto request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Updating product {ProductId}", id);

                var product = await _productRepository.GetProductByIdAsync(id, CancellationToken.None);
                if (product == null)
                {
                    Log.Warning("Product with ID {ProductId} Not found", id);
                    return BaseResponse<bool>.FailResponse("Product not found.");
                }
        
                product.Name = request.Name;
                product.Description = request.Description;
                product.SellingPrice = request.SellingPrice;
                product.CostPrice = request.CostPrice;
                product.StockQuantity = request.StockQuantiy;
                product.CategoryId = request.CategoryId;

                var result = await _productRepository.UpdateAsync(product, CancellationToken.None);

                if (!result)
                {
                    Log.Warning("Failed to update product {ProductName}", request.Name);
                    return BaseResponse<bool>.FailResponse("Failed to update product");
                }

                Log.Information("Product {ProductName} updated succesfully", request.Name);
                return BaseResponse<bool>.SuccessResponse(true, "Product updated successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An Error occured while updating product {ProductName}", request.Name);
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Deleting product {ProductId}", id);

                var product = await _productRepository.GetProductByIdAsync(id, CancellationToken.None);
                if (product == null)
                {
                    Log.Warning("Product with ID {ProductId} Not found", id);
                    return BaseResponse<bool>.FailResponse("Product not found.");
                }
                 
                var result = await _productRepository.DeleteAsync(product, CancellationToken.None);

                if (!result)
                {
                    Log.Warning("Failed to delete product {ProductId} ", id);
                    return BaseResponse<bool>.FailResponse("Failed to delete product");
                }
                    
                Log.Information("Product {ProductName} deleted successfully", product.Name);
                return BaseResponse<bool>.SuccessResponse(true, "Product deleted successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An Error occured while deleting product {ProductId}", id);
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching all products...");

                var products = await _productRepository.GetAllProductAsync(CancellationToken.None);

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

                Log.Information("Retrieved {Count} products successfully", productDtos.Count);
                return BaseResponse<IEnumerable<ProductDto>>.SuccessResponse(productDtos, "Product retrieved successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An Error occurred while fetching products");
                return BaseResponse<IEnumerable<ProductDto>>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching product by ID: {ProductId}", id);

                var product = await _productRepository.GetProductByIdAsync(id, CancellationToken.None);
                if (product == null)
                {
                    Log.Warning("Product with ID {ProductId} not found", id);
                    return BaseResponse<ProductDto>.FailResponse("Product not found");
                }

                var dto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.SellingPrice,
                    Category = product.Category?.Name ?? "Uncategorized"
                };

                Log.Information("Product {ProductName} retrieved successfully", dto.Name);
                return BaseResponse<ProductDto>.SuccessResponse(dto, "Product retrieved successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occured while fetching products with ID {ProductsId}", id);
                return BaseResponse<ProductDto>.FailResponse($"Error: {ex.Message}");
            }
        }
    }
}