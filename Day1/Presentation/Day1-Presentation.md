---
marp: true
theme: default
paginate: true
style: |
  section {
    font-size: 1.15rem;
  }
  section.lead h1 {
    font-size: 2.4rem;
  }
  code {
    font-size: 0.85rem;
  }
---

<!-- _class: lead -->

# Day 1
## XUnit Fundamentals & Fluent Assertions

**.NET 9 · XUnit v3 · FluentAssertions 7**

---

## What is Unit Testing?

- Tests **one unit** of logic in isolation
- Fast — runs in milliseconds
- Deterministic — same result every run
- No database, network, or file system
- Developer writes and owns the tests

> A unit is typically a **single class or method**

> 📂 Ref: `Day1.Calculator/Calculator.cs`

---

## Why Unit Testing?

| Without Tests | With Tests |
|---|---|
| Fear of changing code | Refactor with confidence |
| Bugs found in production | Bugs caught at commit time |
| Manual verification loops | Automated regression net |
| Long debugging sessions | Pinpoint failures instantly |

> Testing is **documentation that verifies itself**

---

## Testing Pyramid

```
        ▲
       /E2E\         slow, expensive, few
      /──────\
     /Integration\   medium speed, some
    /──────────────\
   /   Unit Tests   \ fast, cheap, many
  /──────────────────\
```

- **Unit** — isolated, mock dependencies
- **Integration** — tests real collaborators
- **E2E** — tests the full system end-to-end

> Focus of today: **Unit Tests**

---

## The AAA Pattern

Every test follows three steps:

```csharp
[Fact]
public void Add_TwoNumbers_ReturnsSum()
{
    // Arrange — set up inputs and dependencies
    var calculator = new Calculator();
    int a = 3, b = 5;

    // Act — call the code under test
    int result = calculator.Add(a, b);

    // Assert — verify the outcome
    Assert.Equal(8, result);
}
```

> **Arrange · Act · Assert** — keep each step clear and separate

> 📂 Ref: `Day1.Calculator.Tests/CalculatorTests.cs`

---

## Naming Conventions

> **MethodName\_Scenario\_ExpectedResult**

```csharp
// ✅ Clear and descriptive
Divide_ByZero_ThrowsDivideByZeroException
Deposit_ValidAmount_IncreasesBalance
PlaceOrder_BelowMinimum_ThrowsInvalidOperationException

// ❌ Vague — tells you nothing
Test1
CalculatorTest
TestAdd
```

- Name = mini-spec of the test
- Failing test name reveals the bug immediately
- No need to read the body to understand intent

---

## XUnit: `[Fact]`

The simplest test — runs once, no parameters.

```csharp
[Fact]
public void IsEven_EvenNumber_ReturnsTrue()
{
    // Arrange
    var calculator = new Calculator();

    // Act
    bool result = calculator.IsEven(8);

    // Assert
    Assert.True(result);
    Assert.Equal(8, 8);        // value equality
    Assert.NotNull(calculator); // reference not null
}
```

- One `[Fact]` = one test entry in Test Explorer
- Method must be `public void` or `public async Task`

> 📂 Ref: `Day1.Calculator.Tests/CalculatorTests.cs` → `IsEven_EvenNumber_ReturnsTrue`

---

## XUnit: `[Theory]` + `[InlineData]`

Run the **same test** with multiple input sets.

```csharp
[Theory]
[InlineData(10, 2, 5)]   // 10 / 2 = 5
[InlineData(9,  3, 3)]   // 9  / 3 = 3
[InlineData(0,  5, 0)]   // 0  / 5 = 0
public void Divide_ValidDivisor_ReturnsCorrectQuotient(
    int a, int b, int expected)
{
    int result = _calculator.Divide(a, b);
    Assert.Equal(expected, result);
}
```

- Each `[InlineData]` row = separate test entry
- `[InlineData]` values must be **compile-time constants**
- Use when data fits on one line

> 📂 Ref: `Day1.Calculator.Tests/CalculatorTests.cs` → `Divide_ValidDivisor_ReturnsCorrectQuotient`

---

## XUnit: `[MemberData]` + `[ClassData]`

For **complex or typed** test data that doesn't fit `[InlineData]`.

```csharp
// MemberData — static property on any class
[Theory]
[MemberData(nameof(DiscountMemberData.Cases),
            MemberType = typeof(DiscountMemberData))]
public void CalculateDiscount_MemberData(
    Customer customer, decimal total, decimal expected)
{ ... }

// ClassData — IEnumerable<object[]> class
[Theory]
[ClassData(typeof(CancelOrderClassData))]
public void CancelOrder_ByStatus(OrderStatus status, bool expected)
{ ... }
```

- `[MemberData]` — data is a `static` property or method
- `[ClassData]` — data comes from a separate `IEnumerable` class

> 📂 Ref: `Day1.OrderSystem.Tests/TestData/`

---

## XUnit: Exception Testing

Use `Assert.Throws<T>` to verify an exception is raised.

```csharp
[Fact]
public void Divide_ByZero_ThrowsDivideByZeroException()
{
    // Assert.Throws runs the lambda and returns the exception
    var ex = Assert.Throws<DivideByZeroException>(
        () => _calculator.Divide(10, 0));

    // Optionally assert the message
    Assert.Contains("zero", ex.Message,
        StringComparison.OrdinalIgnoreCase);
}

// For async methods:
var ex = await Assert.ThrowsAsync<InvalidOperationException>(
    async () => await service.ProcessAsync());
```

> 📂 Ref: `Day1.Calculator.Tests/CalculatorTests.cs` → `Divide_ByZero_ThrowsDivideByZeroException`

---

## XUnit: Setup & Teardown

XUnit uses **constructor** (setup) and **IDisposable** (teardown) — no `[SetUp]` attributes.

```csharp
public class CalculatorTests : IDisposable
{
    // Constructor — runs BEFORE each test
    private readonly Calculator _calculator;
    public CalculatorTests()
    {
        _calculator = new Calculator();
    }

    // IDisposable.Dispose — runs AFTER each test
    public void Dispose()
    {
        // release resources: files, connections, etc.
    }

    [Fact]
    public void Add_TwoNumbers_ReturnsSum() { ... }
}
```

> XUnit creates a **new instance** of the test class per test method

> 📂 Ref: `Day1.Calculator.Tests/CalculatorTests.cs`

---

## XUnit: `[Trait]` & `[Skip]`

**`[Trait]`** — tag tests for filtering and reporting.

```csharp
[Fact]
[Trait("Category", "Smoke")]
[Trait("Feature", "Calculator")]
public void Multiply_TwoNumbers_ReturnsProduct() { ... }
```

```bash
dotnet test --filter "Category=Smoke"
```

**`[Skip]`** — disable a test with a reason.

```csharp
[Fact(Skip = "Pending business rule clarification")]
public void SomeTest_Skipped() { ... }
```

> Skipped tests appear in Test Explorer — they don't fail, but they're visible

> 📂 Ref: `Day1.Calculator.Tests/CalculatorTests.cs` → `PlaceholderTest_Skipped`

---

<!-- _class: lead -->

# Fluent Assertions

## A Better Way to Write Assertions

---

## Why Fluent Assertions?

Native XUnit assertions have **readability issues**:

```csharp
// Native XUnit — parameter order confusion
Assert.Equal(expected, actual);   // which is which?
Assert.NotNull(result);
Assert.True(result.Items.Count == 3);
```

Fluent Assertions reads like **English**:

```csharp
// FluentAssertions — natural left-to-right reading
result.Should().NotBeNull();
result.Items.Should().HaveCount(3);
order.Status.Should().Be(OrderStatus.Confirmed);
```

**Better error messages** — shows actual vs. expected clearly

---

## Installing Fluent Assertions

```bash
dotnet add package FluentAssertions
```

Add `using` in your test file:

```csharp
using FluentAssertions;
```

That's it. The `.Should()` extension method is available on **any type**.

```csharp
// Works on any type
42.Should().BeGreaterThan(10);
"hello".Should().StartWith("he");
someList.Should().NotBeEmpty();
```

> 📂 Already configured in all `.Tests.csproj` files in this course

---

## FA: Side-by-Side Comparison

```csharp
// ── Native Assert ──────────────────────────────────
Assert.Equal(OrderStatus.Confirmed, order.Status);
Assert.NotNull(order);
Assert.Equal(15m, order.Discount);
Assert.Single(order.Items);

// ── FluentAssertions ───────────────────────────────
order.Should().NotBeNull();
order.Status.Should().Be(OrderStatus.Confirmed);
order.Discount.Should().Be(15m);
order.Items.Should().HaveCount(1);
```

- Same logic, more **readable** left-to-right
- Error messages show **what was expected vs actual**
- IDE **IntelliSense** guides you through available assertions

> 📂 Ref: `Day1.OrderSystem.Tests/OrderServiceTests_FA.cs`

---

## FA: Exception Testing

```csharp
// Native Assert
var ex = Assert.Throws<InvalidOperationException>(
    () => service.PlaceOrder(cart, customer));
Assert.Contains("minimum", ex.Message);

// FluentAssertions — chains in one expression
Action act = () => service.PlaceOrder(cart, customer);

act.Should()
   .Throw<InvalidOperationException>()
   .WithMessage("*minimum*");  // * = wildcard
```

For async methods:

```csharp
Func<Task> act = async () => await service.ProcessAsync();
await act.Should().ThrowAsync<InvalidOperationException>();
```

> 📂 Ref: `Day1.OrderSystem.Tests/OrderServiceTests_FA.cs` → `PlaceOrder_BelowMinimum_FA_ThrowsWithMessage`

---

## FA: Advanced API Tour

**Object graph comparison:**
```csharp
result.Should().BeEquivalentTo(expected);  // deep equality
```

**Collections:**
```csharp
list.Should().HaveCount(3)
    .And.Contain(item => item.Name == "Alice")
    .And.NotContainNulls();
```

**Strings:**
```csharp
message.Should().StartWith("Error")
    .And.Contain("minimum")
    .And.NotBeNullOrWhiteSpace();
```

**Chaining with `.And`:**
```csharp
order.Discount.Should().Be(15m).And.BePositive();
```

> 📂 Ref: `Day1.OrderSystem.Tests/OrderServiceTests_FA.cs`

---

## Best Practices

- **One concern per test** — test exactly one thing
- **Descriptive names** — `Method_Scenario_Expected`
- **No logic in tests** — no `if`, `for`, `switch`
- **Isolated tests** — no shared mutable state between tests
- **Fast tests** — avoid `Thread.Sleep`, real I/O
- **Tests as documentation** — readable without comments

```csharp
// ❌ Anti-pattern: testing too many things at once
[Fact]
public void TestOrder()
{
    // Tests placement, discount, cancellation all in one — hard to diagnose
}

// ✅ Each test = one clear scenario
[Fact] public void PlaceOrder_ValidCart_ReturnsConfirmedOrder() { }
[Fact] public void PlaceOrder_BelowMinimum_ThrowsException() { }
[Fact] public void CancelOrder_ShippedOrder_ReturnsFalse() { }
```

---

<!-- _class: lead -->

# Lab Time

## `Day1.Lab.BankAccount.Tests`

Open `BankAccountTests.cs` and complete the `// TODO` stubs

```bash
dotnet test
```

> All tests should go **green** by the end of the session
