namespace Day1.Lab.BankAccount;

/// <summary>
/// Represents a simple bank account with deposit, withdraw, and transfer operations.
/// This is the System Under Test (SUT) for the Day 1 lab exercises.
/// </summary>
public class BankAccount
{
    private decimal _balance;

    public string Owner { get; }

    public BankAccount(string owner, decimal initialBalance = 0m)
    {
        if (string.IsNullOrWhiteSpace(owner))
            throw new ArgumentException("Owner name cannot be empty.", nameof(owner));

        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative.", nameof(initialBalance));

        Owner    = owner;
        _balance = initialBalance;
    }

    public decimal GetBalance() => _balance;

    /// <summary>Deposits an amount. Amount must be greater than zero.</summary>
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive.", nameof(amount));

        _balance += amount;
    }

    /// <summary>
    /// Withdraws an amount.
    /// Throws <see cref="ArgumentException"/> when amount is not positive.
    /// Throws <see cref="InvalidOperationException"/> when balance is insufficient.
    /// </summary>
    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));

        if (amount > _balance)
            throw new InvalidOperationException(
                $"Insufficient funds. Balance: {_balance:C}, Requested: {amount:C}");

        _balance -= amount;
    }

    /// <summary>
    /// Transfers an amount from this account to the destination account.
    /// Internally calls <see cref="Withdraw"/> and <see cref="Deposit"/>.
    /// </summary>
    public void Transfer(BankAccount destination, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(destination);

        Withdraw(amount);
        destination.Deposit(amount);
    }
}
