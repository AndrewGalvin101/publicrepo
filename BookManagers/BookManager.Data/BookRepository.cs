using BookManager.Models;
using System.Collections.Generic;

namespace BookManager.Data
{
    public class BookRepository
    {
        public List<Book> MyLibrary = new List<Book>();  //TODO: use dictionary with ID as key and book object as value
        int ID = 0;

        public Book Create(Book book)
        {
            ID++;
            book.ID = ID;
            MyLibrary.Add(book);
            return book;
        }

        public List<Book> ReadAll()
        {
            return MyLibrary;
        }

        public Book ReadByID()
        {
            foreach (Book book in MyLibrary)
            {
                if (book.ID == ID)
                {
                    return book;
                }
            }
            return null;
        }

        public void Update(int ID, Book book)
        {
            foreach (Book bookToEdit in MyLibrary)
            {
                if (bookToEdit.ID == ID)
                {
                    bookToEdit.Title = book.Title;
                    bookToEdit.Author = book.Author;
                    bookToEdit.NumberOfPages = book.NumberOfPages;
                    bookToEdit.Price = book.Price;
                    break;
                }
            }
        }

        public void Delete(int ID)
        {
            foreach (Book bookToRemove in MyLibrary)
            {
                if (bookToRemove.ID == ID)
                {
                    int index = MyLibrary.IndexOf(bookToRemove);
                    MyLibrary.RemoveAt(index);
                    break;
                }
            }
        }
    }
}
