using ProductService.Models;

namespace ProductService.Services;

public interface IProductService
{
    Task<ProductDto?> GetProductAsync(int productId);
}