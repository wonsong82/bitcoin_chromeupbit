using ChromeRemote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbit.App.Events
{
    class AttachBrowsersCompleteEventArgs : EventArgs
    {
        public string Message = "";
        public Dictionary<string, Chrome> Data;

        public AttachBrowsersCompleteEventArgs(Dictionary<string, Chrome> data, string message = "")
        {
            this.Message = message;
            this.Data = data;
        }
    }
}
