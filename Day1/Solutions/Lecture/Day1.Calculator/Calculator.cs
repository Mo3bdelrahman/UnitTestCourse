namespace Day1.Calculator;

public class Calculator
{
    public int Add(int a, int b) => a + b;

    public int Subtract(int a, int b) => a - b;

    public int Multiply(int a, int b) => a * b;

    /// <summary>Divides a by b. Throws <see cref="DivideByZeroException"/> when b is 0.</summary>
    public int Divide(int a, int b)
    {
        if (b == 0)
            throw new DivideByZeroException("Cannot divide by zero.");

        return a / b;
    }

    /// <summary>Returns true when number is even.</summary>
    public bool IsEven(int number) => number % 2 == 0;
}
