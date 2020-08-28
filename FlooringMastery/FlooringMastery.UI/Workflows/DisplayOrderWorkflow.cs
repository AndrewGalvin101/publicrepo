using FlooringMastery.BLL;
using FlooringMastery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlooringMastery.BLL.Factory;
using static FlooringMastery.Models.Requests;

namespace FlooringMastery.UI.Workflows
{
    class DisplayOrderWorkflow
    {
        public void Execute()
        {
            DataManager manager = DataManagerFactory.Create();

            DateTime OrderDate = ConsoleIO.RequestAndValidateDate("display");
            while (true)
            {
                FileLookupRequest request = manager.FileLookup(OrderDate);
                if (!request.success)
                {
                    Console.WriteLine(request.message);
                    return;
                }
                for (int i = 0; i < request.orders.Count(); i++)
                {
                    ConsoleIO.PrintOrder(request.orders[i], OrderDate);
                }
                Console.WriteLine();
                return;
            }
        }
    }
}
