using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace K_Server
{
    class Server
    {
        TcpListener Listener; // Объект, принимающий TCP-клиентов

        // Запуск сервера
        public Server(int Port, IPAddress ip)
        {
            // Создаем "слушателя" для указанного порта
            Listener = new TcpListener(ip, Port);
            Listener.Start(); // Запускаем его

            // В бесконечном цикле
            while (true)
            {
                // Принимаем новых клиентов
                //Listener.AcceptTcpClient();

                //new Client(Listener.AcceptTcpClient());

                // Принимаем нового клиента
                TcpClient Client = Listener.AcceptTcpClient();

                Console.WriteLine("Add new thread");

                // Создаем поток
                Thread Thread = new Thread(new ParameterizedThreadStart(ClientThread.ClientThrd));
                // И запускаем этот поток, передавая ему принятого клиента
                Thread.Start(Client);
            }
        }

        // Остановка сервера
        ~Server()
        {
            // Если "слушатель" был создан
            if (Listener != null)
            {
                // Остановим его
                Listener.Stop();
            }
        }
    }
}
