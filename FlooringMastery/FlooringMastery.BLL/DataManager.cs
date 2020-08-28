using FlooringMastery.Data;
using FlooringMastery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlooringMastery.BLL.Factory;
using static FlooringMastery.Models.Requests;

namespace FlooringMastery.BLL
{
    public class DataManager
    {
        private IDataRepository _dataRepository;

        public DataManager(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public FileLookupRequest FileLookup(DateTime OrderDate)
        {
            FileLookupRequest request = new FileLookupRequest();

            string orderDate = OrderDate.ToString("MMddyyyy");

            string filename = "Orders_" + orderDate;
            request.path = @filename + ".txt";
            request.orders = _dataRepository.LoadOrders(orderDate, request.path);

            if (request.orders == null || request.orders.Count == 0)
            {
                request.success = false;
                request.message = $"There are no orders for {OrderDate.ToString("MM/dd/yyyy")}";
            }
            else
            {
                request.success = true;
            }
            return request;
        }

        public void FileSave(Order o, string orderDate, string path)
        {
            _dataRepository.SaveOrder(o, orderDate, path);
        }

        public void FileRemove(List<Order> orders, Order o, string orderDate, string path)
        {
            orders.Remove(o);
            _dataRepository.RemoveOrder(orders, o, orderDate, path);
        }
    }
}
