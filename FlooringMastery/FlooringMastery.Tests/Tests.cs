using FlooringMastery.BLL;
using FlooringMastery.Models;
using NUnit.Framework;
using System;
using static FlooringMastery.BLL.Factory;
using static FlooringMastery.Models.Requests;
using System.IO;

namespace FlooringMastery.Tests
{
    [TestFixture]
    public class Tests
    {
        [TestCase(1, "Andrew", "PA", 6.25, "Carpet", 225, 2.25, 2.10, 506.25, 472.50, 66.07, 1044.82, @"C:\Users\agalv\source\repos\Orders_06012020.txt")]
        public void AddOrderTest(int OrderNumber, string CustomerName, string State, decimal TaxRate, string ProductType, decimal Area, decimal CostPerSquareFoot, decimal LaborCostPerSquareFoot, decimal MaterialCost, decimal LaborCost, decimal Tax, decimal Total, string path)
        {
            DataManager manager = DataManagerFactory.Create();
            DateTime OrderDate = new DateTime(2020, 6, 1);

            FileLookupRequest request = manager.FileLookup(OrderDate);
      
            Assert.AreEqual(path,request.path);
            Assert.IsFalse(request.success);
            Assert.AreEqual(null, request.orders);

            Order o = new Order();
            o.OrderNumber = OrderNumber;
            o.CustomerName = CustomerName;
            o.State = State;
            o.TaxRate = TaxRate;
            o.ProductType = ProductType;
            o.Area = Area;
            o.CostPerSquareFoot = CostPerSquareFoot;
            o.LaborCostPerSquareFoot = LaborCostPerSquareFoot;
            o.MaterialCost = MaterialCost;
            o.LaborCost = LaborCost;
            o.Tax = Tax;
            o.Total = Total;

            manager.FileSave(o, request.path);

            request = manager.FileLookup(OrderDate);

            Assert.IsTrue(request.success);
            Assert.AreEqual(1, request.orders.Count);
            Assert.AreEqual(CustomerName, request.orders[0].CustomerName);
            Assert.AreEqual(Total, request.orders[0].Total);
        }

        [Test]
        public void EditOrderTest()
        {
            DataManager manager = DataManagerFactory.Create();
            DateTime OrderDate = new DateTime(2020, 6, 1);
            FileLookupRequest request = manager.FileLookup(OrderDate);

            Assert.AreEqual(1, request.orders.Count);
            Assert.AreEqual("Andrew", request.orders[0].CustomerName); 

            Order o = request.orders[0];
            o.CustomerName = "Galvin";

            manager.FileSave(o, request.path);

            request = manager.FileLookup(OrderDate);
            Assert.AreEqual(1, request.orders.Count);
            Assert.AreEqual("Galvin", request.orders[0].CustomerName);
        }
       
        [Test]
        public void RemoveOrderTest()
        {
            DataManager manager = DataManagerFactory.Create();
            DateTime OrderDate = new DateTime(2020, 6, 1); 
            FileLookupRequest request = manager.FileLookup(OrderDate);

            Assert.AreEqual(1, request.orders.Count);

            Order o = request.orders[0];
            manager.FileRemove(request.orders, o, request.path);

            request = manager.FileLookup(OrderDate);
            Assert.AreEqual(null, request.orders);
        }      
    }
}
