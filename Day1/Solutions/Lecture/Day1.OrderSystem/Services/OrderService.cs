using Day1.OrderSystem.Models;

namespace Day1.OrderSystem.Services;

public class OrderService
{
    private const decimal MinimumOrderAmount = 10m;

    /// <summary>
    /// Places an order for the given cart and customer.
    /// Throws <see cref="InvalidOperationException"/> if cart total is below the minimum.
    /// </summary>
    public Order PlaceOrder(Cart cart, Customer customer)
    {
        ArgumentNullException.ThrowIfNull(cart);
        ArgumentNullException.ThrowIfNull(customer);

        if (cart.Total < MinimumOrderAmount)
            throw new InvalidOperationException(
                $"Order total {cart.Total:C} is below the minimum of {MinimumOrderAmount:C}.");

        var discount = CalculateDiscount(customer, cart.Total);

        return new Order
        {
            Customer = customer,
            Items = cart.Items.ToList(),
            Total = cart.Total,
            Discount = discount,
            Status = OrderStatus.Confirmed
        };
    }

    /// <summary>
    /// Returns the discount amount based on customer tier:
    /// Gold = 15%, Silver = 10%, Regular = 0%.
    /// </summary>
    public decimal CalculateDiscount(Customer customer, decimal total)
    {
        ArgumentNullException.ThrowIfNull(customer);

        return customer.Tier switch
        {
            CustomerTier.Gold   => Math.Round(total * 0.15m, 2),
            CustomerTier.Silver => Math.Round(total * 0.10m, 2),
            _                   => 0m
        };
    }

    /// <summary>
    /// Cancels an order. Returns false if the order is already shipped (cannot cancel).
    /// </summary>
    public bool CancelOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (order.Status == OrderStatus.Shipped)
            return false;

        order.Status = OrderStatus.Cancelled;
        return true;
    }
}
