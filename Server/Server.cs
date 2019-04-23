using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Net;
using System.Net.Sockets;
namespace Server
{
    class Server
    {
        private static TcpServerChannel serverChannel;
        /// <summary>
        /// Initialize server & create thread to listen from server
        /// </summary>
        static void Main(string[] args)
        {
            IPHostEntry iPHost = Dns.GetHostEntry(Dns.GetHostName());
            Console.WriteLine(GetAddress(iPHost));

            BinaryServerFormatterSinkProvider serverProvider =
                 new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel =
                  System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

            serverChannel = new TcpServerChannel("ServerChannel", 9988 , serverProvider);
            ChannelServices.RegisterChannel(serverChannel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ServerChat), "Chat", WellKnownObjectMode.Singleton);
            Console.WriteLine("Server started");
            Console.ReadLine();
        }
        /// <summary>
        /// return current IP address for server
        /// </summary>
        /// <param name="Host"></param>
        /// <returns>
        /// <para>
        /// IPAddress 
        /// </para>
        /// </returns>
        private static IPAddress GetAddress(IPHostEntry Host)
        {
            foreach (IPAddress iP in Host.AddressList)
            {
                if (iP.AddressFamily == AddressFamily.InterNetwork)//return IPv4 address only
                {
                    return iP;
                }
            }
            return IPAddress.Parse("127.0.0.1");
        }
    }
}
