using FlooringMastery.UI.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.UI
{
    public static class Menu
    {
        public static void Run ()
        {
            string border = "**********************************";

            while (true)
            {
                Console.WriteLine(border);
                Console.WriteLine("* Flooring Program");
                Console.WriteLine("*");
                Console.WriteLine("*1. Display Orders");
                Console.WriteLine("*2. Add an Order");
                Console.WriteLine("*3. Edit an Order");
                Console.WriteLine("*4. Remove an Order");
                Console.WriteLine("*5. Quit");
                Console.WriteLine("*");
                Console.WriteLine(border);

                string UserChoice = Console.ReadLine();
                bool IsAnInt = int.TryParse(UserChoice, out int userChoice);
                if (!IsAnInt || userChoice < 1 || userChoice > 5)
                {
                    Console.WriteLine("Please enter a number between 1 and 5.");
                    continue;
                }

                switch (userChoice)
                {
                    case 1:
                        DisplayOrderWorkflow displayWorkflow = new DisplayOrderWorkflow();
                        displayWorkflow.Execute();
                        break;
                    case 2:
                        AddOrderWorkflow addWorkflow = new AddOrderWorkflow();
                        addWorkflow.Execute();
                        break;
                    case 3:
                        EditOrderWorkflow editWorkflow = new EditOrderWorkflow();
                        editWorkflow.Execute();
                        break;
                    case 4:
                        RemoveOrderWorkflow removeWorkflow = new RemoveOrderWorkflow();
                        removeWorkflow.Execute();
                        break;
                    case 5:
                        return;
                }
            }
        }
    }
}
