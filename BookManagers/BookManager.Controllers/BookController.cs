using BookManager.Data;
using BookManager.Models;
using BookManager.View;
using System;
using System.Collections.Generic;

namespace BookManager.Controllers
{


    public class BookController
    {

        BookView bookview = new BookView();
        BookRepository myLibrary = new BookRepository();

        public void run()
        {
            Console.WriteLine("Welcome to your Book Repository.");
            while (true)
            {
                int UserChoice = bookview.GetMenuChoice();
                switch (UserChoice)
                {
                    case (1):
                        {
                            CreateBook();
                            break;
                        }
                    case (2):
                        {
                            EditBook();
                            break;
                        }
                    case (3):
                        {
                            SearchBooks();
                            break;
                        }
                    case (4):
                        {
                            DisplayBooks();
                            break;
                        }
                    case (5):
                        {
                            RemoveBook();
                            break;
                        }
                    case (6):
                        {
                            Console.WriteLine("\nThanks for using BookManager. Good bye.");
                            break;
                        }
                }
                if (UserChoice == 6) break;
                continue;
            }

        }

        private void CreateBook()
        {
            Book book = bookview.GetNewBookInfo();
            myLibrary.Create(book);
        }

        private void DisplayBooks()
        {
            List<Book> MyLibrary = myLibrary.ReadAll();
            foreach (Book book in MyLibrary)
            {
                bookview.DisplayBook(book);
            }
        }

        private void SearchBooks()
        {
            int id = bookview.SearchBook();
            List<Book> MyLibrary = myLibrary.ReadAll();
            foreach (Book book in MyLibrary)
            {
                if (book.ID == id)
                {
                    bookview.DisplayBook(book);
                    return;
                }
            }
            Console.WriteLine($"No book found for ID {id}.");

        }

        private void EditBook()
        {
            int ID;
            while (true)
            {
                Console.Write("What is the ID of the book you want to edit? ");
                string id = Console.ReadLine();
                bool valid = int.TryParse(id, out int id1);
                if (!valid)
                {
                    Console.WriteLine("Please enter a whole number. Try again.");
                    continue;
                }
                ID = id1;
                break;
            }
            List<Book> MyLibrary = myLibrary.ReadAll();
            Book book;
            foreach (Book bookToCheck in MyLibrary)
            {
                if (bookToCheck.ID == ID)
                {
                    book = bookToCheck;
                    bookview.EditBookInfo(book);
                    return;
                }
                else
                {
                    continue;
                }
            }
            Console.WriteLine("There is no book with that ID.");
        }

        private void RemoveBook()
        {
            int ID;
            while (true)
            {
                Console.Write("What is the ID of the book you want to delete? ");
                string id = Console.ReadLine();
                bool valid = int.TryParse(id, out int id1);
                if (!valid)
                {
                    Console.WriteLine("Please enter a whole number. Try again.");
                    continue;
                }
                ID = id1;
                break;
            }
            List<Book> MyLibrary = myLibrary.ReadAll();
            Book book;
            foreach (Book bookToCheck in MyLibrary)
            {
                if (bookToCheck.ID == ID)
                {
                    book = bookToCheck;
                    if (bookview.ConfirmRemoveBook(book))
                    {
                        MyLibrary.Remove(book);
                        Console.WriteLine("Book deleted.");
                        return;
                    }
                    else 
                    {
                        Console.WriteLine("Removal cancelled.");
                        return;
                    }
                }
                else
                {
                    continue;
                }
            }
            Console.WriteLine("There is no book with that ID.");
        }
    }
}
