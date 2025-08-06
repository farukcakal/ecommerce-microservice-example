using MassTransit;
using Shared.Contracts.Events;

namespace ShippingService.Consumers;


public class StockCheckedConsumer : IConsumer<StockChecked>
{
    private readonly ILogger<StockCheckedConsumer> _logger;

    public StockCheckedConsumer(ILogger<StockCheckedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<StockChecked> context)
    {
        var message = context.Message;

        if (!message.IsInStock)
        {
            _logger.LogWarning("OrderId={OrderId} - Ürün stokta yok, gönderim iptal edildi", message.OrderId);
            return;
        }
        
        //kargo ile alakalı db işlemleri burada yapılacak
        _logger.LogInformation("OrderId={OrderId} için kargo işlemi başlatıldı. Ürün gönderime hazır.", message.OrderId);
        await Task.CompletedTask;
    }
}