using System;

namespace AssocsConsole
{
    class Program
    {
        static void Main(string[] args)

        {

            IUserInteractions controller = new UserInteractions();            
            while (!controller.ShouldContinue())
            {
                Console.WriteLine(controller.GetPrompt());
                controller.HandleInput(Console.ReadLine());
            }
        }
    }
}
