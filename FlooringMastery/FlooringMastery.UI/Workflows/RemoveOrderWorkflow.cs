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
    class RemoveOrderWorkflow
    {
        public void Execute()
        {
            DataManager manager = DataManagerFactory.Create();
            DateTime OrderDate = ConsoleIO.RequestAndValidateDate("remove");
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
                    if (ConsoleIO.ConfirmEditOrRemoveOrder("remove"))
                    {
                        manager.FileRemove(orders, o, OrderDate.ToString("MMddyyyy"), path);
                        Console.WriteLine("Order removed.");
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
            Console.WriteLine("There is no order with that order number.");
        }
    }
}
