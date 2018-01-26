using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbit.App.Events
{
    class SocketConnectedEventArgs: EventArgs
    {
        public string Message = "";
        public int Count;
        public int Total;

        public SocketConnectedEventArgs(int count, int total, string message = "")
        {
            this.Count = count;
            this.Total = total;
            this.Message = message;
        }
    }
}
