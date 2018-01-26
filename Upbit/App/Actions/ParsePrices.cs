using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Upbit.App.Events;
using Upbit.App.Scripts;
using ChromeRemote;
using Newtonsoft.Json;
using Upbit.App.Models;
using System.Diagnostics;
using Utils;


namespace Upbit.App.Actions
{
    class ParsePrices
    {
        public delegate void CompleteEventHandler(object sender, ParsePricesCompleteEventArgs e);
        public event CompleteEventHandler OnComplete;
        public delegate void ErrorEventHandler(object sender, EventArgs e);
        public event ErrorEventHandler OnError;

        private Dictionary<string, Price> Prices;

        readonly object locker = new object();
        private int CountTotal;
        private int CountComplete = 0;


        

        public void Start(Dictionary<string, Chrome> browsers)
        {
            
            this.CountTotal = browsers.Count;
            this.Prices = new Dictionary<string, Price>();

            // Add null first so the list is in the order
            foreach (KeyValuePair<string, Chrome> browser in browsers)
            {
                this.Prices.Add(browser.Key, null);
            }

            // Start finding prices async    
            Stopwatch start = Stopwatch.StartNew();
            foreach (KeyValuePair<string, Chrome> browser in browsers)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(state => GetPrices(browser.Value, browser.Key)));
            }
            Console.WriteLine("Time to load threads: " + start.Elapsed.TotalSeconds);
            
        }

        

        private void GetPrices(Chrome chrome, string pair)
        {
            Stopwatch timer = Stopwatch.StartNew();

            string pricesResult;
            if (pair.IndexOf("/KRW") != -1)
            {
                pricesResult = chrome.EvalAndGet(Commands.GetPriceKRW());
                pricesResult = pricesResult.Replace("__buffer__", "");
            }
            else
            {
                pricesResult = chrome.EvalAndGet(Commands.GetPriceUSDT());
                pricesResult = pricesResult.Replace("__buffer__", "");
                pricesResult = pricesResult.Replace(" KRW", "");
            }

            Price price = null;
            try
            {
                price = JsonConvert.DeserializeObject<Price>(pricesResult);
                price.Pair = pair;
            }
            catch (Exception)
            {
                this.OnError(this, new EventArgs());
            }
            
                                    
            lock (locker)
            {
                this.Prices[pair] = price;
                this.CountComplete++;

                Console.WriteLine(pair + " it self:" + timer.Elapsed.TotalSeconds);
                

                if (this.CountComplete == this.CountTotal)
                {
                    this.OnComplete(this, new ParsePricesCompleteEventArgs(this.Prices));
                    Utils.Debug.End("Parse Prices Finish:");
                }
                    
                
            }
        }

    }


}
