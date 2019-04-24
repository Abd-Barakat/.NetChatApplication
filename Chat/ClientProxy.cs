using System;

namespace ChatApp
{
    public delegate void MessageArrivedEvent(string Msg);
    public class ClientProxy:MarshalByRefObject
    {
        /// <summary>
        /// Delegate to client's event handler.
        /// </summary>
        public event MessageArrivedEvent MessageArrived;
        
        /// <summary>
        /// event handler for MessageArrived event of ServerObject.
        /// </summary>
        /// <param name="message"></param>
        public void ProxyBroadCastMessage(string message)
        {
            MessageArrived?.Invoke(message);//fire event in client side.
        }
    }
}
