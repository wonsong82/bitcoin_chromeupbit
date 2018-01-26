using ChromeRemote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upbit.App.Events;
using System.Text.RegularExpressions;
using System.Threading;
using Upbit.App.Scripts;
using Utils;

namespace Upbit.App.Actions
{
    class AttachBrowsers
    {
        public delegate void CompleteEventHandler(object sender, AttachBrowsersCompleteEventArgs e);
        public event CompleteEventHandler OnComplete;
        public delegate void SocketConnectedEventHandler(object sender, SocketConnectedEventArgs e);
        public event SocketConnectedEventHandler OnSocketConnected;

        readonly object locker = new object();
        
        private List<string> Coins;
        private Dictionary<string, Chrome> Browsers; // this will returned
        
        private int CountAdded;
        private int CountTotal;


        private List<string> CoinsIndexed;

        public void Start(List<string> coins)
        {
            this.Coins = coins;
            
            // BTC / USDT, BTC / KRW, XRP / USDT, XRP / KRW...순서로 넣음
            this.Browsers = new Dictionary<string, Chrome>();
            foreach(string coin in coins)
            {
                this.Browsers.Add(coin + "/USDT", null);
                this.Browsers.Add(coin + "/KRW", null);
            }

            this.CountAdded = 0;
            this.CountTotal = this.Browsers.Count;

            Thread attach = new Thread(new ThreadStart(AttachThread));
            attach.Start();         
        }
        
                


        private void AttachThread()
        {
            // 리모트 주소 리스트
            string[] remoteAddresses = Properties.Settings.Default.RemoteAddresses.Split(',');

            List<string> keys = new List<string>();
            foreach (KeyValuePair<string, Chrome> browser in this.Browsers)
                keys.Add(browser.Key);
            

            foreach(string key in keys)
            {
                string pair = key;
                string[] pairUriArray = key.Split('/');
                string pairUri = String.Format("{1}-{0}", pairUriArray[0], pairUriArray[1]);

                // 순서대로 리모트에 접속해서 접속할 페이지가 있나 찾고 있으면 연결
                bool found = false;
                foreach(string remoteDebuggingUrl in remoteAddresses)
                {
                    if (!found)
                    {
                        List<RemoteSession> sessions = Chrome.GetAvailableSessions(remoteDebuggingUrl);
                        foreach (RemoteSession session in sessions)
                        {
                            if (!found)
                            {
                                if (session.url.IndexOf(pairUri) != -1) // Found
                                {
                                    this.Browsers[pair] = new Chrome();
                                    found = true;

                                    if (session.url.IndexOf("upbit.com/exchange") != -1) // just connect
                                    {
                                        StartConnectSession(this.Browsers[pair], session.webSocketDebuggerUrl);
                                    }
                                    else /// connect and go to page
                                    {                                        
                                        StartConnectSessionAndGotoPage(this.Browsers[pair], session.webSocketDebuggerUrl, pairUri);
                                        Thread.Sleep(7000);
                                    }
                                }
                            }
                        }
                    }
                }

                if (!found)
                {
                    throw new Exception(String.Format("Couldn't found a browser with {0} in url.", pairUri));
                }
            }
        }

        



        private void StartConnectSession(Chrome chrome, string webSocketDebuggerUrl)
        {
            new Thread(new ThreadStart(() =>
            {
                chrome.Connect(webSocketDebuggerUrl);

                lock (locker)
                {
                    int browserid = CountAdded + 1;
                    chrome.SetBrowserId(browserid);
                    
                    this.CountAdded++;
                    this.OnSocketConnected(this, new SocketConnectedEventArgs(this.CountAdded, this.CountTotal));

                    if (this.CountAdded == this.CountTotal)
                    {
                        this.OnComplete(this, new AttachBrowsersCompleteEventArgs(this.Browsers));
                    }
                }
            })).Start();
        }


        private void StartConnectSessionAndGotoPage(Chrome chrome, string webSocketDebuggerUrl, string pairUri)
        {
            new Thread(new ThreadStart(() =>
            {
                chrome.Connect(webSocketDebuggerUrl);
                
                chrome.Eval(Commands.NavigateTo("https://upbit.com/exchange?code=CRIX.UPBIT." + pairUri));

                lock (locker)
                {
                    int browserid = CountAdded + 1;
                    chrome.SetBrowserId(browserid);

                    this.CountAdded++;
                    this.OnSocketConnected(this, new SocketConnectedEventArgs(this.CountAdded, this.CountTotal));

                    if (this.CountAdded == this.CountTotal)
                    {
                        this.OnComplete(this, new AttachBrowsersCompleteEventArgs(this.Browsers));
                    }
                }
            })).Start();
        }
            
        

        

    }

    

}
