using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChromeRemote;
using Upbit.App.Events;
using System.Threading;
using Utils;
using Upbit.App.Scripts;

namespace Upbit.App.Actions
{
    class SetBrowsers
    {
        public delegate void CompleteEventHandler(object sender, SetBrowsersCompleteEventArgs e);
        public event CompleteEventHandler OnComplete;

        readonly object locker = new object();
        private int TotalCount;
        private int CompleteCount = 0;

        
        public void Start(Dictionary<string, Chrome> browsers)
        {
            this.TotalCount = browsers.Count;
            

            foreach (KeyValuePair<string, Chrome> browser in browsers)
            {
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    SetBrowser(browser.Value, browser.Key);
                }));

                thread.Start();
            }
        }


        private void SetBrowser(Chrome chrome, string pair)
        {
            chrome.EvalUntil(Commands.PageLoaded(), 10);
            chrome.EvalAndGet(Commands.SetElements());

            bool logged = chrome.EvalAndGet(Commands.BuyButtonExists());
            //if (!logged)                    
            //    throw new Exception("Login required.");

            //chrome.Eval(Commands.GotoCoinPage(pair));
            //chrome.EvalUntil(Commands.PageLoaded(), 5);
            //Thread.Sleep(1000);

            lock (locker)
            {
                this.CompleteCount++;

                if (this.CompleteCount == this.TotalCount)
                {
                    this.OnComplete(this, new SetBrowsersCompleteEventArgs());
                }
            }
            
        }

       
    }
}
