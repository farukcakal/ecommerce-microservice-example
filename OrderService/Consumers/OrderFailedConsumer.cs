using MassTransit;
using Shared.Contracts.Events;

namespace OrderService.Consumers;

public class OrderFailedConsumer : IConsumer<OrderFailed>
{
    private readonly ILogger<OrderFailedConsumer> _logger;
    
    public OrderFailedConsumer(ILogger<OrderFailedConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<OrderFailed> context)
    {
        var message = context.Message;
        _logger.LogWarning("Order {OrderId} failed due to: {Reason}", message.OrderId, message.Reason);

        /*
         * burada sipariş iptal edilir.
         * logic e bağlı olarak db'ye ters kayıt atılabilir
         * diğer servislerin bilgilendirilmesi gerekiyorsa OrderCancelled gibi bir event kuyruğa fırlatılabilir
         */
        
        return Task.CompletedTask;
    }
}