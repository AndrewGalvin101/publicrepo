using BookManager.Data;
using BookManager.Models;
using System;

namespace BookManager.View
{
    public class BookView
    {
        BookRepository bookRepository = new BookRepository();

        public int GetMenuChoice()
        {
            Console.WriteLine();
            Console.WriteLine("Please choose from the following menu:");
            Console.WriteLine("1. Create a new book.");
            Console.WriteLine("2. Edit an existing book.");
            Console.WriteLine("3. Search for a book.");
            Console.WriteLine("4. List all books.");
            Console.WriteLine("5. Remove a book.");
            Console.WriteLine("6. Quit.");
            Console.WriteLine();


            while (true)
            {
                Console.Write("Please enter the number of your selection: ");
                string userChoice = Console.ReadLine();
                bool isInt = int.TryParse(userChoice, out int UserChoice);
                if (!isInt || (1 > UserChoice || UserChoice > 6))
                {
                    Console.WriteLine("Please enter a number between 1 and 5. Try again.");
                    continue;
                }
                else
                {
                    return UserChoice;
                }
            }
        }

        public Book GetNewBookInfo()
        {
            string title = GetTitle();
            string author = GetAuthor();
            int pages = GetNumberOfPages();
            float price = GetPrice();

            Book book = new Book(title, author, pages, price);
            return book;

        }

        public void EditBookInfo(Book book)
        {
            Console.WriteLine("Please enter new values for only those book attributes you want to edit.\nIf you want to leave an attribute as it is, just hit enter.");
            string title = AskForTitle();
            if (!String.IsNullOrEmpty(title) && !String.IsNullOrWhiteSpace(title))
            {
                book.Title = title;
            }

            string author = AskForAuthor();
            if (!String.IsNullOrEmpty(author) && !String.IsNullOrWhiteSpace(author))
            {
                book.Author = author;
            }

            while (true)
            {
                string pages = AskForNumberOfPages();
                if (!String.IsNullOrEmpty(pages) && !String.IsNullOrWhiteSpace(pages))
                {
                    bool valid = int.TryParse(pages, out book.NumberOfPages);
                    if (!valid || book.NumberOfPages <= 0)
                    {
                        Console.WriteLine("Please enter a whole number greater than zero. Try again.");
                        continue;
                    }
                }
                break;
            }
            while (true)
            {
                string price = AskForPrice();
                if (!String.IsNullOrEmpty(price) && !String.IsNullOrWhiteSpace(price))
                {
                    bool valid = float.TryParse(price, out book.Price);
                    if (!valid || book.Price <= 0)
                    {
                        Console.WriteLine("Please enter a price with a decimal point. Try again.");
                        continue;
                    }
                }
                break;
            }
        }

        public void DisplayBook(Book book)
        {
            Console.WriteLine($"{book.ID}. '{book.Title}' by {book.Author}, {book.NumberOfPages} pages, ${book.Price}");
        }

        public int SearchBook()
        {
            while (true)
            {
                Console.Write("What ID number do you want to search for? ");
                string id = Console.ReadLine();
                bool valid = int.TryParse(id, out int ID);
                if (!valid)
                {
                    Console.WriteLine("Please enter a whole number. Try again.");
                    continue;
                }
                return ID;
            }
        }

        public bool ConfirmRemoveBook(Book book)
        {
            Console.Write($"Are you sure you want to delete {book.Title}? Enter Y for yes, N for no: ");
            string UserConfirm = Console.ReadLine().ToUpper();
            if (UserConfirm[0] == 'Y')
            {
                return true;
            }
            return false;
        }

        private string GetTitle()
        {
            while (true)
            {
                string title = AskForTitle();
                if (IsValidTitleOrAuthor(title))
                {
                    return title;
                }
            }
        }

        private string GetAuthor()
        {
            while (true)
            {
                string author = AskForAuthor();
                if (IsValidTitleOrAuthor(author))
                {
                    return author;
                }
            }
        }

        private int GetNumberOfPages()
        {
            while (true)
            {
                string pages = AskForNumberOfPages();
                bool valid = int.TryParse(pages, out int NumberPages);
                if (!valid || NumberPages <= 0)
                {
                    Console.WriteLine("Please enter a whole number greater than zero. Try again.");
                    continue;
                }
                else
                {
                    return NumberPages;
                }
            }
        }

        private float GetPrice()
        {
            while (true)
            {
                string price = AskForPrice();
                bool valid = float.TryParse(price, out float Price);
                if (!valid || Price <= 0)
                {
                    Console.WriteLine("Please enter a price with a decimal point. Try again.");
                    continue;
                }
                else
                {
                    return Price;
                }
            }
        }

        private string AskForPrice()
        {
            Console.Write("Price: ");
            return Console.ReadLine();
        }

        private string AskForTitle()
        {
            Console.Write("Please enter the title: ");
            return Console.ReadLine();
        }

        private string AskForAuthor()
        {
            Console.Write("Author: ");
            return Console.ReadLine();
        }

        private string AskForNumberOfPages()
        {
            Console.Write("Number of pages: ");
            return Console.ReadLine();
        }

        private bool IsValidTitleOrAuthor(string value)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine("You must enter at least one character. Please try again.");
                return false;
            }
            else
            {
                return true;
            }
        }

        //TODO: GetID method 
    }
}


