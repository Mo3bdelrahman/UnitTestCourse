# Day 1 Lab Notes
## Unit Testing with XUnit v3 & FluentAssertions

---

## Prerequisites

Before the lab, verify your environment is ready:

| Requirement | Version | Check |
|---|---|---|
| .NET SDK | 9.0+ | `dotnet --version` |
| Visual Studio 2022 | 17.8+ | Help → About |
| VS Code + C# Dev Kit | latest | Extensions panel |
| Git (optional) | any | `git --version` |

> If multiple SDKs are installed, confirm .NET 9 is active:
> ```bash
> dotnet --list-sdks
> ```
> To pin the version for this folder, run `dotnet new globaljson --sdk-version 9.0.xxx`

---

## Lab Folder Structure

```
Day1\Solutions\Lab\
├── Day1.Lab.sln
├── Day1.Lab.BankAccount\
│   ├── Day1.Lab.BankAccount.csproj   ← class library (already implemented)
│   └── BankAccount.cs                ← System Under Test — DO NOT EDIT
└── Day1.Lab.BankAccount.Tests\
    ├── Day1.Lab.BankAccount.Tests.csproj
    └── BankAccountTests.cs           ← ✏️  YOUR WORK IS HERE
```

---

## Setup Steps

### Step 1 — Open the Solution

**Visual Studio 2022:**
```
File → Open → Project/Solution → Day1.Lab.sln
```

**VS Code:**
```bash
cd "Day1\Solutions\Lab"
code .
```

**Terminal only:**
```bash
cd "Day1\Solutions\Lab"
```

---

### Step 2 — Restore NuGet Packages

```bash
dotnet restore
```

Expected output:
```
Restored Day1.Lab.BankAccount\Day1.Lab.BankAccount.csproj
Restored Day1.Lab.BankAccount.Tests\Day1.Lab.BankAccount.Tests.csproj
```

> **Packages included in the project — no manual install needed:**
> - `xunit.v3` — test framework
> - `xunit.runner.visualstudio` — Test Explorer integration
> - `FluentAssertions` — readable assertions
> - `Microsoft.NET.Test.Sdk` — test host

---

### Step 3 — Build the Solution

```bash
dotnet build
```

Expected: `Build succeeded. 0 Error(s)`

> If you see errors in `BankAccount.cs`, you may have accidentally edited it.
> Restore it from the course materials zip.

---

### Step 4 — Run the Tests (They Will Fail)

```bash
dotnet test
```

You will see `NotImplementedException` for every test — this is **expected**.
Your job is to replace each `throw new NotImplementedException()` with real assertions.

```bash
# Verbose output — see exactly which tests failed and why
dotnet test --logger "console;verbosity=detailed"
```

---

### Step 5 — Open `BankAccountTests.cs`

Every test method follows this structure:

```csharp
[Fact]
public void Deposit_ValidAmount_IncreasesBalance()
{
    // Arrange — inputs are already set up for you
    var account = new BankAccount("Alice", initialBalance: 100m);

    // Act — the action is already called for you
    account.Deposit(50m);

    // TODO: Assert — this is what you write
    // account.GetBalance().Should().Be(???);
    throw new NotImplementedException();
}
```

**Your task:** Remove the `throw new NotImplementedException()` and write the assertion.

---

## FluentAssertions Quick Reference

| Scenario | FluentAssertions Syntax |
|---|---|
| Value equals expected | `result.Should().Be(expected)` |
| Value is null | `result.Should().BeNull()` |
| Value is not null | `result.Should().NotBeNull()` |
| Boolean is true | `flag.Should().BeTrue()` |
| Boolean is false | `flag.Should().BeFalse()` |
| Decimal is positive | `amount.Should().BePositive()` |
| Greater than | `x.Should().BeGreaterThan(0)` |
| Exception thrown | `act.Should().Throw<ExceptionType>()` |
| Exception with message | `.Throw<T>().WithMessage("*keyword*")` |
| Collection size | `list.Should().HaveCount(3)` |
| Collection contains | `list.Should().Contain(x => x.Name == "A")` |

---

## Lab Exercises

Complete each test method in `BankAccountTests.cs`.

### Exercise 1 — Deposit (3 tests)

| # | Test Method | What to Assert |
|---|---|---|
| 1 | `Deposit_ValidAmount_IncreasesBalance` | Balance = initial + deposited |
| 2 | `Deposit_MultipleDeposits_AccumulatesCorrectly` | Balance = sum of all deposits |
| 3 | `Deposit_ZeroOrNegativeAmount_ThrowsArgumentException` | `ArgumentException` is thrown |

**Hint for test 3:**
```csharp
Action act = () => account.Deposit(invalidAmount);
act.Should().Throw<ArgumentException>();
```

---

### Exercise 2 — Withdraw (4 tests)

| # | Test Method | What to Assert |
|---|---|---|
| 4 | `Withdraw_ValidAmount_DecreasesBalance` | Balance = initial - withdrawn |
| 5 | `Withdraw_ExactBalance_LeavesZeroBalance` | Balance = 0 |
| 6 | `Withdraw_MoreThanBalance_ThrowsInvalidOperationException` | `InvalidOperationException` + message contains "Insufficient" |
| 7 | `Withdraw_ZeroOrNegativeAmount_ThrowsArgumentException` | `ArgumentException` is thrown |

**Hint for test 6:**
```csharp
Action act = () => account.Withdraw(100m);
act.Should()
   .Throw<InvalidOperationException>()
   .WithMessage("*Insufficient*");
```

---

### Exercise 3 — Transfer (3 tests)

| # | Test Method | What to Assert |
|---|---|---|
| 8 | `Transfer_ValidAmount_MovesMoneyBetweenAccounts` | Source decreases AND destination increases |
| 9 | `Transfer_InsufficientFunds_ThrowsAndLeavesAccountsUnchanged` | Exception thrown + both balances unchanged |
| 10 | `Transfer_NullDestination_ThrowsArgumentNullException` | `ArgumentNullException` is thrown |

**Hint for test 8 (asserting two values):**
```csharp
source.GetBalance().Should().Be(150m);
destination.GetBalance().Should().Be(250m);
```

---

### Exercise 4 — Constructor Validation (2 tests)

| # | Test Method | What to Assert |
|---|---|---|
| 11 | `Constructor_NegativeInitialBalance_ThrowsArgumentException` | `ArgumentException` thrown |
| 12 | `Constructor_InvalidOwnerName_ThrowsArgumentException` | `ArgumentException` thrown for `""`, `"   "`, `null` |

**Hint for test 12 (`[Theory]`):**
```csharp
Action act = () => new BankAccount(invalidName!, 100m);
act.Should().Throw<ArgumentException>();
```

---

## Running Tests

### CLI
```bash
# Run all tests
dotnet test

# Run a specific test by name
dotnet test --filter "Deposit_ValidAmount_IncreasesBalance"

# Run all tests in a category
dotnet test --filter "FullyQualifiedName~BankAccountTests"

# Verbose output
dotnet test --logger "console;verbosity=detailed"
```

### Visual Studio Test Explorer
1. **View → Test Explorer** (or `Ctrl+E, T`)
2. Click **Run All** (or the green play button)
3. Red = failing, Green = passing, Yellow = skipped

### VS Code
1. Install **C# Dev Kit** extension
2. Click the **flask icon** in the Activity Bar
3. Click **Run All Tests**

---

## Expected Final Result

When all exercises are complete:

```
Total tests: 12
     Passed: 12
     Failed: 0
```

Every test method name tells you exactly what it verified. If a test fails, the
FluentAssertions error message shows you the actual value vs. the expected value.

---

## Troubleshooting

| Problem | Solution |
|---|---|
| `NotImplementedException` | You haven't replaced the `throw` yet |
| `Package not found` | Run `dotnet restore` |
| `CS0246: type not found` | Add `using FluentAssertions;` at the top |
| Test not appearing in Explorer | Rebuild: `dotnet build` |
| Wrong balance after transfer | Check source AND destination assertions |

---

## What's Next — Day 2 Preview

| Topic | What You'll Learn |
|---|---|
| Moq | Replace real dependencies with test doubles |
| Mock, Stub, Fake | The difference between test double types |
| Setup & Verify | Control mock behavior and assert interactions |
| API Unit Testing | Test controllers without starting a server |
| Test Coverage | Measure how much code your tests exercise |
