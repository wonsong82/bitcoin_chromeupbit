using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace SimpleSocket
{
    public class SocketClient
    {
        string SocketUrl;
        byte[] bytes = new byte[1024]; // Data buffer for incoming data.


        public SocketClient(string socketUrl)
        {
            this.SocketUrl = socketUrl;                                    
        }


        public void Open()
        {
            
        }

    }
}
