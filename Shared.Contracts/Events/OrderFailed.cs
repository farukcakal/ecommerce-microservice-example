namespace Shared.Contracts.Events;

public interface OrderFailed
{
    Guid OrderId { get; }
    string Reason { get; }
}