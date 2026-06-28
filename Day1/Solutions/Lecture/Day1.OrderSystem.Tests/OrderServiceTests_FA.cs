using Day1.OrderSystem.Models;
using Day1.OrderSystem.Services;
using FluentAssertions;
using Xunit;

namespace Day1.OrderSystem.Tests;

// ─────────────────────────────────────────────────────────────────────────────
// FLUENT ASSERTIONS EDITION
// The same tests as OrderServiceTests.cs — rewritten with FluentAssertions.
// Use this file side-by-side to demonstrate the readability improvement.
//
// Compare:
//   Assert.Equal(OrderStatus.Confirmed, order.Status);
//   order.Status.Should().Be(OrderStatus.Confirmed);
//
//   Assert.Throws<InvalidOperationException>(() => ...);
//   act.Should().Throw<InvalidOperationException>().WithMessage("*minimum*");
// ─────────────────────────────────────────────────────────────────────────────

public class OrderServiceTests_FA : IClassFixture<OrderServiceFixture>
{
    private readonly OrderService _service;

    private readonly Customer _regular = new("Alice", CustomerTier.Regular);
    private readonly Customer _gold    = new("Carol", CustomerTier.Gold);

    public OrderServiceTests_FA(OrderServiceFixture fixture)
    {
        _service = fixture.Service;
    }

    // ── PlaceOrder — using FluentAssertions ───────────────────────────────────

    [Fact]
    public void PlaceOrder_ValidCart_FA_ReturnsConfirmedOrder()
    {
        // Arrange
        var cart = BuildCart(("Laptop Stand", 49.99m, 1));

        // Act
        var order = _service.PlaceOrder(cart, _regular);

        // Assert — FA style
        order.Should().NotBeNull();
        order.Status.Should().Be(OrderStatus.Confirmed);
        order.Customer.Should().Be(_regular);
        order.Items.Should().HaveCount(1);
    }

    [Fact]
    public void PlaceOrder_BelowMinimum_FA_ThrowsWithMessage()
    {
        var cart = BuildCart(("Pen", 2.00m, 1));

        // Act wrapped in a lambda — FA captures and inspects the exception
        Action act = () => _service.PlaceOrder(cart, _regular);

        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("*minimum*");  // wildcard match on message
    }

    [Fact]
    public void PlaceOrder_GoldCustomer_FA_AppliesDiscountAndFinalAmount()
    {
        var cart = BuildCart(("Item", 100m, 1));

        var order = _service.PlaceOrder(cart, _gold);

        // Chain multiple assertions on related values
        order.Discount.Should().Be(15m);
        order.FinalAmount.Should().Be(85m);
    }

    // ── CalculateDiscount — .BeEquivalentTo() on an anonymous object ──────────

    [Theory]
    [InlineData(CustomerTier.Regular, 100,  0)]
    [InlineData(CustomerTier.Silver,  100, 10)]
    [InlineData(CustomerTier.Gold,    100, 15)]
    public void CalculateDiscount_FA_ReturnsExpectedAmount(
        CustomerTier tier, decimal total, decimal expected)
    {
        var customer = new Customer("Test", tier);

        decimal discount = _service.CalculateDiscount(customer, total);

        discount.Should().Be(expected);
    }

    // ── CancelOrder — chaining with .And ──────────────────────────────────────

    [Fact]
    public void CancelOrder_PendingOrder_FA_ReturnsTrueAndSetsCancelled()
    {
        var order = new Order
        {
            Customer = _regular,
            Items    = [],
            Total    = 0m,
            Status   = OrderStatus.Pending
        };

        bool result = _service.CancelOrder(order);

        result.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public void CancelOrder_ShippedOrder_FA_ReturnsFalseAndKeepsStatus()
    {
        var order = new Order
        {
            Customer = _regular,
            Items    = [],
            Total    = 0m,
            Status   = OrderStatus.Shipped
        };

        bool result = _service.CancelOrder(order);

        // Chain both assertions in one expression
        result.Should().BeFalse("shipped orders cannot be cancelled");
        order.Status.Should().Be(OrderStatus.Shipped);
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private static Cart BuildCart(params (string name, decimal price, int qty)[] items)
    {
        var cart = new Cart();
        foreach (var (name, price, qty) in items)
            cart.Add(new CartItem(name, price, qty));
        return cart;
    }
}
