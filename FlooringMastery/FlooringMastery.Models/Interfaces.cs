using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Models
{
    public interface IDataRepository
    {
        List<Order> LoadOrders(string orderDate,string path);
        void SaveOrder(Order o,string orderDate,string path);
        void RemoveOrder(List<Order> orders, Order o, string orderDate, string path);
    }
}
