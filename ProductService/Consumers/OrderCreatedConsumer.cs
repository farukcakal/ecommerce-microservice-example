using MassTransit;
using Shared.Contracts.Events;

namespace ProductService.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var message = context.Message;

        _logger.LogInformation("OrderCreated received: OrderId={OrderId}, ProductId={ProductId}", message.OrderId, message.ProductId);

        var isInStock = message.ProductId % 2 == 0;

        //keyword: compensation (revert işlemi)
        if (!isInStock)
        {
            await context.Publish<OrderFailed>(new
            {
                OrderId = message.OrderId,
                Reason = "Ürün stokta yok"
            });
            return;
        }
        
        await context.Publish<StockChecked>(new
        {
            OrderId = message.OrderId,
            ProductId = message.ProductId,
            IsInStock = isInStock,
            CheckedAt = DateTime.UtcNow
        });
        
        _logger.LogInformation("StockChecked published for OrderId={OrderId}, InStock={InStock}", message.OrderId, isInStock);
    }
}