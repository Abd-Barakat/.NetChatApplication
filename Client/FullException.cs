using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication
{
    class FullException:Exception
    {
        public FullException():base("The room is full")
        {

        }
    }
}
