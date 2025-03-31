using System;
using System.Collections.Generic;

class Car
{
    public int Year { get; set; }
    public string Brand { get; set; }
    public bool IsClean { get; set; }

    public Car(int year, string brand, bool isClean)
    {
        Year = year;
        Brand = brand;
        IsClean = isClean;
    }
}

class Garage
{
    public List<Car> Cars { get; private set; } = new List<Car>();

    public void AddCar(Car car)
    {
        Cars.Add(car);
    }
}

class CarWash
{
    public void WashCar(Car car)
    {
        car.IsClean = true;
        Console.WriteLine($"Car {car.Brand} ({car.Year}) now clean.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Garage garage = new Garage();
        
        Console.Write("Enter the number of cars:");
        int carCount = int.Parse(Console.ReadLine() ?? "0");

        for (int i = 0; i < carCount; i++)
        {
            Console.WriteLine($"Enter the information for the car {i + 1}:");
            Console.Write("Year of release:");
            int year = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Model: ");
            string brand = Console.ReadLine();

            Console.Write("Condition (clean - 1, dirty - 0): ");
            bool isClean = Console.ReadLine() == "1";

            garage.AddCar(new Car(year, brand, isClean));
        }

        CarWash carWash = new CarWash();
        foreach (Car car in garage.Cars)
        {
            if (!car.IsClean)
            {
                carWash.WashCar(car);
            }
        }
    }
}
