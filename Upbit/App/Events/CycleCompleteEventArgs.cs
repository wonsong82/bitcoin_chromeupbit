using ChromeRemote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upbit.App.Models;

namespace Upbit.App.Events
{
    class CycleCompleteEventArgs : EventArgs
    {
        public string Message = "";
        public CycleResult Data;

        public CycleCompleteEventArgs(CycleResult data, string message = "")
        {
            this.Message = message;
            this.Data = data;
        }
    }
}
