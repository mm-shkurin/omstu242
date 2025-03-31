using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the expression in the postfix entry:");
        string input = Console.ReadLine();
        double result = EvaluatePostfix(input);
        Console.WriteLine($"Result: {result}");
    }

    static double EvaluatePostfix(string expression)
    {
        Stack<double> stack = new Stack<double>();
        string[] tokens = expression.Split(' ');
        foreach (string token in tokens)
        {
            if (double.TryParse(token, out double number)) 
            {
                stack.Push(number);
            }
            else 
            {
                double b = stack.Pop();
                double a = stack.Pop();
                switch (token)
                {
                    case "+":
                        stack.Push(a + b);
                        break;
                    case "-":
                        stack.Push(a - b);
                        break;
                    case "*":
                        stack.Push(a * b);
                        break;
                    case "/":
                        if (b == 0)
                        {
                            throw new DivideByZeroException("Error: Division by zero.");
                        }
                        stack.Push(a / b);
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown operator: {token}");
                }
            }
        }

        return stack.Pop();
    }
}
