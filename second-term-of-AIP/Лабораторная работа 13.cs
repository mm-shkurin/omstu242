using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement
{
    public record Product(
        int Id,
        string Name,
        decimal PurchasePrice,
        decimal SalePrice
    );

    public record Supplier(
        int Id, 
        string Name
    );

    public record StockMovement(
        DateTime Date,
        Product Product,
        Supplier? Supplier,
        int Quantity,
        bool IsIncoming
    );

    public static class InventoryService
    {
        public static void DisplayMovementsGroupedByDate(IEnumerable<StockMovement> movements, bool incoming)
        {
            var filtered = movements
                .Where(m => m.IsIncoming == incoming)
                .GroupBy(m => m.Date.Date)
                .OrderBy(g => g.Key);

            var movementType = incoming ? "Incoming" : "Outgoing";
            Console.WriteLine($"\n{movementType} products grouped by date:");

            foreach (var group in filtered)
            {
                Console.WriteLine($"Date: {group.Key:d}");
                foreach (var item in group)
                {
                    var supplierInfo = item.Supplier?.Name ?? "N/A";
                    Console.WriteLine($" - {item.Product.Name}: {item.Quantity} units");
                    if (incoming) Console.WriteLine($"   Supplier: {supplierInfo}");
                }
            }
        }

        public static void DisplayInventoryReport(IEnumerable<Product> products, IEnumerable<StockMovement> movements)
        {
            var stock = products
                .Select(p => new 
                {
                    Product = p,
                    Quantity = movements
                        .Where(m => m.Product == p)
                        .Sum(m => m.IsIncoming ? m.Quantity : -m.Quantity)
                })
                .Where(x => x.Quantity > 0);

            Console.WriteLine("\nCurrent inventory:");
            foreach (var item in stock)
            {
                Console.WriteLine($" - {item.Product.Name}: {item.Quantity} units in stock");
            }
        }

        public static decimal CalculateTotalValue(IEnumerable<StockMovement> movements, bool useSalePrice)
        {
            return movements
                .Where(m => !m.IsIncoming)
                .Sum(m => m.Quantity * (useSalePrice ? m.Product.SalePrice : m.Product.PurchasePrice));
        }
    }

    public static class ConsoleHelper
    {
        public static T ReadValidInput<T>(string prompt, Func<string, T> parser)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                
                try
                {
                    return parser(input!);
                }
                catch
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
        }

        public static DateTime ReadDate(string prompt)
        {
            return ReadValidInput(prompt, input => 
                input.Equals("exit", StringComparison.OrdinalIgnoreCase) 
                    ? throw new OperationCanceledException()
                    : DateTime.Parse(input));
        }
    }

    class Program
    {
        static void Main()
        {
            var products = new List<Product>();
            var suppliers = new List<Supplier>();
            var stockMovements = new List<StockMovement>();

            InitializeSuppliers(suppliers);
            InitializeProducts(products);
            RecordStockMovements(products, suppliers, stockMovements);

            GenerateReports(products, stockMovements);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void InitializeSuppliers(ICollection<Supplier> suppliers)
        {
            var count = ConsoleHelper.ReadValidInput(
                "Enter number of suppliers: ",
                int.Parse);

            for (var i = 0; i < count; i++)
            {
                var name = ConsoleHelper.ReadValidInput(
                    $"Enter supplier #{i + 1} name: ",
                    s => string.IsNullOrWhiteSpace(s) 
                        ? throw new ArgumentException("Name cannot be empty") 
                        : s.Trim());

                suppliers.Add(new Supplier(i + 1, name));
            }
        }

        private static void InitializeProducts(ICollection<Product> products)
        {
            var count = ConsoleHelper.ReadValidInput(
                "Enter number of products: ",
                int.Parse);

            for (var i = 0; i < count; i++)
            {
                var name = ConsoleHelper.ReadValidInput(
                    $"Enter product #{i + 1} name: ",
                    s => string.IsNullOrWhiteSpace(s) 
                        ? throw new ArgumentException("Name cannot be empty") 
                        : s.Trim());

                var purchasePrice = ConsoleHelper.ReadValidInput(
                    "Enter purchase price per unit: ",
                    decimal.Parse);

                var salePrice = ConsoleHelper.ReadValidInput(
                    "Enter sale price per unit: ",
                    decimal.Parse);

                products.Add(new Product(i + 1, name, purchasePrice, salePrice));
            }
        }

        private static void RecordStockMovements(
            IReadOnlyCollection<Product> products,
            IReadOnlyCollection<Supplier> suppliers,
            ICollection<StockMovement> movements)
        {
            while (true)
            {
                try
                {
                    var date = ConsoleHelper.ReadDate(
                        "Enter movement date (or 'exit' to finish): ");

                    var product = GetValidEntity(
                        products,
                        "Enter product ID: ",
                        "Invalid product ID");

                    var quantity = ConsoleHelper.ReadValidInput(
                        "Enter quantity: ",
                        input => 
                        {
                            var value = int.Parse(input);
                            return value > 0 ? value : throw new ArgumentException();
                        });

                    var supplier = GetOptionalEntity(
                        suppliers,
                        "Enter supplier ID (0 if none): ",
                        "Invalid supplier ID");

                    movements.Add(new StockMovement(
                        date,
                        product,
                        supplier,
                        quantity,
                        supplier != null));

                    Console.WriteLine("Movement recorded successfully.\n");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static T GetValidEntity<T>(
            IEnumerable<T> entities,
            string prompt,
            string errorMessage) where T : class
        {
            return ConsoleHelper.ReadValidInput(prompt, input =>
            {
                var id = int.Parse(input);
                return entities.FirstOrDefault(e => 
                    (e as dynamic).Id == id) 
                    ?? throw new ArgumentException(errorMessage);
            });
        }

        private static T? GetOptionalEntity<T>(
            IEnumerable<T> entities,
            string prompt,
            string errorMessage) where T : class
        {
            return ConsoleHelper.ReadValidInput(prompt, input =>
            {
                var id = int.Parse(input);
                return id == 0 ? null : entities.FirstOrDefault(e => 
                    (e as dynamic).Id == id) 
                    ?? throw new ArgumentException(errorMessage);
            });
        }

        private static void GenerateReports(
            IEnumerable<Product> products,
            IEnumerable<StockMovement> movements)
        {
            InventoryService.DisplayMovementsGroupedByDate(movements, true);
            InventoryService.DisplayMovementsGroupedByDate(movements, false);
            InventoryService.DisplayInventoryReport(products, movements);

            var totalRevenue = InventoryService.CalculateTotalValue(movements, true);
            var totalCost = InventoryService.CalculateTotalValue(movements, false);
            var totalProfit = totalRevenue - totalCost;

            Console.WriteLine($"\nFinancial Report:");
            Console.WriteLine($"Total Revenue: {totalRevenue:C}");
            Console.WriteLine($"Total Cost: {totalCost:C}");
            Console.WriteLine($"Total Profit: {totalProfit:C}");
        }
    }
}