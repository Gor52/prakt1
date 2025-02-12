using LibrarySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibrarySystem
{
   
    public abstract class User
    {
        public string Name { get; set; }
        protected List<Book> BorrowedBooks { get; set; } = new List<Book>();

        protected User(string name)
        {
            Name = name;
        }

        public abstract void ShowBorrowedBooks();

        public void BorrowBook(Book book)
        {
            if (book.Status == BookStatus.Available)
            {
                book.Status = BookStatus.Borrowed;
                BorrowedBooks.Add(book);
                Console.WriteLine($"Книга '{book.Title}' была успешно взята.");
            }
            else
            {
                Console.WriteLine($"Книга '{book.Title}' уже выдана.");
            }
        }

        public void ReturnBook(Book book)
        {
            if (BorrowedBooks.Contains(book))
            {
                book.Status = BookStatus.Available;
                BorrowedBooks.Remove(book);
                Console.WriteLine($"Книга '{book.Title}' была успешно возвращена.");
            }
            else
            {
                Console.WriteLine($"Вы не брали книгу '{book.Title}'.");
            }
        }
    }

   
    public class Librarian : User
    {
        public Librarian(string name) : base(name) { }

        public override void ShowBorrowedBooks()
        {
            Console.WriteLine("Библиотекари не имеют взятых книг.");
        }

        public void AddBook(List<Book> books, Book book)
        {
            books.Add(book);
            Console.WriteLine($"Книга '{book.Title}' добавлена в библиотеку.");
        }

        public void RemoveBook(List<Book> books, Book book)
        {
            if (books.Remove(book))
                Console.WriteLine($"Книга '{book.Title}' удалена из библиотеки.");
            else
                Console.WriteLine($"Книга '{book.Title}' не найдена в библиотеке.");
        }

        public void RegisterUser(List<User> users, User user)
        {
            users.Add(user);
            Console.WriteLine($"Пользователь '{user.Name}' зарегистрирован.");
        }

        public void ShowUsers(List<User> users)
        {
            Console.WriteLine("Список пользователей:");
            foreach (var user in users)
            {
                Console.WriteLine(user.Name);
            }
        }

        public void ShowBooks(List<Book> books)
        {
            Console.WriteLine("Список книг:");
            foreach (var book in books)
            {
                Console.WriteLine($"Книга: {book.Title} Статус: {book.Status}");
            }
        }
    }

    
    public class RegularUser : User
    {
        public RegularUser(string name) : base(name) { }

        public override void ShowBorrowedBooks()
        {
            Console.WriteLine($"Взятые книги пользователя '{Name}':");
            foreach (var book in BorrowedBooks)
            {
                Console.WriteLine(book.Title);
            }
        }
    }

   
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public BookStatus Status { get; set; }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
            Status = BookStatus.Available;
        }
    }

    public enum BookStatus
    {
        Available,
        Borrowed
    }
}
class Program
{
    static List<Book> books = new List<Book>();
    static List<User> users = new List<User>();

    static void Main(string[] args)
    {
        Console.WriteLine($"Каталог приложения: {AppDomain.CurrentDomain.BaseDirectory}");
        LoadData(); Console.WriteLine("Выберите роль: 1 - Библиотекарь, 2 - Пользователь");
        var role = Console.ReadLine();

        if (role == "1")
        {
            Librarian librarian = new Librarian("Библиотекарь");
            LibrarianMenu(librarian);
        }
        else if (role == "2")
        {
            Console.Write("Введите ваше имя: ");
            var userName = Console.ReadLine();
            RegularUser user = new RegularUser(userName);
            UserMenu(user);
        }

        SaveData();
    }

    static void LibrarianMenu(Librarian librarian)
    {
        while (true)
        {
            Console.WriteLine("\nМеню библиотекаря:");
            Console.WriteLine("1 - Добавить книгу");
            Console.WriteLine("2 - Удалить книгу");
            Console.WriteLine("3 - Зарегистрировать пользователя");
            Console.WriteLine("4 - Просмотреть список пользователей");
            Console.WriteLine("5 - Просмотреть список книг");
            Console.WriteLine("0 - Выход");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddBook(librarian);
                    break;
                case "2":
                    RemoveBook(librarian);
                    break;
                case "3":
                    RegisterUser(librarian);
                    break;
                case "4":
                    librarian.ShowUsers(users);
                    break;
                case "5":
                    librarian.ShowBooks(books);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    static void UserMenu(RegularUser user)
    {
        while (true)
        {
            Console.WriteLine("\nМеню пользователя:");
            Console.WriteLine("1 - Просмотреть доступные книги");
            Console.WriteLine("2 - Взять книгу");
            Console.WriteLine("3 - Вернуть книгу");
            Console.WriteLine("4 - Просмотреть взятые книги");
            Console.WriteLine("0 - Выход");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    user.ShowBorrowedBooks();
                    break;
                case "2":
                    BorrowBook(user);
                    break;
                case "3":
                    ReturnBook(user);
                    break;
                case "4":
                    user.ShowBorrowedBooks();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    static void AddBook(Librarian librarian)
    {
        Console.Write("Введите название книги: ");
        var title = Console.ReadLine();

        Console.Write("Введите автора книги: ");
        var author = Console.ReadLine();

        var book = new Book(title, author);
        librarian.AddBook(books, book);
    }

    static void RemoveBook(Librarian librarian)
    {
        Console.Write("Введите название книги для удаления: ");
        var title = Console.ReadLine();

        var bookToRemove = books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (bookToRemove != null)
        {
            librarian.RemoveBook(books, bookToRemove);
        }
        else
        {
            Console.WriteLine($"Книга с названием '{title}' не найдена.");
        }
    }

    static void RegisterUser(Librarian librarian)
    {
        Console.Write("Введите имя нового пользователя: ");
        var userName = Console.ReadLine();

        var user = new RegularUser(userName);
        librarian.RegisterUser(users, user);
    }

    static void BorrowBook(RegularUser user)
    {
        Console.Write("Введите название книги для взятия: ");
        var title = Console.ReadLine();

        var bookToBorrow = books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (bookToBorrow != null)
        {
            user.BorrowBook(bookToBorrow);
        }
        else
        {
            Console.WriteLine($"Книга с названием '{title}' не найдена.");
        }
    }

    static void ReturnBook(RegularUser user)
    {
        Console.Write("Введите название книги для возврата: ");
        var title = Console.ReadLine();

        var bookToReturn = books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (bookToReturn != null)
        {
            user.ReturnBook(bookToReturn);
        }
        else
        {
            Console.WriteLine($"Книга с названием '{title}' не найдена.");
        }
    }

    static void LoadData()
    {
    
        if (File.Exists("books.txt"))
        {
            var lines = File.ReadAllLines("books.txt");
            foreach (var line in lines)
            {
                var parts = line.Split(';');
                if (parts.Length == 3 && Enum.TryParse(parts[2], out BookStatus status))
                {
                    books.Add(new Book(parts[0], parts[1]) { Status = status });
                }
            }
        }

       
        if (File.Exists("users.txt"))
        {
            var lines = File.ReadAllLines("users.txt");
            foreach (var line in lines)
            {
                var parts = line.Split(';');
                if (parts.Length == 1)
                {
                    users.Add(new RegularUser(parts[0]));
                }
            }
        }
    }

    static void SaveData()
    {
     
        using (var writer = new StreamWriter("books.txt"))
        {
            foreach (var book in books)
            {
                writer.WriteLine($"Книга: {book.Title} Автор {book.Author} Статус: {book.Status}");
            }
        }

       
        using (var writer = new StreamWriter("users.txt"))
        {
            foreach (var user in users.OfType<RegularUser>())
            {
                writer.WriteLine($"Пользователи: \n{ user.Name}");
                
            }
        }
    }
}
