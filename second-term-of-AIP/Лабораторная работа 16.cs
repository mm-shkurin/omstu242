using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalyzer
{
    public static class Program
    {
        static void Main()
        {
            try
            {
                var text = TextReader.GetInputText();
                var frequency = CharacterAnalyzer.Analyze(text);
                ResultPresenter.Display(frequency);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public static class TextReader
    {
        public static string GetInputText()
        {
            Console.WriteLine("Enter text (press Enter twice to finish):");
            
            var lines = new List<string>();
            while (true)
            {
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line)) break;
                lines.Add(line);
            }

            if (lines.Count == 0)
            {
                throw new InvalidOperationException("No text entered");
            }

            return string.Join("", lines);
        }
    }

    public static class CharacterAnalyzer
    {
        public static IReadOnlyDictionary<char, int> Analyze(string text)
        {
            return text
                .GroupBy(c => c)
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count()
                );
        }
    }

    public static class ResultPresenter
    {
        public static void Display(IReadOnlyDictionary<char, int> frequency)
        {
            Console.WriteLine("\nCharacter frequency:");
            Console.WriteLine("---------------------");
            
            foreach (var (character, count) in frequency)
            {
                Console.WriteLine($"'{character}': {count} occurrences");
            }

            Console.WriteLine("---------------------");
            Console.WriteLine($"Total unique characters: {frequency.Count}");
        }
    }
}