using System;

public abstract class Shape : IGeometricShape
{
    public string Name { get; set; }

    public abstract double Perimeter();
    public abstract double Area();
}

public interface IGeometricShape
{
    double Perimeter();
    double Area();
}

public class Circle : Shape
{
    public double Radius { get; set; }

    public override double Perimeter()
    {
        return 2 * Math.PI * Radius;
    }

    public override double Area()
    {
        return Math.PI * Math.Pow(Radius, 2);
    }
}

public class Square : Shape
{
    public double SideLength { get; set; }

    public override double Perimeter()
    {
        return 4 * SideLength;
    }

    public override double Area()
    {
        return Math.Pow(SideLength, 2);
    }
}

public class Triangle : Shape
{
    public double SideLength { get; set; }

    public override double Perimeter()
    {
        return 3 * SideLength;
    }

    public override double Area()
    {
        return (Math.Sqrt(3) / 4) * Math.Pow(SideLength, 2);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the circle parameters:");
        Console.Write("Radius: ");
        double radius = Convert.ToDouble(Console.ReadLine());
        var circle = new Circle { Name = "circle", Radius = radius };

        Console.WriteLine("Enter the parameters of the square:");
        Console.Write("Side Length: ");
        double sideLengthSquare = Convert.ToDouble(Console.ReadLine());
        var square = new Square { Name = "square", SideLength = sideLengthSquare };

        Console.WriteLine("Enter the triangle parameters:");
        Console.Write("Side Length: ");
        double sideLengthTriangle = Convert.ToDouble(Console.ReadLine());
        var triangle = new Triangle { Name = "triangle", SideLength = sideLengthTriangle };

        Console.WriteLine($"\nCalculation results:");
        PrintResults(circle);
        PrintResults(square);
        PrintResults(triangle);

        Console.WriteLine("\nPress any key to complete...");
        Console.ReadKey();
    }

    private static void PrintResults(Shape shape)
    {
        Console.WriteLine($"{shape.Name}:");
        Console.WriteLine($"Perimeter: {shape.Perimeter():F2}");
        Console.WriteLine($"Square: {shape.Area():F2}\n");
    }
}
