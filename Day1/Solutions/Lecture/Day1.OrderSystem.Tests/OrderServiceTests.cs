using Day1.OrderSystem.Models;
using Day1.OrderSystem.Services;
using Day1.OrderSystem.Tests.TestData;
using Xunit;

namespace Day1.OrderSystem.Tests;

// ─────────────────────────────────────────────────────────────────────────────
// Uses XUnit NATIVE Assert API — pre Fluent Assertions section.
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// IClassFixture shares one OrderService instance across all tests in this class.
/// Useful when creating the object is expensive (DB, HTTP, heavy init).
/// </summary>
public class OrderServiceFixture
{
    public OrderService Service { get; } = new OrderService();
}

public class OrderServiceTests : IClassFixture<OrderServiceFixture>, IDisposable
{
    private readonly OrderService _service;

    // Helpers re-created fresh for each test via constructor
    private readonly Customer _regular = new("Alice",  CustomerTier.Regular);
    private readonly Customer _silver  = new("Bob",    CustomerTier.Silver);
    private readonly Customer _gold    = new("Carol",  CustomerTier.Gold);

    public OrderServiceTests(OrderServiceFixture fixture)
    {
        _service = fixture.Service;
    }

    public void Dispose()
    {
        // Per-test teardown goes here
    }

    // ── [Fact]: PlaceOrder ─────────────────────────────────────────────────────

    [Fact]
    public void PlaceOrder_ValidCart_ReturnsConfirmedOrder()
    {
        // Arrange
        var cart = BuildCart(("Laptop Stand", 49.99m, 1));

        // Act
        var order = _service.PlaceOrder(cart, _regular);

        // Assert
        Assert.NotNull(order);
        Assert.Equal(OrderStatus.Confirmed, order.Status);
        Assert.Equal(_regular, order.Customer);
        Assert.Single(order.Items);
    }

    [Fact]
    public void PlaceOrder_BelowMinimumAmount_ThrowsInvalidOperationException()
    {
        var cart = BuildCart(("Pen", 2.00m, 1)); // Total = $2 < $10 minimum

        var ex = Assert.Throws<InvalidOperationException>(
            () => _service.PlaceOrder(cart, _regular));

        Assert.Contains("minimum", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void PlaceOrder_NullCart_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _service.PlaceOrder(null!, _regular));
    }

    [Fact]
    public void PlaceOrder_NullCustomer_ThrowsArgumentNullException()
    {
        var cart = BuildCart(("Item", 50m, 1));
        Assert.Throws<ArgumentNullException>(() => _service.PlaceOrder(cart, null!));
    }

    [Fact]
    public void PlaceOrder_GoldCustomer_AppliesDiscount()
    {
        var cart = BuildCart(("Item", 100m, 1)); // Total = $100

        var order = _service.PlaceOrder(cart, _gold);

        Assert.Equal(15m, order.Discount);       // 15% of 100
        Assert.Equal(85m, order.FinalAmount);
    }

    // ── [Theory] + [InlineData]: CalculateDiscount ────────────────────────────

    [Theory]
    [InlineData(CustomerTier.Regular, 100,  0)]
    [InlineData(CustomerTier.Silver,  100, 10)]
    [InlineData(CustomerTier.Gold,    100, 15)]
    [InlineData(CustomerTier.Gold,    200, 30)]
    [InlineData(CustomerTier.Silver,   50,  5)]
    public void CalculateDiscount_ByTier_ReturnsExpectedAmount(
        CustomerTier tier, decimal total, decimal expected)
    {
        var customer = new Customer("Test", tier);

        decimal discount = _service.CalculateDiscount(customer, total);

        Assert.Equal(expected, discount);
    }

    // ── [Theory] + [MemberData]: complex typed test data ──────────────────────

    [Theory]
    [MemberData(nameof(DiscountMemberData.Cases), MemberType = typeof(DiscountMemberData))]
    public void CalculateDiscount_MemberData_ReturnsExpectedAmount(
        Customer customer, decimal total, decimal expected)
    {
        decimal discount = _service.CalculateDiscount(customer, total);

        Assert.Equal(expected, discount);
    }

    // ── [Theory] + [ClassData]: data from a class ─────────────────────────────

    [Theory]
    [ClassData(typeof(CancelOrderClassData))]
    public void CancelOrder_ByStatus_ReturnsExpectedResult(
        OrderStatus status, bool expectedResult)
    {
        var order = BuildOrder(_regular, status);

        bool result = _service.CancelOrder(order);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CancelOrder_ShippedOrder_LeavesStatusUnchanged()
    {
        var order = BuildOrder(_regular, OrderStatus.Shipped);

        _service.CancelOrder(order);

        Assert.Equal(OrderStatus.Shipped, order.Status);
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private static Cart BuildCart(params (string name, decimal price, int qty)[] items)
    {
        var cart = new Cart();
        foreach (var (name, price, qty) in items)
            cart.Add(new CartItem(name, price, qty));
        return cart;
    }

    private static Order BuildOrder(Customer customer, OrderStatus status) =>
        new()
        {
            Customer = customer,
            Items    = [],
            Total    = 0m,
            Status   = status
        };
}
