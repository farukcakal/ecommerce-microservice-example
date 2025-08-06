using System.Text.Json;
using ProductService.Models;

namespace ProductService.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductService> _logger;

    public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ProductDto?> GetProductAsync(int productId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/products/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Stock API failed. Status: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ProductService error");
            return null;
        }
    }
}