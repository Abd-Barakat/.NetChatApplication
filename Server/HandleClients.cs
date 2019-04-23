using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;
namespace Server
{
    class HandleClients
    {
        private TcpClient Client;//hold client socket
        private int Client_Number;//store client number 
        Thread ChThreat;//to keep listening from client
        /// <summary>
        /// initialize client socket and client's number and create thread to listen from client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="number"></param>
        public HandleClients(TcpClient client, int number)
        {
            Client = client;
            Client_Number = number;
            CancellationTokenSource cancellation = new CancellationTokenSource();
            ChThreat = new Thread(Chat);
            ChThreat.IsBackground = true;
            ChThreat.Start(cancellation);//start listening from client
        }
        /// <summary>
        /// broadcasting message retrived from the client to all connected clients
        /// </summary>
        private void Chat(object cancellation)
        {
            CancellationTokenSource Cancellation = (CancellationTokenSource) cancellation;
            int Actual_size = 0;
            int BytesNumber;//hold number of bytes 
            byte[] Buffer;//locate memory to store all bytes received
            string ReceivedData = "";//locate memory to store received data as string 
            while (!Cancellation.IsCancellationRequested)
            {
                try
                {
                    Buffer = new byte[2]; //to store received Data from client
                    NetworkStream network = Client.GetStream();
                    BytesNumber = network.Read(Buffer, 0, Buffer.Length);//save header sent by client
                    while (Buffer != null)
                    {
                        if (BytesNumber == 0)//client is being closed 
                        {
                            Console.WriteLine("Client #" + Client_Number + " Closed");
                            Server.clients.Remove(this.Client);//remove client from the list
                            Cancellation.Cancel();
                            break;
                        }
                        byte[] Header = new byte[2];
                        Header = Buffer.ToList().GetRange(0, 2).ToArray();
                        int Header_size = BitConverter.ToInt16(Header, 0);
                        byte[] packet = new byte[Header_size];
                        if (Buffer.Length == 2)
                        {
                            Buffer = new byte[1024 * 20];
                            Actual_size = network.Read(Buffer, 0, Buffer.Length);
                        }
                        else
                        {
                            List<byte> temp = Buffer.ToList();
                            temp.RemoveRange(0, 2);
                            Buffer = temp.ToArray();
                            Actual_size = Buffer.Length;
                        }
                        if (Actual_size < Buffer.Length)//cut buffer to actual size
                        {

                            Array.Resize(ref Buffer, Actual_size);
                        }
                        if (Header_size == Actual_size)
                        {
                            ReceivedData = Encoding.UTF8.GetString(Buffer);//transform bytes into string
                            Console.WriteLine(ReceivedData);//print string 
                            for (int index = 0; index < Server.clients.Count; index++)//broadcast to all clients 
                            {
                                Server.clients[index].GetStream().Write(Buffer, 0, Buffer.Length);
                            }
                            Buffer = null;
                        }
                        else
                        {
                            List<byte> temp = Buffer.ToList();
                            packet = temp.GetRange(0, Header_size).ToArray();
                            temp.RemoveRange(0, Header_size);
                            Buffer = temp.ToArray();
                            ReceivedData = Encoding.UTF8.GetString(packet);//transform bytes into string
                            Console.WriteLine(ReceivedData);//print string 
                            for (int index = 0; index < Server.clients.Count; index++)//broadcast to all clients 
                            {
                                Server.clients[index].GetStream().Write(packet, 0, packet.Length);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    PrintErrors(ex);
                    Client.Close();
                    Server.clients.Remove(this.Client);
                    return;
                }
            }
            Client.Close();//close client socket
            Cancellation.Dispose();
        }/// <summary>
         /// Print Errors in Error.txt file in application folder
         /// </summary>
         /// <param name="ex"></param>
        private void PrintErrors(Exception ex)
        {
            string ErrorPath = System.IO.Directory.GetParent(@"..\..\..\").FullName;//return path of Application folder
            using (StreamWriter stream = new StreamWriter(ErrorPath + @"\Error.txt", true))
            {
                stream.WriteLine("Date : " + DateTime.Now.ToLocalTime());
                stream.WriteLine("Stack trace :");
                stream.WriteLine(ex.StackTrace);
                stream.WriteLine("Message :");
                stream.WriteLine(ex.Message);
                stream.WriteLine("---------------------------------------------------------------------------------------------------------------");
            }
        }
    }
}
