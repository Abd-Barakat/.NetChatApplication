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
        public event MessageArrivedEvent MessageArrived;
        public void BroadCastMessage(string Message)
        {
            Console.WriteLine(Message);
            InvokeMessage(Message);
        }
        public int GetNumOfClients()
        {
            return NumberOfClients;
        }
        public void DecrementNumOfClients()
        {
            NumberOfClients--;
            Console.WriteLine("Number of client : " + NumberOfClients);
        }

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
                catch (Exception)
                {
                    MessageArrived -= listener;
                }
            }
        }
    }
}
