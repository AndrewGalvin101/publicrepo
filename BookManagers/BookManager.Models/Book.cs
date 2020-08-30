namespace BookManager.Models
{
    public class Book
    {
        public int ID;
        public string Title;
        public string Author;
        public int NumberOfPages;
        public float Price;

        public Book(string title, string author, int pages, float price)
        {
            ID = 0;
            Title = title;
            Author = author;
            NumberOfPages = pages;
            Price = price;
        }
    }
}
