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
        /// <summary>
        /// delegate to Client's event handler.
        /// </summary>
        public event MessageArrivedEvent MessageArrived;
        
        /// <summary>
        /// event handler for MessageArrived event of ServerObject.
        /// </summary>
        /// <param name="message"></param>
        public void ProxyBroadCastMessage(string message)
        {
            if (MessageArrived !=null)
            {
                MessageArrived(message);//fire event in client side.
            }
        }
    }
}
