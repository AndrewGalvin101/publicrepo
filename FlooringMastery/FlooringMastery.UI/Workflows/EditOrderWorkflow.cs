using FlooringMastery.BLL;
using FlooringMastery.Data;
using FlooringMastery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlooringMastery.BLL.Factory;
using static FlooringMastery.Models.Requests;

namespace FlooringMastery.UI.Workflows
{
    public class EditOrderWorkflow
    {
        public void Execute()
        {
            DataManager manager = DataManagerFactory.Create();
            DateTime OrderDate = ConsoleIO.RequestAndValidateDate("edit");
            FileLookupRequest request = manager.FileLookup(OrderDate);
            if (!request.success)
            {
                Console.WriteLine("Error: No orders found for that date.");
                Console.WriteLine();
                return;
            }

            string path = request.path;

            int OrderNumber = ConsoleIO.RequestAndValidateOrderNumber();

            List<Order> orders = request.orders;
            Order o;
            foreach (Order OrderToCheck in orders)
            {
                if (OrderToCheck.OrderNumber == OrderNumber)
                {
                    o = OrderToCheck;
                    ConsoleIO.PrintOrder(o, OrderDate);
                    EditOrderInfo(o);
                    ConsoleIO.PrintOrder(o, OrderDate);
                    if (ConsoleIO.ConfirmEditOrRemoveOrder("save"))
                    {
                        manager.FileSave(o, OrderDate.ToString("MMddyyyy"), path);
                        Console.WriteLine("Order saved.");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Order not saved.");
                        return;
                    }
                }
                else
                {
                    continue;
                }
            }
            Console.WriteLine("There is no order with that order number.");
        }

        public static void EditOrderInfo(Order o)
        {
            Console.WriteLine();
            Console.WriteLine("Please enter new values for only those order attributes you want to edit.\nIf you want to leave an attribute as it is, just hit enter.");
            Console.WriteLine();
            Console.WriteLine("Customer name?");
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
            {
                o.CustomerName = name;
            }

            while (true)
            {
                Console.WriteLine("State?");
                string state = Console.ReadLine();
                if (!string.IsNullOrEmpty(state) && !string.IsNullOrWhiteSpace(state))
                {
                    bool StateOK = Controllers.FindStateInTaxFile(o, state);
                    if (!StateOK) continue;
                }
                break;
            }

            ProductTypeRequest request = Controllers.ShowProductTypes();
            if (!request.success) return;

            while (true)
            {
                Console.WriteLine("Product?");
                string UserProduct = Console.ReadLine();

                if (!string.IsNullOrEmpty(UserProduct) && !string.IsNullOrWhiteSpace(UserProduct))
                {
                    bool found = Controllers.ProcessProductChoice(o, request.rows, UserProduct);
                    if (!found) continue;
                }
                break;
            }

            while (true)
            {
                Console.WriteLine("Area?");
                string UserArea = Console.ReadLine();
                if (!string.IsNullOrEmpty(UserArea) && !string.IsNullOrWhiteSpace(UserArea))
                {
                    bool NewAreaOK = Controllers.CheckUserArea(o, UserArea);
                    if (!NewAreaOK) continue;
                }
                break;
            }
            Controllers.CalculateRestOfOrder(o);
        }
    }
}
