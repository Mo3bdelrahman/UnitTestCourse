namespace Day1.OrderSystem.Models;

public record CartItem(string ProductName, decimal UnitPrice, int Quantity)
{
    public decimal LineTotal => UnitPrice * Quantity;
}
