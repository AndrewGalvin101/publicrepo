using FlooringMastery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Data
{
    public class TestData
    { 
        public class TestDataRepository : IDataRepository
        {
            /*
            public List<Order> LoadOrders(string orderDate, string path)
            {
                if (orderDate == OrderDate.ToString("MMddyyyy"))
                {
                    return _orders;
                }  
                else return null;
            } */

            public List<Order> LoadOrders(string orderDate, string path)
            {
                foreach (var key in d_orders.Keys)
                {
                    if (key == orderDate) return d_orders[key];
                }
                return null;
            }

            
            public void SaveOrder(Order o, string path)
            {
                bool found = false;
                for (int i = 0; i < _orders.Count(); i++)
                {
                    if (_orders[i].OrderNumber == o.OrderNumber)
                    {
                        _orders[i] = o;
                        found = true;
                    }
                    if (found) break;
                }
                if (!found) _orders.Add(o);
            } 

            public void SaveOrder(Order o, string orderDate, string path)
            {
                bool found = false;
                foreach (var key in d_orders.Keys)
                {
                    if (key == orderDate)
                    {
                        for (int i = 0; i < d_orders[key].Count; i++)
                        {
                            if (o.OrderNumber == d_orders[key][i].OrderNumber)
                            {
                                d_orders[key][i] = o;
                                found = true;
                                break;
                            }
                        }
                    }
                }
                if (!found)
                {
                    List<Order> orders = new List<Order> { o };
                    d_orders.Add(orderDate, orders);
                }
            }
            
            public void RemoveOrder (List<Order> orders, string path)
            {
                _orders = orders;
            } 

            public void RemoveOrder(List<Order> orders, Order o, string orderDate, string path)
            {
                foreach (var key in d_orders.Keys)
                {
                    if (key == orderDate)
                    {
                        d_orders[key].Remove(o);
                    }
                }
            }
        }

        private static DateTime OrderDate = new DateTime(2020, 6, 2);

        private static Order order1 = new Order()
        {
            OrderNumber = 1,
            CustomerName = "Wise",
            State = "OH",
            TaxRate = 6.25M,
            ProductType = "Wood",
            Area = 100.00M,
            CostPerSquareFoot = 5.15M,
            LaborCostPerSquareFoot = 4.75M,
            MaterialCost = 515.00M,
            LaborCost = 475.00M,
            Tax = 61.88M,
            Total = 1051.88M
        };

        private static Order order2 = new Order()
        {
            OrderNumber = 2,
            CustomerName = "Ward",
            State = "IN",
            TaxRate = 6.00M,
            ProductType = "Carpet",
            Area = 225.00M,
            CostPerSquareFoot = 2.25M,
            LaborCostPerSquareFoot = 2.10M,
            MaterialCost = 506.25M,
            LaborCost = 472.50M,
            Tax = 58.73M,
            Total = 1037.48M
        };

        private static List<Order> _orders = new List<Order> { order1, order2 };

        // experimental dictionary version follows:

        private static string d_orderDate = OrderDate.ToString("MMddyyyy");
        private static Dictionary<string, List<Order>> d_orders = new Dictionary<string, List<Order>>()
        {
            {d_orderDate, new List<Order> { order1, order2 } }
        };  
    }
}
