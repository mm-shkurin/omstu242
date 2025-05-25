using System;
using System.IO;
using System.Text.RegularExpressions;

namespace FileNumberFilter
{
    public class EvenNumberDetector
    {
        private static readonly Regex NumberPattern = new Regex(@"\d+", RegexOptions.Compiled);

        public bool HasEvenNumber(string input)
        {
            foreach (Match match in NumberPattern.Matches(input))
            {
                if (int.TryParse(match.Value, out int number) && IsEven(number))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsEven(int number) => number % 2 == 0;
    }

    public class FileProcessor
    {
        private readonly EvenNumberDetector _detector = new EvenNumberDetector();

        public void ProcessFiles(string inputPath, string outputPath)
        {
            try
            {
                var lines = File.ReadAllLines(inputPath);
                FilterLinesToFile(lines, outputPath);
                Console.WriteLine("Processing completed successfully. Check output file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        private void FilterLinesToFile(string[] lines, string outputPath)
        {
            using var writer = new StreamWriter(outputPath);
            foreach (var line in lines)
            {
                if (_detector.HasEvenNumber(line))
                {
                    writer.WriteLine(line);
                }
            }
        }
    }

    public class Application
    {
        private const string InputFile = "input.txt";
        private const string OutputFile = "output.txt";

        public void Run()
        {
            var processor = new FileProcessor();
            processor.ProcessFiles(InputFile, OutputFile);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            new Application().Run();
        }
    }
}