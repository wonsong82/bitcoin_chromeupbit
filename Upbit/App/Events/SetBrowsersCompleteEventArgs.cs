using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbit.App.Events
{
    class SetBrowsersCompleteEventArgs
    {
        public string Message = "";

        public SetBrowsersCompleteEventArgs(string message = "")
        {
            this.Message = message;
        }
    }
}
