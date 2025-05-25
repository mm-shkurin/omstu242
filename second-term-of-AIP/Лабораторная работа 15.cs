using System;

namespace PalindromeFinder
{
    public static class Program
    {
        static void Main()
        {
            try
            {
                var numbers = InputHandler.GetNumbers();
                var palindromes = NumberProcessor.FindPalindromes(numbers);
                OutputHandler.DisplayResults(palindromes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public static class InputHandler
    {
        public static int[] GetNumbers()
        {
            Console.Write("Enter array size: ");
            var size = ReadPositiveInteger();
            
            var numbers = new int[size];
            Console.WriteLine($"Enter {size} integers:");
            
            for (var i = 0; i < size; i++)
            {
                numbers[i] = ReadInteger();
            }
            
            return numbers;
        }

        private static int ReadPositiveInteger()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out var result) && result > 0)
                    return result;

                Console.Write("Invalid input. Please enter a positive integer: ");
            }
        }

        private static int ReadInteger()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out var result))
                    return result;

                Console.Write("Invalid integer. Please try again: ");
            }
        }
    }

    public static class NumberProcessor
    {
        public static int[] FindPalindromes(int[] numbers)
        {
            var result = new System.Collections.Generic.List<int>();
            foreach (var number in numbers)
            {
                if (IsPalindrome(number))
                {
                    result.Add(number);
                }
            }
            return result.ToArray();
        }

        public static bool IsPalindrome(int number)
        {
            if (number < 0) return false;
            if (number < 10) return true;

            var original = number;
            long reversed = 0; // Используем long для защиты от переполнения

            while (number > 0)
            {
                reversed = reversed * 10 + number % 10;
                if (reversed > int.MaxValue) return false;
                number /= 10;
            }

            return original == reversed;
        }
    }

    public static class OutputHandler
    {
        public static void DisplayResults(int[] palindromes)
        {
            Console.WriteLine("\nPalindromes in array:");
            
            if (palindromes.Length == 0)
            {
                Console.WriteLine("No palindromes found");
                return;
            }

            foreach (var num in palindromes)
            {
                Console.WriteLine($"→ {num}");
            }
            
            Console.WriteLine($"\nTotal found: {palindromes.Length}");
        }
    }
}