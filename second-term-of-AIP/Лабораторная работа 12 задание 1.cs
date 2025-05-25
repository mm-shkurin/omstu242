using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker
{
    public class Client
    {
        public string Name { get; }
        public List<DateTime> Visits { get; } = new List<DateTime>();

        public Client(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public class Trainer
    {
        public string Name { get; }

        public Trainer(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public class GymVisit
    {
        public Client Client { get; }
        public Trainer Trainer { get; }
        public DateTime VisitDate { get; }

        public GymVisit(Client client, Trainer trainer, DateTime visitDate)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Trainer = trainer ?? throw new ArgumentNullException(nameof(trainer));
            VisitDate = visitDate;
        }
    }

    public class FitnessTrackerService
    {
        private readonly List<Client> _clients = new List<Client>();
        private readonly List<Trainer> _trainers = new List<Trainer>();
        private readonly List<GymVisit> _visits = new List<GymVisit>();

        public void AddTrainer(string name) => _trainers.Add(new Trainer(name));
        public void AddClient(string name) => _clients.Add(new Client(name));

        public void AddVisit(string clientName, string trainerName, DateTime date)
        {
            var client = _clients.FirstOrDefault(c => c.Name == clientName);
            var trainer = _trainers.FirstOrDefault(t => t.Name == trainerName);

            if (client == null || trainer == null)
                throw new ArgumentException("Invalid client or trainer");

            _visits.Add(new GymVisit(client, trainer, date));
            client.Visits.Add(date);
        }

        public IEnumerable<(DateTime Date, string Trainer, int ClientsCount)> GetVisitStatistics()
            => _visits
                .GroupBy(v => new { v.VisitDate.Date, v.Trainer.Name })
                .Select(g => (g.Key.Date, g.Key.Name, g.Select(v => v.Client.Name).Distinct().Count()));

        public (string Name, int Count)? GetMostFrequentClient()
            => _clients
                .Select(c => (c.Name, Count: c.Visits.Distinct().Count()))
                .OrderByDescending(c => c.Count)
                .FirstOrDefault();

        public IEnumerable<(string Client, IEnumerable<DateTime> Visits)> GetAllClientVisits()
            => _clients.Select(c => (c.Name, c.Visits.Distinct()));
    }

    public class FitnessTrackerUI
    {
        private readonly FitnessTrackerService _service = new FitnessTrackerService();

        public void Run()
        {
            InitializeTrainers();
            InitializeClients();
            InitializeVisits();
            ShowStatistics();
        }

        private void InitializeTrainers()
        {
            var count = ReadNumber("Enter number of trainers:");
            for (int i = 0; i < count; i++)
            {
                var name = ReadInput($"Enter trainer #{i + 1} name:");
                _service.AddTrainer(name);
            }
        }

        private void InitializeClients()
        {
            var count = ReadNumber("Enter number of clients:");
            for (int i = 0; i < count; i++)
            {
                var name = ReadInput($"Enter client #{i + 1} name:");
                _service.AddClient(name);
            }
        }

        private void InitializeVisits()
        {
            var count = ReadNumber("Enter number of visits:");
            for (int i = 0; i < count; i++)
            {
                try
                {
                    var client = ReadInput("Enter client name:");
                    var trainer = ReadInput("Enter trainer name:");
                    var date = ReadDate("Enter visit date (yyyy-MM-dd):");
                    _service.AddVisit(client, trainer, date);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    i--;
                }
            }
        }

        private void ShowStatistics()
        {
            Console.WriteLine("\nVisits by date and trainer:");
            foreach (var stat in _service.GetVisitStatistics())
            {
                Console.WriteLine($"Date: {stat.Date:d}, Trainer: {stat.Trainer}, Clients: {stat.ClientsCount}");
            }

            var topClient = _service.GetMostFrequentClient();
            Console.WriteLine(topClient.HasValue
                ? $"\nMost frequent client: {topClient.Value.Name}, Visits: {topClient.Value.Count}"
                : "\nNo client visits recorded");

            Console.WriteLine("\nAll client visits:");
            foreach (var client in _service.GetAllClientVisits())
            {
                Console.WriteLine($"Client: {client.Client}");
                foreach (var visit in client.Visits)
                {
                    Console.WriteLine($" - {visit:d}");
                }
            }
        }

        private static string ReadInput(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        private static int ReadNumber(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) return result;
                Console.WriteLine("Invalid number, try again");
            }
        }

        private static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                if (DateTime.TryParse(Console.ReadLine(), out DateTime result)) return result;
                Console.WriteLine("Invalid date format, use yyyy-MM-dd");
            }
        }
    }

    class Program
    {
        static void Main(string[] args) => new FitnessTrackerUI().Run();
    }
}