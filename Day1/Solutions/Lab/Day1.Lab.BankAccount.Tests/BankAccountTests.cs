using Day1.Lab.BankAccount;
using FluentAssertions;
using Xunit;

namespace Day1.Lab.BankAccount.Tests;

// ╔══════════════════════════════════════════════════════════════════════════╗
// ║  DAY 1 LAB — Unit Testing with XUnit v3 & FluentAssertions             ║
// ║                                                                          ║
// ║  Instructions:                                                           ║
// ║  1. Read each test method name — it describes the scenario exactly.     ║
// ║  2. Follow the AAA comments: Arrange → Act → Assert                     ║
// ║  3. Replace every // TODO with real code using FluentAssertions.        ║
// ║  4. Run: dotnet test  (all tests should go green)                       ║
// ╚══════════════════════════════════════════════════════════════════════════╝

public class BankAccountTests
{
    // ── Exercise 1: Deposit ───────────────────────────────────────────────────

    [Fact]
    public void Deposit_ValidAmount_IncreasesBalance()
    {
        // Arrange
        var account = new BankAccount("Alice", initialBalance: 100m);

        // Act
        account.Deposit(50m);

        // TODO: Assert — balance should now be 150
        // account.GetBalance().Should().Be(???);
        throw new NotImplementedException();
    }

    [Fact]
    public void Deposit_MultipleDeposits_AccumulatesCorrectly()
    {
        // Arrange
        var account = new BankAccount("Bob", initialBalance: 0m);

        // Act
        account.Deposit(30m);
        account.Deposit(20m);
        account.Deposit(50m);

        // TODO: Assert — total should be 100
        throw new NotImplementedException();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(-0.01)]
    public void Deposit_ZeroOrNegativeAmount_ThrowsArgumentException(decimal invalidAmount)
    {
        // Arrange
        var account = new BankAccount("Carol", initialBalance: 50m);

        // Act
        Action act = () => account.Deposit(invalidAmount);

        // TODO: Assert — should throw ArgumentException
        // act.Should().Throw<???>()
        throw new NotImplementedException();
    }

    // ── Exercise 2: Withdraw ──────────────────────────────────────────────────

    [Fact]
    public void Withdraw_ValidAmount_DecreasesBalance()
    {
        // Arrange
        var account = new BankAccount("Dave", initialBalance: 200m);

        // Act
        account.Withdraw(80m);

        // TODO: Assert — balance should be 120
        throw new NotImplementedException();
    }

    [Fact]
    public void Withdraw_ExactBalance_LeavesZeroBalance()
    {
        // Arrange
        var account = new BankAccount("Eve", initialBalance: 75m);

        // Act
        account.Withdraw(75m);

        // TODO: Assert — balance should be 0
        throw new NotImplementedException();
    }

    [Fact]
    public void Withdraw_MoreThanBalance_ThrowsInvalidOperationException()
    {
        // Arrange
        var account = new BankAccount("Frank", initialBalance: 50m);

        // Act
        Action act = () => account.Withdraw(100m);

        // TODO: Assert — should throw InvalidOperationException
        //               message should contain "Insufficient"
        throw new NotImplementedException();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Withdraw_ZeroOrNegativeAmount_ThrowsArgumentException(decimal amount)
    {
        // Arrange
        var account = new BankAccount("Grace", initialBalance: 100m);

        // TODO: Act + Assert — use FluentAssertions .Throw<>()
        throw new NotImplementedException();
    }

    // ── Exercise 3: Transfer ─────────────────────────────────────────────────

    [Fact]
    public void Transfer_ValidAmount_MovesMoneyBetweenAccounts()
    {
        // Arrange
        var source      = new BankAccount("Heidi",  initialBalance: 300m);
        var destination = new BankAccount("Ivan",   initialBalance: 100m);

        // Act
        source.Transfer(destination, 150m);

        // TODO: Assert BOTH accounts
        // source balance should be 150, destination should be 250
        throw new NotImplementedException();
    }

    [Fact]
    public void Transfer_InsufficientFunds_ThrowsAndLeavesAccountsUnchanged()
    {
        // Arrange
        var source      = new BankAccount("Judy",  initialBalance: 50m);
        var destination = new BankAccount("Karl",  initialBalance: 100m);

        // Act
        Action act = () => source.Transfer(destination, 200m);

        // TODO: Assert exception is thrown
        // TODO: Assert source balance unchanged (still 50)
        // TODO: Assert destination balance unchanged (still 100)
        throw new NotImplementedException();
    }

    [Fact]
    public void Transfer_NullDestination_ThrowsArgumentNullException()
    {
        // Arrange
        var source = new BankAccount("Lena", initialBalance: 100m);

        // TODO: Act + Assert
        throw new NotImplementedException();
    }

    // ── Exercise 4: Constructor Validation ───────────────────────────────────

    [Fact]
    public void Constructor_NegativeInitialBalance_ThrowsArgumentException()
    {
        // TODO: Act + Assert
        // Hint: Action act = () => new BankAccount("Test", -100m);
        throw new NotImplementedException();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_InvalidOwnerName_ThrowsArgumentException(string? invalidName)
    {
        // TODO: Act + Assert
        throw new NotImplementedException();
    }
}
