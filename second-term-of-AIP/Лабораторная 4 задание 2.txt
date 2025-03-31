using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the list items, separating them with spaces:");
        string input = Console.ReadLine();
        List<string> elements = new List<string>(input.Split(' '));
        Dictionary<string, int> frequency = GetElementFrequency(elements);
        Console.WriteLine("Unique elements:");
        foreach (var element in frequency.Keys)
        {
            Console.WriteLine(element);
        }
        Console.WriteLine("\nThe frequency of each element:");
        foreach (var kvp in frequency)
        {
            Console.WriteLine($"Element: {kvp.Key}, Frequency: {kvp.Value}");
        }
    }

