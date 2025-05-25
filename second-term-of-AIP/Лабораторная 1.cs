using System;
using System.Collections.Generic;

public class Phone
{
    public string Number { get; }
    public string Operator { get; }
    public int Year { get; }
    public string City { get; }

    public Phone(string number, string operatorName, int year, string city)
    {
        Number = number;
        Operator = operatorName;
        Year = year;
        City = city;
    }

    public override string ToString()
    {
        return $"Number: {Number}, Operator: {Operator}, Year: {Year}, City: {City}";
    }
}

public class Subscriber
{
    public string FullName { get; }
    public List<Phone> Phones { get; }

    public Subscriber(string fullName)
    {
        FullName = fullName;
        Phones = new List<Phone>();
    }

    public void AddPhone(Phone phone)
    {
        Phones.Add(phone);
    }

    public override string ToString()
    {
        return $"Subscriber: {FullName}, Count: {Phones.Count}";
    }
}

class Program
{
    static List<Subscriber> subscribers = new List<Subscriber>();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить абонента");
            Console.WriteLine("2. Добавить телефон абонента");
            Console.WriteLine("3. Поиск по городу");
            Console.WriteLine("4. Поиск по номеру телефона");
            Console.WriteLine("5. Поиск по оператору");
            Console.WriteLine("6. Поиск по году подключения");
            Console.WriteLine("0. Выход");
            string option = Console.ReadLine();
            
            switch (option)
            {
                case "1":
                    AddSubscriber();
                    break;
                case "2":
                    AddPhoneToSubscriber();
                    break;
                case "3":
                    SearchByCity();
                    break;
                case "4":
                    SearchByPhoneNumber();
                    break;
                case "5":
                    SearchByOperator();
                    break;
                case "6":
                    SearchByYear();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Некорректный ввод. Пожалуйста, попробуйте снова.");
                    break;
            }
        }
    }

    static void AddSubscriber()
    {
        Console.Write("Введите ФИО абонента: ");
        string fullName = Console.ReadLine();
        subscribers.Add(new Subscriber(fullName));
        Console.WriteLine("Абонент добавлен.");
    }

    static void AddPhoneToSubscriber()
    {
        Console.Write("Введите ФИО абонента: ");
        string fullName = Console.ReadLine();
        var subscriber = subscribers.Find(s => s.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase));
        if (subscriber != null)
        {
            Console.Write("Введите номер телефона: ");
            string number = Console.ReadLine();
            Console.Write("Введите оператора связи: ");
            string operatorName = Console.ReadLine();
            Console.Write("Введите год подключения: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Введите город: ");
            string city = Console.ReadLine();
            subscriber.AddPhone(new Phone(number, operatorName, year, city));
            Console.WriteLine("Телефон добавлен.");
        }
        else
        {
            Console.WriteLine("Абонент не найден.");
        }
    }

    static void SearchByCity()
    {
        Console.Write("Введите город для поиска: ");
        string city = Console.ReadLine();
        foreach (var subscriber in subscribers)
        {
            foreach (var phone in subscriber.Phones)
            {
                if (phone.City.Equals(city, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"{subscriber.FullName} - {phone}");
                }
            }
        }
    }


    static void SearchByPhoneNumber()
    {
        Console.Write("Введите номер телефона для поиска: ");
        string number = Console.ReadLine();
        foreach (var subscriber in subscribers)
        {
            foreach (var phone in subscriber.Phones)
            {
                if (phone.Number.Equals(number, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"{subscriber.FullName} - {phone}");
                }
            }
        }
    }

    static void SearchByOperator()
    {
        Console.Write("Введите оператора связи для поиска: ");
        string operatorName = Console.ReadLine();
        foreach (var subscriber in subscribers)
        {
            foreach (var phone in subscriber.Phones)
            {
                if (phone.Operator.Equals(operatorName, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"{subscriber.FullName} - {phone}");
                }
            }
        }
    }

    static void SearchByYear()
    {
        Console.Write("Введите год подключения для поиска: ");
        int year = int.Parse(Console.ReadLine());
        foreach (var subscriber in subscribers)
        {
            foreach (var phone in subscriber.Phones)
            {
                if (phone.Year == year)
                {
                    Console.WriteLine($"{subscriber.FullName} - {phone}");
                }
            }
        }
    }
}
