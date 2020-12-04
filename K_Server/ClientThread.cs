using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace K_Server
{
    public static class ClientThread
    {
        public static void ClientThrd(Object StateInfo)
        {
            new Client((TcpClient)StateInfo);
        }
    }
}
