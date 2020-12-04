using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace K_Server
{
    class Client
    {
        public Client(TcpClient Client)
        {
            //Получаем сообщение клиента

            string Message = "";
            byte[] Buffer = new byte[1024];
            int Count;
            while ((Count = Client.GetStream().Read(Buffer, 0, Buffer.Length)) > 0)
            {
                Message += Encoding.UTF8.GetString(Buffer, 0, Count);

                if (Message.IndexOf("\r\n\r\n") >= 0 || Message.Length > 4096)
                {
                    Console.WriteLine(Message);
                    break;
                }
            }

            //
            //Здесь будет вся логика
            //
                        
            string Ans = "OK" + "\n\n";
            // Приведем строку к виду массива байт
            Buffer = Encoding.ASCII.GetBytes(Ans);


            // Ответ клиенту
            Client.GetStream().Write(Buffer, 0, Buffer.Length);
            // Закроем соединение
            Client.Close();
        }
    }
}
