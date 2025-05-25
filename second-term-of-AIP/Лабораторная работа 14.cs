using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputerUsersRegistry
{
    public record Computer(
        string Brand,
        int ManufactureYear,
        string OperatingSystem
    );

    public record User(
        string FullName,
        int BirthYear,
        Computer? Computer
    )
    {
        public bool HasComputer => Computer != null;
    }

    public static class ConsoleHelper
    {
        public static T ReadValidInput<T>(string prompt, Func<string, T> parser, string errorMessage)
        {
            while (true)
            {
                Console.Write(prompt);
                try
                {
                    return parser(Console.ReadLine()!);
                }
                catch
                {
                    Console.WriteLine(errorMessage);
                }
            }
        }

        public static bool ReadYesNo(string prompt)
        {
            return ReadValidInput(prompt, input => 
                input!.Equals("yes", StringComparison.OrdinalIgnoreCase), 
                "Please answer 'yes' or 'no'");
        }
    }

    public static class ReportGenerator
    {
        public static void DisplayUsersWithoutComputers(IEnumerable<User> users)
        {
            var usersWithout = users.Where(u => !u.HasComputer);
            
            Console.WriteLine("\nUsers without computers:");
            usersWithout.Select(u => $"{u.FullName} ({u.BirthYear})")
                       .ToList()
                       .ForEach(Console.WriteLine);
        }

        public static void DisplayOsGroups(IEnumerable<User> users)
        {
            var osGroups = users
                .Where(u => u.HasComputer)
                .GroupBy(u => u.Computer!.OperatingSystem)
                .OrderBy(g => g.Key);

            Console.WriteLine("\nOS Distribution:");
            foreach (var group in osGroups)
            {
                Console.WriteLine($"\nOS: {group.Key}");
                group.Select(u => $"{u.FullName} - {u.Computer!.Brand} ({u.Computer.ManufactureYear})")
                    .ToList()
                    .ForEach(Console.WriteLine);
            }
        }

        public static void DisplayBrandStatistics(IEnumerable<User> users)
        {
            var brandGroups = users
                .Where(u => u.HasComputer)
                .GroupBy(u => u.Computer!.Brand)
                .OrderBy(g => g.Key);

            Console.WriteLine("\nComputer Brands:");
            foreach (var group in brandGroups)
            {
                Console.WriteLine($"\nBrand: {group.Key}");
                group.Select(u => $"{u.FullName} - {u.Computer!.OperatingSystem} ({u.Computer.ManufactureYear})")
                    .ToList()
                    .ForEach(Console.WriteLine);
            }
        }

        public static void DisplayOwnershipStatistics(IEnumerable<User> users)
        {
            var withComputer = users.Count(u => u.HasComputer);
            var withoutComputer = users.Count() - withComputer;
            
            Console.WriteLine($"\nOwnership Statistics:\nWith computers: {withComputer}\nWithout: {withoutComputer}");
            Console.WriteLine(withComputer switch
            {
                > 0 when withComputer > withoutComputer => "Majority of users have computers",
                > 0 when withComputer < withoutComputer => "Majority of users don't have computers",
                _ => "Equal distribution between owners and non-owners"
            });
        }
    }

    class Program
    {
        private static readonly List<User> Users = new();

        static void Main()
        {
            RegisterUsers();
            DisplayMenu();
        }

        private static void RegisterUsers()
        {
            Console.WriteLine("User Registration (type 'exit' to finish)\n");
            
            while (true)
            {
                var fullName = GetUserName();
                if (fullName == null) break;

                var birthYear = GetBirthYear();
                var computer = GetComputerInfo();

                Users.Add(new User(fullName, birthYear, computer));
                Console.WriteLine("User registered successfully!\n");
            }
        }

        private static string? GetUserName()
        {
            return ConsoleHelper.ReadValidInput(
                "Full Name: ",
                input => string.IsNullOrWhiteSpace(input) ? null : input.Trim(),
                "Name cannot be empty"
            );
        }

        private static int GetBirthYear()
        {
            return ConsoleHelper.ReadValidInput(
                "Birth Year: ",
                input => int.TryParse(input, out var year) && year > 1900 && year <= DateTime.Now.Year 
                    ? year 
                    : throw new FormatException(),
                "Invalid birth year"
            );
        }

        private static Computer? GetComputerInfo()
        {
            if (!ConsoleHelper.ReadYesNo("Has computer? (yes/no): ")) 
                return null;

            return new Computer(
                ConsoleHelper.ReadValidInput("Brand: ", ValidateNonEmpty, "Brand required"),
                ConsoleHelper.ReadValidInput("Manufacture Year: ", ValidateManufactureYear, "Invalid year"),
                ConsoleHelper.ReadValidInput("OS: ", ValidateNonEmpty, "OS required")
            );

            static string ValidateNonEmpty(string input) => 
                string.IsNullOrWhiteSpace(input) ? throw new FormatException() : input.Trim();

            static int ValidateManufactureYear(string input) => 
                int.TryParse(input, out var year) && year >= 1980 && year <= DateTime.Now.Year 
                    ? year 
                    : throw new FormatException();
        }

        private static void DisplayMenu()
        {
            var menuActions = new Dictionary<string, Action>
            {
                ["1"] = () => ReportGenerator.DisplayUsersWithoutComputers(Users),
                ["2"] = () => ReportGenerator.DisplayOsGroups(Users),
                ["3"] = () => ReportGenerator.DisplayBrandStatistics(Users),
                ["4"] = () => ReportGenerator.DisplayOwnershipStatistics(Users),
                ["5"] = () => Environment.Exit(0)
            };

            while (true)
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1. Show users without computers");
                Console.WriteLine("2. Show OS distribution");
                Console.WriteLine("3. Show computer brands");
                Console.WriteLine("4. Show ownership statistics");
                Console.WriteLine("5. Exit");

                var choice = ConsoleHelper.ReadValidInput(
                    "Select option: ",
                    input => menuActions.ContainsKey(input!) ? input! : throw new Exception(),
                    "Invalid menu option"
                );

                menuActions[choice]();
            }
        }
    }
}