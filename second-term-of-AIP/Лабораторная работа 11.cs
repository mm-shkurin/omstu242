using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NumberFilterApplication
{
    public class EvenNumberValidator
    {
        private static readonly Regex NumberPattern = new Regex(@"\d+", RegexOptions.Compiled);

        public bool ContainsEvenNumber(string input)
        {
            foreach (Match match in NumberPattern.Matches(input))
            {
                if (int.TryParse(match.Value, out int number) && number % 2 == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class FileOutputWriter : IDisposable
    {
        private readonly StreamWriter _writer;
        
        public string OutputPath { get; }

        public FileOutputWriter(string filePath)
        {
            OutputPath = filePath;
            _writer = new StreamWriter(filePath);
        }

        public void WriteLine(string content)
        {
            _writer.WriteLine(content);
        }

        public void Dispose()
        {
            _writer?.Dispose();
        }
    }

    public class ApplicationCore
    {
        private readonly EvenNumberValidator _validator = new EvenNumberValidator();
        private const string ExitCommand = "exit";
        private const string OutputFileName = "filtered_output.txt";

        public void Run()
        {
            Console.WriteLine("Enter text lines. To exit, type 'exit':");
            
            using (var fileWriter = new FileOutputWriter(OutputFileName))
            {
                ProcessUserInput(fileWriter);
            }
            
            Console.WriteLine($"Lines with even numbers saved to: {OutputFileName}");
        }

        private void ProcessUserInput(FileOutputWriter writer)
        {
            while (true)
            {
                var input = Console.ReadLine()?.Trim();
                
                if (IsExitCommand(input)) break;
                if (ShouldWriteLine(input)) writer.WriteLine(input);
            }
        }

        private bool IsExitCommand(string input) => 
            input?.Equals(ExitCommand, StringComparison.OrdinalIgnoreCase) ?? false;

        private bool ShouldWriteLine(string input) => 
            !string.IsNullOrEmpty(input) && _validator.ContainsEvenNumber(input);
    }

    class Program
    {
        static void Main(string[] args)
        {
            new ApplicationCore().Run();
        }
    }
}