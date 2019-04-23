using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    public delegate void MessageArrivedEvent(string Msg);
    public class ClientProxy:MarshalByRefObject
    {
        public event MessageArrivedEvent MessageArrived;
        
        public void ProxyBroadCastMessage(string message)
        {
            if (MessageArrived !=null)
            {
                MessageArrived(message);
            }
        }
    }
}
