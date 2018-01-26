using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Upbit.App.Models;
using Upbit.App.Events;
using Upbit.App.Scripts;
using ChromeRemote;
using Newtonsoft.Json;
using Utils;

namespace Upbit.App.Actions
{
    class RunProcesses
    {
        public delegate void CompleteEventHandler(object sender, CycleCompleteEventArgs e);
        public event CompleteEventHandler OnComplete;


        private CycleResult CycleResult;

        readonly object locker = new object();
        private bool PremiumProcessComplete = false;
        private bool DiscountProcessComplete = false;
        private Dictionary<string, Chrome> Browsers;
        private CycleCoins CycleCoins;





        public void Start(CycleCoins cycleCoins, Dictionary<string, Chrome> browsers)
        {
            this.Browsers = browsers;
            this.CycleCoins = cycleCoins;
            this.CycleResult = new CycleResult();

            new Thread(new ThreadStart(() => // Premium Thread
            {
                PremiumProcess();
                if(this.CycleResult.Count == 4)
                {
                    this.OnComplete(this, new CycleCompleteEventArgs(this.CycleResult));
                }
            })).Start();

            new Thread(new ThreadStart(() => // Discount Thread
            {
                DiscountProcess();
                if (this.CycleResult.Count == 4)
                {
                    this.OnComplete(this, new CycleCompleteEventArgs(this.CycleResult));
                }
            })).Start();
        }

        private void PremiumProcess()
        {
            string buyPair = CycleCoins.PremiumCoin.UsdtPrice.Pair;
            //decimal buyPrice = CycleCoins.PremiumCoin.UsdtPrice.BuyPriceUSDT;
            string sellPair = CycleCoins.PremiumCoin.KrwPrice.Pair;
            decimal amount = CycleCoins.PremiumCoinAmount;
            
            PremiumBuy(buyPair, amount);
            PremiumSell(sellPair, amount);
        }

        private void DiscountProcess()
        {
            string buyPair = CycleCoins.DiscountCoin.KrwPrice.Pair;
            //decimal buyPrice = CycleCoins.DiscountCoin.KrwPrice.BuyPrice;
            string sellPair = CycleCoins.DiscountCoin.UsdtPrice.Pair;
            decimal amount = CycleCoins.DiscountCoinAmount;

            DiscountBuy(buyPair, amount);
            DiscountSell(sellPair, amount);
        }



        private void PremiumBuy(string pair, decimal amount)
        {
            Chrome chrome = Browsers[pair];

            // Order tab
            chrome.Eval(Commands.ClickBuyTab()); // tested
            
            // Check for enough fund
            decimal fiat = Properties.Settings.Default.CycleMoney * (decimal)1.5;
            bool available = chrome.EvalAndGet(Commands.CheckAmountAvailable(fiat)); // 0.01

            if (!available)
            {
                throw new Exception(String.Format("Not enough funds (USDT): Need {0}", fiat));
            }

            // Order
            chrome.Eval(Commands.Buy(9, amount));

            Debug.End("fill in and click buy: "); //

            chrome.EvalUntil(Commands.WaitConfirmBuy());

            string orderResult;
            orderResult = chrome.EvalAndGet(Commands.ConfirmBuy());
            orderResult = orderResult.Replace("__buffer__", "");
            Console.WriteLine(orderResult);
            
            OrderResult result = JsonConvert.DeserializeObject<OrderResult>(orderResult);
            result.Type = OrderResult.BUY;
            result.Pair = pair;
            result.Fee = result.TotalCost * (decimal)0.0025;

            Debug.End("order result: ");

            chrome.EvalUntil(Commands.ConfirmOrderWait());
            chrome.Eval(Commands.ConfirmOrder());

            Debug.End("confirm: ");

            
        }


        private void PremiumSell(string pair, decimal amount)
        {


        }

        private void DiscountBuy(string pair, decimal amount)
        {
            Chrome chrome = Browsers[pair];

            // Order tab
            chrome.Eval(Commands.ClickBuyTab());

            // Check for enough fund
            decimal fiat = Properties.Settings.Default.CycleMoney * (decimal)2 * 1000;
            bool available = chrome.EvalAndGet(Commands.CheckAmountAvailable(amount));
            
            if (!available)
            {
                throw new Exception(String.Format("Not enough funds (KRW): Need {0}", fiat));
            }

            // Order
            chrome.Eval(Commands.Buy(9, amount));
            
            
        }



        

        private void DiscountSell(string pair, decimal amount)
        {

        }
    }
}
