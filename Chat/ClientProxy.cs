using System;
using System.Windows.Forms;
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
            try
            {
                MessageArrived?.Invoke(message);//fire event in client side.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
