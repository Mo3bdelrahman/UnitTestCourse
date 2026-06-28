using Day1.Calculator;

var calc = new Calculator();

Console.WriteLine($"5 + 3 = {calc.Add(5, 3)}");
Console.WriteLine($"10 - 4 = {calc.Subtract(10, 4)}");
Console.WriteLine($"6 x 7 = {calc.Multiply(6, 7)}");
Console.WriteLine($"20 / 4 = {calc.Divide(20, 4)}");
Console.WriteLine($"Is 8 even? {calc.IsEven(8)}");
