using Day1.Calculator;
using Xunit;

namespace Day1.Calculator.Tests;

// ─────────────────────────────────────────────────────────────────────────────
// This test class uses XUnit's NATIVE Assert API.
// ─────────────────────────────────────────────────────────────────────────────

public class CalculatorTests : IDisposable
{
    // Constructor = Arrange shared state (runs before every test)
    private readonly Calculator _calculator;

    public CalculatorTests()
    {
        _calculator = new Calculator();
    }

    // IDisposable.Dispose = Teardown (runs after every test)
    public void Dispose()
    {
        // Nothing to dispose here — but this is where you would
        // close files, release DB connections, etc.
    }

    // ── [Fact] ────────────────────────────────────────────────────────────────

    [Fact]
    public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
    {
        // Arrange
        int a = 3, b = 5;

        // Act
        int result = _calculator.Add(a, b);

        // Assert
        Assert.Equal(8, result);
    }

    [Fact]
    public void Add_NegativeAndPositive_ReturnsCorrectSum()
    {
        int result = _calculator.Add(-4, 6);
        Assert.Equal(2, result);
    }

    [Fact]
    public void Subtract_LargerFromSmaller_ReturnsNegativeResult()
    {
        int result = _calculator.Subtract(3, 10);
        Assert.Equal(-7, result);
    }

    [Fact]
    public void IsEven_EvenNumber_ReturnsTrue()
    {
        bool result = _calculator.IsEven(8);
        Assert.True(result);
    }

    [Fact]
    public void IsEven_OddNumber_ReturnsFalse()
    {
        bool result = _calculator.IsEven(7);
        Assert.False(result);
    }

    // ── [Theory] + [InlineData] ───────────────────────────────────────────────
    // Each [InlineData] row runs the test with different values.
    // XUnit generates a separate test entry for each row in Test Explorer.

    [Theory]
    [InlineData(10, 2, 5)]   // 10 / 2 = 5
    [InlineData(9,  3, 3)]   // 9  / 3 = 3
    [InlineData(0,  5, 0)]   // 0  / 5 = 0
    [InlineData(-8, 2, -4)]  // -8 / 2 = -4
    public void Divide_ValidDivisor_ReturnsCorrectQuotient(int a, int b, int expected)
    {
        int result = _calculator.Divide(a, b);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(100)]
    [InlineData(0)]
    [InlineData(-6)]
    public void IsEven_EvenValues_ReturnsTrue(int number)
    {
        Assert.True(_calculator.IsEven(number));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(99)]
    [InlineData(-7)]
    public void IsEven_OddValues_ReturnsFalse(int number)
    {
        Assert.False(_calculator.IsEven(number));
    }

    // ── Exception testing ──────────────────────────────────────────────────────
    // Assert.Throws<T> runs the lambda and verifies the exception type.

    [Fact]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        // The lambda is the "Act"; XUnit catches the exception for us.
        var ex = Assert.Throws<DivideByZeroException>(
            () => _calculator.Divide(10, 0));

        // Optionally assert the message
        Assert.Contains("zero", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    // ── [Trait] — categories & filtering ──────────────────────────────────────
    // dotnet test --filter "Category=Smoke" to run only smoke tests.

    [Fact]
    [Trait("Category", "Smoke")]
    public void Multiply_TwoNumbers_ReturnsProduct()
    {
        int result = _calculator.Multiply(4, 5);
        Assert.Equal(20, result);
    }

    // ── [Skip] — explicitly skipping a test ───────────────────────────────────

    [Fact(Skip = "Intentionally skipped — demonstrates [Skip] usage.")]
    public void PlaceholderTest_Skipped()
    {
        // This test will never run — it appears as 'Skipped' in Test Explorer.
        Assert.True(false, "This should never be reached.");
    }
}
