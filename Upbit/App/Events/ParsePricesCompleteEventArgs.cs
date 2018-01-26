using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upbit.App.Models;

namespace Upbit.App.Events
{
    class ParsePricesCompleteEventArgs
    {
        public string Message = "";
        public Dictionary<string, Price> Data;

        public ParsePricesCompleteEventArgs(Dictionary<string, Price> data, string message = "")
        {
            this.Data = data;
            this.Message = message;
        }
    }
}
