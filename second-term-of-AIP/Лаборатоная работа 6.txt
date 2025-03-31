using System;

public class Values : EventArgs
{
    public string Message { get; }

    public Values(string message)
    {
        Message = message;
    }
}

public class Points
{
    public event EventHandler<Values> OutOfRange;

    private int x;
    private int y;
    private readonly int minX;
    private readonly int minY;
    private readonly int maxX;
    private readonly int maxY;

    public int X
    {
        get => x;
        set
        {
            x = value;
            Check();
        }
    }

    public int Y
    {
        get => y;
        set
        {
            y = value;
            Check();
        }
    }

    public Points(int x, int y, int minX, int minY, int maxX, int maxY)
    {
        this.x = x;
        this.y = y;
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;
    }

    private void Check()
    {
        if (x < minX || x > maxX || y < minY || y > maxY)
        {
            OnOutOfRange($"GET ERROR_REQUEST / Point ({x},{y}) is out of range");
        }
    }

    protected virtual void OnOutOfRange(string message)
    {
        OutOfRange?.Invoke(this, new Values(message));
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter coordinates n square");
        Console.Write("Min X:");
        int minX = Convert.ToInt32(Console.ReadLine());
        Console.Write("Min Y:");
        int minY = Convert.ToInt32(Console.ReadLine());
        Console.Write("Max X:");
        int maxX = Convert.ToInt32(Console.ReadLine());
        Console.Write("Max Y:");
        int maxY = Convert.ToInt32(Console.ReadLine());

        Random random = new Random();
        Points points = new Points(random.Next(minX, maxX + 1), random.Next(minY, maxY + 1), minX, minY, maxX, maxY);
        points.OutOfRange += Point_OutOfRange;

        string command;

        do
        {
            Console.WriteLine("Now coordinates: ({0},{1})", points.X, points.Y);
            Console.WriteLine("Enter 'move' for transition point, 'exit' for exit: ");
            command = Console.ReadLine();

            if (command.Equals("move", StringComparison.OrdinalIgnoreCase))
            {
                points.X += random.Next(-10, 11);
                points.Y += random.Next(-10, 11);
            }
        } while (!command.Equals("exit", StringComparison.OrdinalIgnoreCase));
    }

    private static void Point_OutOfRange(object sender, Values e)
    {
        Console.WriteLine(e.Message);
    }
}
