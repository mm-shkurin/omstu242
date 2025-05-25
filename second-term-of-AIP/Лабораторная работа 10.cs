using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem
{
    public class Book
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public int PublicationYear { get; set; }
        public string Publisher { get; set; }
        public List<BorrowRecord> BorrowHistory { get; } = new List<BorrowRecord>();

        public bool HasBorrowHistory => BorrowHistory.Any();
        public bool IsBorrowed => BorrowHistory.Any(r => !r.IsReturned);
    }

    public class BorrowRecord
    {
        public string BorrowerName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned => ReturnDate.HasValue;
    }

    public class LibraryManager
    {
        private readonly List<Book> _catalog = new List<Book>();

        public void AddBook(Book book) => _catalog.Add(book);

        public void BorrowBook(int bookIndex, string borrowerName)
        {
            var book = _catalog[bookIndex];
            book.BorrowHistory.Add(new BorrowRecord
            {
                BorrowerName = borrowerName,
                BorrowDate = DateTime.Now
            });
        }

        public void ReturnBook(int bookIndex)
        {
            var record = _catalog[bookIndex].BorrowHistory.Last();
            record.ReturnDate = DateTime.Now;
        }

        public IEnumerable<Book> GetNeverBorrowedBooks() => 
            _catalog.Where(b => !b.HasBorrowHistory);

        public IEnumerable<Book> GetCurrentBorrows() => 
            _catalog.Where(b => b.IsBorrowed);

        public void DisplayBooks(IEnumerable<Book> books, string header)
        {
            Console.WriteLine($"\n{header}:");
            Console.WriteLine(books.Any() 
                ? string.Join("\n", books.Select(FormatBookInfo)) 
                : "No books found");
        }

        private string FormatBookInfo(Book book) => 
            $"{book.Title} ({book.Author}), {book.PublicationYear}, {book.Publisher}";

        public bool IsValidBookIndex(int index) => 
            index >= 0 && index < _catalog.Count;
    }

    public class LibraryInterface
    {
        private readonly LibraryManager _library = new LibraryManager();

        public void Run()
        {
            Console.WriteLine("Library Management System");
            while (true)
            {
                ShowMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddBook(); break;
                    case "2": ProcessBorrow(); break;
                    case "3": ProcessReturn(); break;
                    case "4": ShowNeverBorrowed(); break;
                    case "5": ShowCurrentBorrows(); break;
                    case "6": return;
                    default: ShowError(); break;
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("\nAvailable actions:");
            Console.WriteLine("1. Add new book");
            Console.WriteLine("2. Borrow book");
            Console.WriteLine("3. Return book");
            Console.WriteLine("4. Show never borrowed books");
            Console.WriteLine("5. Show current borrows");
            Console.WriteLine("6. Exit\n");
            Console.Write("Enter your choice: ");
        }

        private void AddBook()
        {
            var newBook = new Book();
            Console.WriteLine("\nAdding new book:");

            newBook.Author = GetInput("Author: ");
            newBook.Title = GetInput("Title: ");
            newBook.PublicationYear = int.Parse(GetInput("Publication year: "));
            newBook.Publisher = GetInput("Publisher: ");

            _library.AddBook(newBook);
            Console.WriteLine("Book added successfully!");
        }

        private void ProcessBorrow()
        {
            if (!HasBooks()) return;

            ShowAvailableBooks();
            var bookIndex = GetBookIndex("Enter book number to borrow: ");

            if (!_library.IsValidBookIndex(bookIndex)) return;

            var borrower = GetInput("Borrower name: ");
            _library.BorrowBook(bookIndex, borrower);
            Console.WriteLine("Book borrowed successfully!");
        }

        private void ProcessReturn()
        {
            if (!HasBooks()) return;

            var currentBorrows = _library.GetCurrentBorrows().ToList();
            if (!currentBorrows.Any())
            {
                Console.WriteLine("No books currently borrowed");
                return;
            }

            ShowBorrowedBooks(currentBorrows);
            var selection = GetBookIndex("Enter book number to return: ");

            if (selection < 0 || selection >= currentBorrows.Count) return;

            _library.ReturnBook(selection);
            Console.WriteLine("Book returned successfully!");
        }

        private void ShowNeverBorrowed() => 
            _library.DisplayBooks(_library.GetNeverBorrowedBooks(), "Never borrowed books");

        private void ShowCurrentBorrows() => 
            _library.DisplayBooks(_library.GetCurrentBorrows(), "Current borrows");

        private bool HasBooks()
        {
            if (_library.IsValidBookIndex(0)) return true;
            Console.WriteLine("No books in library");
            return false;
        }

        private void ShowAvailableBooks()
        {
            Console.WriteLine("\nAvailable books:");
            _library.DisplayBooks(_library.GetCurrentBorrows().ToList(), "Available");
        }

        private void ShowBorrowedBooks(IEnumerable<Book> borrowedBooks)
        {
            Console.WriteLine("\nBorrowed books:");
            var books = borrowedBooks.ToList();
            foreach (var (book, index) in books.Select((b, i) => (b, i)))
            {
                var record = book.BorrowHistory.Last();
                Console.WriteLine($"{index + 1}. {book.Title} ({book.Author}) - {record.BorrowerName}");
            }
        }

        private static string GetInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        private static int GetBookIndex(string prompt)
        {
            Console.Write(prompt);
            return int.Parse(Console.ReadLine()) - 1;
        }

        private static void ShowError() => Console.WriteLine("Invalid option!");
    }

    class Program
    {
        static void Main(string[] args) => new LibraryInterface().Run();
    }
}