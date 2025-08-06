namespace Shared.Contracts.Events;

public interface StockChecked
{
    Guid OrderId { get; }
    int ProductId { get; }
    bool IsInStock { get; }
    DateTime CheckedAt { get; }
}