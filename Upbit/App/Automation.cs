using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChromeRemote;
using Upbit.Tests.ThreadTest;
using Upbit.App.Events;
using Utils;
using System.Threading;
using Upbit.App.Models;

namespace Upbit.App
{
    
    public class Automation
    {

        private List<string> Coins; // Coins
        private Dictionary<string, Chrome> Browsers; // Browsers
        private Dictionary<string, Price> Prices;

        private bool ShouldRunCycle = true;



        

        public void Start()
        {
            // Read the coins from config and set them up
            this.Coins = SetCoins();
            AttachBrowsers();
            SetBrowsers();
            
            // Settings
            decimal cycleMoney = Properties.Settings.Default.CycleMoney;
            decimal profitThreshold = Properties.Settings.Default.profitThreshold * (decimal)0.01;
            decimal availablityMultiplier = 2;



            while (true) // endless loop
            {
                if (this.ShouldRunCycle) // Cycle play
                {
                    Debug.Start(); 

                    // get prices
                    ParsePrices();
                    if (this.ParsePricesError)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    

                    // find two coins to work with
                    CycleCoins cycleCoins = Actions.CycleCondition.FindCycleCoins(this.Coins, this.Prices);
                    
                    // get amounts and availblities
                    cycleCoins.CalculateAmounts(cycleMoney);
                    double parsePriceElapsed = Debug.End(true);


                    // Check conditions
                    bool conditionMet =
                        Actions.CycleCondition.CheckSameCoinCondition(cycleCoins) &&
                        Actions.CycleCondition.CheckProfitCondition(cycleCoins, profitThreshold) &&
                        Actions.CycleCondition.CheckAmountCondition(cycleCoins, availablityMultiplier);

                    Console.WriteLine("\n-----------------------------------------------------------------------------------"); // 
                    Console.WriteLine(String.Format("PremiumCoin:{0}, DiscountCoin:{1}, Profit:{2:P2}, ConditionMet:{4}, Elapsed:{3:0.000}sec", cycleCoins.PremiumCoin.Name, cycleCoins.DiscountCoin.Name, cycleCoins.Profit, parsePriceElapsed, conditionMet));
                    Console.WriteLine("PremiumAmt:{0:0.00000000}, Buyable: {1}, Sellable: {2} | DiscountAmt: {3:0.00000000}, Buyable: {4}, Sellable: {5}", cycleCoins.PremiumCoinAmount, cycleCoins.PremiumBuyableAmount, cycleCoins.PremiumSellableAmount, cycleCoins.DiscountCoinAmount, cycleCoins.DiscountBuyableAmount, cycleCoins.DiscountSellableAmount);
                    Console.WriteLine("-----------------------------------------------------------------------------------\n"); // 


                    if (!conditionMet)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }


                    //RunProcesses(cycleCoins);


                    Thread.Sleep(1000);// Test
                }

                else // Cycle stopped
                {
                    Thread.Sleep(1000);
                }
            }
        }

        



        private List<string> SetCoins()
        {
            string[] coins = Properties.Settings.Default.Coins.Split(',');
            return new List<string>(coins);
        }


        // ATTACH BROWSERS ////////////////////////////////////////
        private bool AttachBrowsersComplete;

        private void AttachBrowsers()
        {
            this.AttachBrowsersComplete = false;
            this.Browsers = null;

            // attach browsers
            Actions.AttachBrowsers attachBrowsers = new Actions.AttachBrowsers();
            attachBrowsers.OnComplete += AttachBrowsers_OnComplete;
            attachBrowsers.OnSocketConnected += AttachBrowsers_OnSocketConnected;
            attachBrowsers.Start(this.Coins);

            while (!this.AttachBrowsersComplete)
                Thread.Sleep(10);            
        }

        private void AttachBrowsers_OnSocketConnected(object sender, SocketConnectedEventArgs e)
        {
            Console.WriteLine(String.Format("Socket connected: {0}/{1}", e.Count, e.Total));
        }

        private void AttachBrowsers_OnComplete(object sender, AttachBrowsersCompleteEventArgs e)
        {
            this.Browsers = e.Data;
            this.AttachBrowsersComplete = true;                          
        }
        // END ATTACH BROSERS ////////////////////////////////////////////////



        // SET BROWSERS /////////////////////////////////////////////
        private bool SetBrowsersComplete;

        private void SetBrowsers()
        {
            //Debug.Start();
            this.SetBrowsersComplete = false;
            Actions.SetBrowsers setBrowsers = new Actions.SetBrowsers();
            setBrowsers.OnComplete += SetBrowsers_OnComplete;
            setBrowsers.Start(this.Browsers);
            while (!this.SetBrowsersComplete)
                Thread.Sleep(10);
        }
        private void SetBrowsers_OnComplete(object sender, SetBrowsersCompleteEventArgs e)
        {
            this.SetBrowsersComplete = true;
            //Debug.End(); // 2: 0.3 4: 0.3(sometimes 0.6)
            //Console.WriteLine("Browsers set complete");
        }



        // RUN CYCLE ///////////////////////////////////////////////////////////////
        private bool ParsePricesComplete;
        private bool ParsePricesError;

        private void ParsePrices()
        {
            this.ParsePricesComplete = false;
            this.ParsePricesError = false;
            this.Prices = null;

            Actions.ParsePrices parsePrices = new Actions.ParsePrices();
            parsePrices.OnComplete += ParsePrices_OnComplete;
            parsePrices.OnError += ParsePrices_OnError;
            parsePrices.Start(this.Browsers);
            while (!this.ParsePricesComplete)
                Thread.Sleep(5);
        }

        private void ParsePrices_OnError(object sender, EventArgs e)
        {
            this.ParsePricesError = true;
            this.ParsePricesComplete = true;
        }

        private void ParsePrices_OnComplete(object sender, ParsePricesCompleteEventArgs e)
        {
            this.Prices = e.Data;
            this.ParsePricesComplete = true;            
        }


        private bool RunProcessesComplete;

        private void RunProcesses(CycleCoins cycleCoins)
        {
            this.RunProcessesComplete = false;

            Actions.RunProcesses runProcesses = new Actions.RunProcesses();
            runProcesses.OnComplete += RunProcesses_OnComplete;
            runProcesses.Start(cycleCoins, this.Browsers);
            while (!this.RunProcessesComplete)
                Thread.Sleep(10);
        }

        private void RunProcesses_OnComplete(object sender, EventArgs e)
        {
            // cycle result
            this.RunProcessesComplete = true;
        }
    }
}
