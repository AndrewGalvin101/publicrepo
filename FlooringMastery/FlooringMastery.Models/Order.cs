using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Models
{
    public class Order
    {
        public int OrderNumber;
        public string CustomerName;
        public string State;
        public decimal TaxRate;
        public string ProductType;
        public decimal Area;
        public decimal CostPerSquareFoot;
        public decimal LaborCostPerSquareFoot;
        public decimal MaterialCost;
        public decimal LaborCost;
        public decimal Tax;
        public decimal Total;
    }
}
