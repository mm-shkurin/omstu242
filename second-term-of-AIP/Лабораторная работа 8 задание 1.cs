using System;
class Program
{
    static void Main()
    {
        Console.WriteLine("enter first number");
        double num1 = double.Parse(Console.ReadLine());
        Console.WriteLine("enter second number");
        double num2 = double.Parse(Console.ReadLine());
        Func<double, double, double> add = (x, y) => x + y;
        Func<double, double, double> subtract = (x, y) => x - y;
        Func<double, double, double> multiply = (x, y) => x * y;
        Func<double, double, double> divide = (x, y) => 
        {
            if (y == 0)
            {
                Console.WriteLine("error!");
                return double.NaN;
            }
            return x / y;
        };
        Console.WriteLine($"sum: {num1} + {num2} = {add(num1, num2)}");
        Console.WriteLine($"difference: {num1} - {num2} = {subtract(num1, num2)}");
        Console.WriteLine($"multiplication: {num1} * {num2} = {multiply(num1, num2)}");
        double divisionResult = divide(num1, num2);
        if (!double.IsNaN(divisionResult))
        {
            Console.WriteLine($"division: {num1} / {num2} = {divisionResult}");
        }
    }
}