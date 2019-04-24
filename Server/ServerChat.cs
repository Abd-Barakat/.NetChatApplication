using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp;
using System.Windows.Forms;
namespace Server
{
    public class ServerChat : MarshalByRefObject, IChat
    {
        private int NumberOfClients;
        /// <summary>
        /// Initialized by client to point on event handler in client side.
        /// </summary>
        public event MessageArrivedEvent MessageArrived;
        /// <summary>
        /// Broadcast client message to all connected clients.
        /// </summary>
        /// <param name="Message"></param>
        public void BroadCastMessage(string Message)
        {
            Console.WriteLine(Message);
            InvokeMessage(Message);
        }
        /// <summary>
        /// Used by client to check if room is full or not and then throw the full exception.
        /// </summary>
        /// <returns></returns>
        public int GetNumOfClients()
        {
            return NumberOfClients;
        }
        /// <summary>
        /// Decrement number of clients, used by client when exiting the chat room to notify the server.
        /// </summary>
        public void DecrementNumOfClients()
        {
            NumberOfClients--;
            Console.WriteLine("Number of client : " + NumberOfClients);
        }
        /// <summary>
        /// Send message to all connected clients by fire event in client's side.
        /// </summary>
        /// <param name="Message"></param>
        private void InvokeMessage(string Message)
        {
            if (MessageArrived == null)
            {
                return;
            }
            MessageArrivedEvent listener = null;
            Delegate[] dels = MessageArrived.GetInvocationList();//get all delegates from clients to invoke them later
            NumberOfClients = dels.Length;
            foreach (Delegate del in dels)
            {
                try
                {
                    listener = (MessageArrivedEvent)del;
                    listener.Invoke(Message);
                }
                catch (Exception)//if user is not connected any more.
                {
                    MessageArrived -= listener;
                }
            }
        }
    }
}
