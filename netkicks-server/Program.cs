using System;

namespace netkicks_server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            Server server = new Server();     
            
            while (!Console.KeyAvailable)
            {
                Server.server.PollEvents();
            }

        }
    }
}
