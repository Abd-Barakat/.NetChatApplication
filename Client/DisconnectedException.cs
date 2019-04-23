using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication
{
    class DisconnectedException:Exception
    {
        public DisconnectedException():base("You are not connected, please enter user name and server IP ")
        {

        }
    }
}
