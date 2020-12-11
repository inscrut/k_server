using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace K_Server
{
    class Client
    {
        private string login = "";
        private string passwd = "";
        public Client(TcpClient Client)
        {
            string _s_ip_cl = (Client.Client.LocalEndPoint as IPEndPoint).Address.ToString();

            string Message = "";
            byte[] Buffer = new byte[1024];
            int Count;

            int status = 0;

            string Ans = "";

            Console.WriteLine("Connected: " + _s_ip_cl);
            

            for (;;)
            {
                Message = "";
                //Получаем сообщение клиента
                try
                {
                    while ((Count = Client.GetStream().Read(Buffer, 0, Buffer.Length)) > 0)
                    {
                        Message += Encoding.UTF8.GetString(Buffer, 0, Count);

                        if (Message.Contains("\r\n\r\n") || Message.Length > 1024) break;
                    }
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(_s_ip_cl + ": (ERR):" + e.Message);
                    break;
                }
                catch
                {
                    Console.WriteLine(_s_ip_cl + ": (ERR): Непредвиденная ошибка");
                    break;
                }

                //
                //Здесь будет вся логика
                //
                Console.WriteLine(_s_ip_cl + ": " + Message);

                if (Message == "LOGIN\r\n\r\n")
                {
                    status = 1;
                    Ans = "OK\r\n\r\n";
                }

                else if(status == 1)
                {
                    login = Message.Replace("\r\n\r\n", "");
                    status = 2;
                    continue;
                }

                else if(status == 2)
                {
                    passwd = Message.Replace("\r\n\r\n", "");
                    status = 3;
                }

                if(status == 3)
                {
                    //check log and pass
                    var ch_pswd = BDConnector.getPasswd(login);

                    if (ch_pswd == null)
                    {
                        Ans = "DENY\r\n\r\n";
                    }
                    else if (ch_pswd == passwd)
                    {
                        //if ok
                        Ans = "ACCEPT\r\n\r\n";
                        status = 4;
                    }
                    else
                    {
                        Ans = "DENY\r\n\r\n";
                    }
                    
                }

                if (Message == "BYE\r\n\r\n") break;

                

                // Приведем строку к виду массива байт
                Buffer = Encoding.ASCII.GetBytes(Ans);
                // Ответ клиенту
                Client.GetStream().Write(Buffer, 0, Buffer.Length);

                if (status == 3) break; //close connection
                if (status == 4) status = 5; //auth'ed
            }
                       
            
            // Закроем соединение
            Client.Close();
        }
    }
}
