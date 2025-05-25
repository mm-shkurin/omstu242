using System;

public class GenericCollection<T>
{
    private readonly T[] _items;
    private int _size;

    public GenericCollection(int capacity)
    {
        _items = new T[capacity];
        _size = 0;
    }

    public void Add(T element)
    {
        if (_size >= _items.Length)
        {
            Console.WriteLine("Collection is full");
            return;
        }
        
        _items[_size++] = element;
    }

    public void RemoveAt(int index)
    {
        if (!IsValidIndex(index))
        {
            Console.WriteLine("Invalid index");
            return;
        }

        Array.Copy(_items, index + 1, _items, index, _size - index - 1);
        _items[--_size] = default;
    }

    public T GetElement(int index)
    {
        if (!IsValidIndex(index))
            throw new IndexOutOfRangeException("Invalid index");

        return _items[index];
    }

    public void DisplayElements()
    {
        for (var i = 0; i < _size; i++)
            Console.WriteLine(_items[i]);
    }

    private bool IsValidIndex(int index) => index >= 0 && index < _size;
}

public class CollectionManager
{
    private readonly GenericCollection<string> _collection;

    public CollectionManager(int capacity)
    {
        _collection = new GenericCollection<string>(capacity);
    }

    public void Run()
    {
        while (true)
        {
            DisplayMenu();
            var choice = GetUserChoice();

            switch (choice)
            {
                case 1:
                    AddElement();
                    break;
                case 2:
                    RemoveElement();
                    break;
                case 3:
                    RetrieveElement();
                    break;
                case 4:
                    ShowAllElements();
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("\nAvailable actions:");
        Console.WriteLine("1. Add element");
        Console.WriteLine("2. Remove element");
        Console.WriteLine("3. Get element by index");
        Console.WriteLine("4. Display all elements");
        Console.WriteLine("5. Exit\n");
    }

    private static int GetUserChoice()
    {
        Console.Write("Enter your choice: ");
        return int.Parse(Console.ReadLine());
    }

    private void AddElement()
    {
        Console.Write("Enter element to add: ");
        _collection.Add(Console.ReadLine());
    }

    private void RemoveElement()
    {
        Console.Write("Enter index to remove: ");
        var index = int.Parse(Console.ReadLine());
        _collection.RemoveAt(index);
    }

    private void RetrieveElement()
    {
        Console.Write("Enter index to retrieve: ");
        var index = int.Parse(Console.ReadLine());

        try
        {
            var element = _collection.GetElement(index);
            Console.WriteLine($"Element at index {index}: {element}");
        }
        catch (IndexOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void ShowAllElements()
    {
        Console.WriteLine("\nCurrent collection contents:");
        _collection.DisplayElements();
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Enter collection capacity: ");
        var capacity = int.Parse(Console.ReadLine());
        
        new CollectionManager(capacity).Run();
    }
}