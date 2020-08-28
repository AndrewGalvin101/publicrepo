using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringMastery.Models;

namespace FlooringMastery.Data
{
    public class FileData
    { 
        public class FileDataRepository : IDataRepository
        {
           // private static readonly string path = @"C:\Users\agalv\source\repos\";

            List<Order> _orders = new List<Order>();

            public List<Order> LoadOrders(string orderDate,string path)
            {
                if (File.Exists(path))
                {
                    _orders = ReadOrdersFromFile(path);
                    return _orders;
                }
                else return null;     
            }

            public void SaveOrder(Order o, string orderDate, string path)
            {
                List<Order> orders = new List<Order>();
                if (File.Exists(path))
                {
                    orders = ReadOrdersFromFile(path);
                    bool found = false;

                    Order OrderToReplace = orders.Where(order => order.OrderNumber == o.OrderNumber).First();
                    int index = orders.IndexOf(OrderToReplace);
                    if (index != -1)
                    {
                        orders[index] = o;
                        found = true;
                    }
/*
                    for (int i = 0; i < orders.Count(); i++)
                    {
                        if (orders[i].OrderNumber == o.OrderNumber)
                        {
                            orders[i] = o;
                            found = true;
                        }
                        if (found) break;
                    } */
                    if (!found) orders.Add(o);
                }
                else
                {
                    orders.Add(o);
                }
                WriteOrdersToFile(orders, path);
            }

            public void RemoveOrder(List<Order> orders, Order o, string orderDate, string path)
            {
                if (orders.Count == 0) File.Delete(path);
                else WriteOrdersToFile(orders, path);
            }
        }

        public static List<Order> ReadOrdersFromFile(string path)
        {
            string[] rows = File.ReadAllLines(path);
            List<Order> orders = new List<Order>();

            try
            {

                for (int i = 1; i < rows.Length; i++)
                {
                    Order o = new Order();
                    string[] columns = rows[i].Split(',');

                    o.OrderNumber = int.Parse(columns[0]);
                    o.CustomerName = columns[1];
                    o.State = columns[2];
                    o.TaxRate = decimal.Parse(columns[3]);
                    o.ProductType = columns[4];
                    o.Area = decimal.Parse(columns[5]);
                    o.CostPerSquareFoot = decimal.Parse(columns[6]);
                    o.LaborCostPerSquareFoot = decimal.Parse(columns[7]);
                    o.MaterialCost = decimal.Parse(columns[8]);
                    o.LaborCost = decimal.Parse(columns[9]);
                    o.Tax = decimal.Parse(columns[10]);
                    o.Total = decimal.Parse(columns[11]);
                    orders.Add(o);
                }
            }

            catch
            {
                Console.WriteLine("Data Format Error.");
            }

            return orders;
        }

        public static readonly string FileHeader = "OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total";

        public static string CreateCsvForOrder(Order o)
        {
            return string.Format($"{o.OrderNumber.ToString()},{o.CustomerName},{o.State},{o.TaxRate},{o.ProductType},{o.Area.ToString()},{o.CostPerSquareFoot.ToString()},{o.LaborCostPerSquareFoot.ToString()},{o.MaterialCost.ToString()},{o.LaborCost.ToString()},{o.Tax.ToString()},{o.Total.ToString()}");
        }

        public static void WriteOrdersToFile(List<Order> orders, string path)
        {
            if (File.Exists(path)) File.Delete(path);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(FileHeader);
                for (int i = 0; i < orders.Count(); i++)
                {
                    string LineToWrite = CreateCsvForOrder(orders[i]);
                    writer.WriteLine(LineToWrite);
                }
            }
        }
    }
}
