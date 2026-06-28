namespace Day1.OrderSystem.Models;

public class Order
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Customer Customer { get; init; }
    public required List<CartItem> Items { get; init; }
    public decimal Total { get; init; }
    public decimal Discount { get; init; }
    public decimal FinalAmount => Total - Discount;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
