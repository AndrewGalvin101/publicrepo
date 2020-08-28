using FlooringMastery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlooringMastery.Models.Requests;

namespace FlooringMastery.BLL
{
    public static class Controllers
    {
        public static bool FindStateInTaxFile(Order o, string UserState)
        {
            bool StateFoundInFile = false;
            string TaxFile = @"Taxes.txt";
            if (!File.Exists(TaxFile))
            {
                Console.WriteLine("Error: Cannot find Taxes file.");
                return false;
            }

            try
            {
                string[] rows = File.ReadAllLines(TaxFile);
                
                for (int i = 1; i < rows.Length; i++)
                {
                    string[] columns = rows[i].Split(',');
                    if (columns[0] == UserState)
                    {
                        o.State = columns[0];
                        o.TaxRate = decimal.Parse(columns[2]);
                        StateFoundInFile = true;
                        break;
                    }
                }
                if (!StateFoundInFile)
                {
                    Console.WriteLine("Error: State not found in tax file.");
                }
            }
            catch
            {
                Console.WriteLine("Error: Data in tax file is not properly formatted.");
            }

            return StateFoundInFile;
        }

        public static ProductTypeRequest ShowProductTypes()
        {
            ProductTypeRequest request = new ProductTypeRequest();

            string ProductFile = @"Products.txt";
            if (!File.Exists(ProductFile))
            {
                Console.WriteLine("Error: Cannot find Products file.");
                request.success = false;
            }

            try
            {
                request.rows = File.ReadAllLines(ProductFile);
                Console.WriteLine("Please choose a product from the following:");
                string format = "{0,-12} | ${1,-20} | ${2,-30}";
                Console.WriteLine("PRODUCT      | Cost Per Square Foot  | Labor Cost Per Square Foot");
                for (int i = 1; i < request.rows.Length; i++)
                {
                    string[] columns = request.rows[i].Split(',');
                    Console.WriteLine(format, columns[0], columns[1], columns[2]);
                }
                Console.WriteLine();
                request.success = true;
            }
            catch
            {
                Console.WriteLine("Error. Data in product file is not properly formatted.");
                request.success = false;
            }
           
            return request;
        }

        public static bool ProcessProductChoice(Order o, string[] rows, string UserProduct)
        {
            bool found = false;
            for (int i = 1; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split(',');
                if (columns[0] == UserProduct)
                {
                    o.ProductType = columns[0];
                    o.CostPerSquareFoot = decimal.Parse(columns[1]);
                    o.LaborCostPerSquareFoot = decimal.Parse(columns[2]);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Console.WriteLine("Error: Product not found.");
            }
            return found;
        }

        public static void CalculateRestOfOrder(Order o)
        {
            o.MaterialCost = o.CostPerSquareFoot * o.Area;
            o.LaborCost = o.LaborCostPerSquareFoot * o.Area;
            o.Tax = (o.MaterialCost + o.LaborCost) * (o.TaxRate / 100);
            o.Total = o.MaterialCost + o.LaborCost + o.Tax;
        }

        public static bool CheckUserArea(Order o, string UserArea)
        {
            bool AreaOK = decimal.TryParse(UserArea, out o.Area);
            if (!AreaOK || o.Area < 100)
            {
                Console.WriteLine("Error: The minimum order is 100 square feet.");
                AreaOK = false;
            }
            return AreaOK;
        }

    }
}
