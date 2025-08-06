using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Services;
using Shared.Contracts.Events;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IPublishEndpoint _publishEndpoint;
        public ProductController(IProductService productService, IPublishEndpoint publishEndpoint)
        {
            _productService = productService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _productService.GetProductAsync(productId);

            if (product == null) return StatusCode(503, "Stok bilgisine ulaşılamadı.");

            await _publishEndpoint.Publish<OrderCreated>(new
            {
                OrderId = Guid.NewGuid(),
                ProductId = productId,
                Quantity = 1
            });
            
            return Ok(product);
        }
        
    }
}
