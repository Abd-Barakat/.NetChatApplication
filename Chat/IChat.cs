using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{

    /// <summary>
    /// Shared Interface between Client and Server.
    /// </summary>
    public interface IChat
    {
        /// <summary>
        /// Broadcast client message to all connected clients.
        /// </summary>
        /// <param name="Message"></param>
        void BroadCastMessage(string Message);
        /// <summary>
        /// Initialized by client to point on event handler in client side.
        /// </summary>
        event MessageArrivedEvent MessageArrived;
        /// <summary>
        /// Decrement number of clients, used by client when exiting the chat room to notify the server.
        /// </summary>
        void DecrementNumOfClients();
        /// <summary>
        /// Used by client to check if room is full or not and then throw the full exception.
        /// </summary>
        /// <returns></returns>
        int GetNumOfClients();
    }
}
