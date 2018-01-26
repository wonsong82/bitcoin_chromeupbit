using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbit.App.Models
{
    class CycleResult
    {
        public OrderResult PremiumBuy;
        public OrderResult PremiumSell;
        public OrderResult DiscountBuy;
        public OrderResult DiscountSell;

        public int Count = 0;
    }
}
