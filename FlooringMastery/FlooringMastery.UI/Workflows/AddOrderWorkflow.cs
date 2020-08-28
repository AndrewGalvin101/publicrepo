using FlooringMastery.BLL;
using FlooringMastery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static FlooringMastery.BLL.Factory;
using static FlooringMastery.Models.Requests;

namespace FlooringMastery.UI.Workflows
{
    class AddOrderWorkflow
    {
        public void Execute()
        {
            DataManager manager = DataManagerFactory.Create();
            while (true)
            {
                DateTime OrderDate = ConsoleIO.RequestAndValidateDate("add");

                if (OrderDate < DateTime.Now)
                {
                    Console.WriteLine("Date must be in the future.");
                    continue;
                }

                Order o = new Order();

                Console.WriteLine("Customer Name?");
                o.CustomerName = Console.ReadLine();

                while (true)
                {
                    Console.WriteLine("State?");
                    string UserState = Console.ReadLine();
                    bool StateOK = Controllers.FindStateInTaxFile(o, UserState);
                    if (!StateOK)
                    {
                        continue;
                    }
                    break;
                }

                while (true)
                {
                    ProductTypeRequest productRequest = Controllers.ShowProductTypes();
                    if (!productRequest.success)
                    {
                        continue;
                    }
                    while (true)
                    {
                        if (!GetProductType(o, productRequest.rows))
                        {
                            continue;
                        }
                        break;
                    }
                    break;
                }

                while (true)
                {
                    Console.WriteLine("Area?");
                    string UserArea = Console.ReadLine();
                    bool AreaOK = Controllers.CheckUserArea(o, UserArea);
                    if (!AreaOK)
                    {
                        continue;
                    }
                    break;
                }

                Controllers.CalculateRestOfOrder(o);

                ConsoleIO.PrintOrder(o, OrderDate);

                Console.WriteLine("Do you want to add this order? Y/N");
                string AddOrNot = Console.ReadLine().ToUpper();
                if (AddOrNot[0] != 'Y')
                {
                    break;
                }

                FileLookupRequest request = manager.FileLookup(OrderDate);

                if (!request.success)
                {
                    o.OrderNumber = 1;
                    manager.FileSave(o, OrderDate.ToString("MMddyyyy"), request.path);
                    Console.WriteLine($"Order file created for {OrderDate:MM/dd/yyyy}.");
                    break;
                }
                else
                {
                    List<Order> Orders = request.orders;
                    o.OrderNumber = Orders[(Orders.Count() - 1)].OrderNumber + 1;
                    manager.FileSave(o, OrderDate.ToString(), request.path);
                    Console.WriteLine($"Order added to file for {OrderDate:MM/dd/yyyy}.");
                    break;
                }
            }
        }

        public static bool GetProductType(Order o, string[] rows)
        {
            Console.WriteLine();
            Console.WriteLine("Product?");
            string UserProduct = Console.ReadLine();
            bool success = Controllers.ProcessProductChoice(o, rows, UserProduct);
            return success;
        }
    }
}
