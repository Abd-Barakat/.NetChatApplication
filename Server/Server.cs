using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
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
            try
            {
                IPHostEntry iPHost = Dns.GetHostEntry(Dns.GetHostName());//get current machine 

                BinaryServerFormatterSinkProvider serverProvider =
                     new BinaryServerFormatterSinkProvider();
                serverProvider.TypeFilterLevel =
                      System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

                serverChannel = new TcpServerChannel("ServerChannel", 9988, serverProvider);
                Console.WriteLine(GetAddress(iPHost));//print Ipv4 address 
                ChannelServices.RegisterChannel(serverChannel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(ServerChat), "Chat", WellKnownObjectMode.Singleton);
                Console.WriteLine("Server started");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server is already running");//print Ipv4 address 
                PrintErrors("Server is already running", ex);
              
            }

        }
        /// <summary>
        /// Print exception message in Error.txt file in the application folder.
        /// </summary>
        /// <param name="Message">
        /// error message
        /// </param>
        /// <param name="ex">
        /// exception to write it's stack trace
        /// </param>
        private static void PrintErrors(string Message, Exception ex)
        {
            try
            {
                string ErrorPath = System.IO.Directory.GetParent(@"..\..\..\").FullName;
                using (StreamWriter stream = new StreamWriter(ErrorPath + @"\Error.txt", true))
                {
                    stream.WriteLine("Date : " + DateTime.Now.ToLocalTime());
                    stream.WriteLine("Stack trace :");
                    stream.WriteLine(ex.StackTrace);
                    stream.WriteLine("Message :");
                    stream.WriteLine(Message);
                    stream.WriteLine("---------------------------------------------------------------------------------------------------------------");
                }
            }
            catch (Exception NewEx)
            {
                MessageBox.Show(NewEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            try
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
            catch (Exception ex)
            {
                PrintErrors(ex.Message, ex);
                return  null;
            }
        }
    }
}
