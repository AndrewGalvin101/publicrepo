using FlooringMastery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlooringMastery.Models.Requests;
using FlooringMastery.BLL;

namespace FlooringMastery.UI
{
    public class ConsoleIO
    { 
        public static void PrintOrder(Order o, DateTime orderDate) 
        {
            Console.WriteLine();
            Console.WriteLine($"{o.OrderNumber} | {orderDate:MM/dd/yyyy}");
            Console.WriteLine(o.CustomerName);
            Console.WriteLine(o.State);
            Console.WriteLine($"Product : {o.ProductType}");
            Console.WriteLine($"Materials : {o.MaterialCost:C}");
            Console.WriteLine($"Labor : {o.LaborCost:C}");
            Console.WriteLine($"Tax : {o.Tax:C}");
            Console.WriteLine($"Total : {o.Total:C}");
        }

        public static DateTime RequestAndValidateDate(string action)
        {
            while (true)
            {
                Console.WriteLine($"What is the date of the order you want to {action}?");
                string UserDate = Console.ReadLine();
                bool IsDateTime = DateTime.TryParse(UserDate, out DateTime OrderDate);
                if (!IsDateTime)
                {
                    Console.WriteLine("Error: Please enter a date.");
                    continue;
                }
                return OrderDate;
            }
        }

        public static int RequestAndValidateOrderNumber()
        {
            while (true)
            {   
                Console.WriteLine("What is the order number?");
                string UserOrder = Console.ReadLine();
                bool valid = int.TryParse(UserOrder, out int OrderNumber);
                if (!valid)
                {
                    Console.WriteLine("Please enter a whole number. Try again.");
                    continue;
                }
                return OrderNumber;
            }
        }

        public static bool ConfirmEditOrRemoveOrder(string action)
        {
            Console.WriteLine($"Are you sure you want to {action} this order? Y/N");
            string UserChoice = Console.ReadLine().ToUpper();
            if (UserChoice[0] != 'Y') return false;
            return true;
        }
    }
}
