using BookManager.Controllers;

namespace BookManager
{
    class Program
    {
        static void Main(string[] args)
        {
            BookController bookController = new BookController();
            bookController.run();
        }
    }
}
