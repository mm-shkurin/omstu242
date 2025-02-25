using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter the expression to check: ");
        string expression = Console.ReadLine();
        
        if (IsBalanced(expression))
        {
            Console.WriteLine("The brackets are placed correctly.");
        }
        else
        {
            Console.WriteLine("The brackets are placed incorrectly.");
        }
    }

    static bool IsBalanced(string expression)
    {
        Stack<char> stack = new Stack<char>();
        foreach (char ch in expression)
        {
            if (ch == '[' || ch == '(' || ch == '{')
            {
                stack.Push(ch);()
            }
            else if (ch == ']' || ch == ')' || ch == '}')
            {
                if (stack.Count == 0)
                {
                    return false; 
                }

                char openingBracket = stack.Pop();
                if (!IsMatchingPair(openingBracket, ch))
                {
                    return false;
                }
            }
        }
        return stack.Count == 0;
    }

    static bool IsMatchingPair(char opening, char closing)
    {
        return (opening == '[' && closing == ']') ||
               (opening == '(' && closing == ')') ||
               (opening == '{' && closing == '}');
    }
}
