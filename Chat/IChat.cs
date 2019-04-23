using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{


    public interface IChat
    {
        void BroadCastMessage(string Message);
        event MessageArrivedEvent MessageArrived;
        void DecrementNumOfClients();
        int GetNumOfClients();
    }
}
