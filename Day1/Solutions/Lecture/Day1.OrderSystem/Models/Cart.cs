namespace Day1.OrderSystem.Models;

public class Cart
{
    private readonly List<CartItem> _items = new();

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

    public decimal Total => _items.Sum(i => i.LineTotal);

    public void Add(CartItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }
}
