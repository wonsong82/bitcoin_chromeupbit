using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbit.App.Models
{
    class CycleLog
    {
        public string cycle_started_time;
        public string cycle_ended_time;
        public double cycle_elapsed;

        public string coin1;
        public string coin2;

        public decimal start_amount;
        public decimal end_amount;
        public decimal profit_amount;

        public decimal coin1_high_price;
        public decimal coin1_low_price;
        public decimal coin1_low_price_usdt;
        public decimal coin1_premium;

        public decimal coin2_high_price;
        public decimal coin2_low_price;
        public decimal coin2_low_price_usdt;
        public decimal coin2_premium;

        public decimal premium;

        public double stage1_elapsed;
        public double stage2_elapsed;
        public double stage3_elapsed;
        public double stage4_elapsed;

        public decimal stage1_price;
        public decimal stage1_amount;
        public decimal stage1_cost;
        public decimal stage1_fee;

        public decimal stage2_price;
        public decimal stage2_amount;
        public decimal stage2_cost;
        public decimal stage2_fee;

        public decimal stage3_price;
        public decimal stage3_amount;
        public decimal stage3_cost;
        public decimal stage3_fee;

        public decimal stage4_price;
        public decimal stage4_amount;
        public decimal stage4_cost;
        public decimal stage4_fee;
    }
}
