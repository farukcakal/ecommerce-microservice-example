namespace OrderService.Models;

public class CreateOrderRequestDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}