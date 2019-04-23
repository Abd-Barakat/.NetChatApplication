using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry Host = Dns.GetHostEntry(Dns.GetHostName());
            Console.WriteLine(GetAddress(Host).ToString());
            TcpListener server = new TcpListener( GetAddress(Host) , 5647);
            TcpClient ClientSocket = default(TcpClient);
            int ClientsCount = 0;
            server.Start();
            Console.WriteLine("Server Started");
            while (true)
            {
                ClientsCount++;
                ClientSocket = server.AcceptTcpClient();
                Console.WriteLine("Client #" + ClientsCount.ToString() + " started");
                HandleClients handle = new HandleClients(ClientSocket,ClientsCount);
            }
        }
        private static IPAddress GetAddress(IPHostEntry Host)
        {
            foreach (IPAddress iP in Host.AddressList)
            {
                if (iP.AddressFamily == AddressFamily.InterNetwork)
                {
                    return iP;
                }
            }
            return IPAddress.Parse("127.0.0.1");
        }
    }
}
