# Unit Testing in .NET

Hands-on course materials for learning unit testing with **.NET 9**, **xUnit v3**, and **FluentAssertions**. Each day combines lecture demos, guided code snippets, and a lab exercise you implement yourself.

## Prerequisites

| Requirement | Version | Verify |
|---|---|---|
| .NET SDK | 9.0+ | `dotnet --version` |
| Visual Studio 2022 | 17.8+ | Help → About |
| VS Code + C# Dev Kit | latest | Extensions panel |
| Git (optional) | any | `git --version` |

If multiple SDKs are installed, confirm .NET 9 is active:

```bash
dotnet --list-sdks
```

To pin the SDK for this folder:

```bash
dotnet new globaljson --sdk-version 9.0.xxx
```

## Repository structure

```
UnitTest/
├── Day1/
│   ├── Presentation/     # Lecture slides (Marp)
│   ├── CodeSnippets/     # Step-by-step walkthrough
│   ├── LabNotes/         # Lab setup and exercises
│   └── Solutions/
│       ├── Lecture/      # Completed lecture demos
│       └── Lab/          # Lab starter + reference solution
└── README.md
```

Additional days will follow the same layout as the course expands.

## Day 1 — xUnit fundamentals & Fluent Assertions

**Topics:** testing pyramid, AAA pattern, test naming, `[Fact]`, `[Theory]`, exception testing, shared fixtures, FluentAssertions.

| Resource | Path |
|---|---|
| Slides | `Day1/Presentation/Day1-Presentation.md` |
| Code walkthrough | `Day1/CodeSnippets/Day1-CodeSnippets.md` |
| Lab instructions | `Day1/LabNotes/Day1-LabNotes.md` |

### Lecture demos

Two sample apps demonstrate concepts from basic assertions through advanced patterns:

| Project | What it covers |
|---|---|
| `Day1/Solutions/Lecture/Day1.Calculator` | First tests, theories, exception handling |
| `Day1/Solutions/Lecture/Day1.OrderSystem` | Fixtures, `[MemberData]`, `[ClassData]`, FluentAssertions |

Run all lecture tests:

```bash
dotnet test Day1/Solutions/Lecture/Day1.Calculator.Tests
dotnet test Day1/Solutions/Lecture/Day1.OrderSystem.Tests
```

### Lab — Bank account tests

Implement 12 unit tests for a pre-built `BankAccount` class. Your work goes in `BankAccountTests.cs`; do not edit the production code.

```
Day1/Solutions/Lab/
├── Day1.Lab.BankAccount/           # System under test (do not edit)
└── Day1.Lab.BankAccount.Tests/     # Your lab work goes here
```

**Setup:**

```bash
cd Day1/Solutions/Lab
dotnet restore
dotnet build
dotnet test
```

Tests start red with `NotImplementedException`. Replace each `// TODO` with FluentAssertions assertions until all 12 pass. See `Day1/LabNotes/Day1-LabNotes.md` for exercise details and hints.

**Expected result:**

```
Total tests: 12
     Passed: 12
     Failed: 0
```

## Running tests

From any test project folder:

```bash
dotnet test                          # run all tests
dotnet test --filter "TestName"      # run one test by name
dotnet test --logger "console;verbosity=detailed"
```

In Visual Studio, open a test project folder and use **Test Explorer** (`Ctrl+E, T`). In VS Code, use the **Testing** view after installing C# Dev Kit.

## Packages used

Test projects reference these NuGet packages (restored automatically):

- `xunit.v3` — test framework
- `xunit.runner.visualstudio` — Test Explorer integration
- `Microsoft.NET.Test.Sdk` — test host
- `FluentAssertions` — readable assertion syntax

## Coming next — Day 2 preview

| Topic | What you'll learn |
|---|---|
| Moq | Replace real dependencies with test doubles |
| Mock, Stub, Fake | Differences between test double types |
| Setup & Verify | Control mock behavior and assert interactions |
| API unit testing | Test controllers without starting a server |
| Test coverage | Measure how much code your tests exercise |

## Troubleshooting

| Problem | Solution |
|---|---|
| `NotImplementedException` in lab | Replace `// TODO` blocks with real assertions |
| `Package not found` | Run `dotnet restore` |
| `CS0246: type not found` | Add `using FluentAssertions;` |
| Tests missing in Explorer | Rebuild with `dotnet build` |
