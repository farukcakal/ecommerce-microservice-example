namespace Shared.Contracts.Events;

public interface OrderCreated
{
    Guid  OrderId { get; }
    int ProductId { get; }
    int Quantity { get; }
    DateTime CreatedAt { get; }
}