using System;
using System.Net;

namespace K_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");

            Console.WriteLine("Starting server . . .");

            BDConnector.InitDB();
            
            try
            {
                new Server(80, ip);
            }
            catch
            {
                Console.WriteLine("Fail!");
            }

            Console.WriteLine("OK.");

        }
    }
}
