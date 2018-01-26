using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbit.App.Models
{
    class OrderResult
    {
        public const int BUY = 1;
        public const int SELL = 2;
        
        public int Type;
        public string Pair;
        public decimal Price;
        public decimal Amount;
        public decimal TotalCost;
        public decimal Fee;
    }
}


