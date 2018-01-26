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
    class AttachBrowsersCopy
    {
        public delegate void CompleteEventHandler(object sender, AttachBrowsersCompleteEventArgs e);
        public event CompleteEventHandler OnComplete;
        public delegate void SocketConnectedEventHandler(object sender, SocketConnectedEventArgs e);
        public event SocketConnectedEventHandler OnSocketConnected;

        readonly object locker = new object();
        
        private List<string> Coins;
        private Dictionary<string, Chrome> Browsers; // this will returned

        private List<string> CoinsIndexed;
        private int CountAdded;


        public void Start(List<string> coins)
        {
            this.Coins = coins;
            this.Browsers = new Dictionary<string, Chrome>();

            IndexCoins(); // BTC, XRP, ...

            Thread attach = new Thread(new ThreadStart(AttachThread));
            attach.Start();         
        }

        private void IndexCoins()
        {
            this.CountAdded = 0;
            this.CoinsIndexed = new List<string>();
            
            foreach(string coin in this.Coins)
            {
                this.CoinsIndexed.Add(coin + "/USDT");
                this.CoinsIndexed.Add(coin + "/KRW");
            }
        }


        private void AttachThread()
        {
            Dictionary<string, object> availableSesisons = GetAvailableSessions(this.Coins);

            if ((int)availableSesisons["count"] < (this.Coins.Count * 2))
                throw new Exception("Need more Chrome tabs to start");

            List<RemoteSession> upbitSessions = (List<RemoteSession>) availableSesisons["upbit"];
            List<RemoteSession> rawSessions = (List<RemoteSession>)availableSesisons["raw"];

            if(upbitSessions.Count > this.Coins.Count * 2)
            {
                int start = 0;
                int count = upbitSessions.Count - this.Coins.Count * 2;
                upbitSessions.RemoveRange(start, count);
            }
            
            foreach (RemoteSession session in upbitSessions)
                StartConnectSession(session);
            
            int countSessionsToAdd = this.Coins.Count * 2 - upbitSessions.Count;

            
            for(int i=0; i<countSessionsToAdd; i++)
            {
                Thread.Sleep(5000); // not to exceed request limit
                StartConnectSessionAndGotoUpbit(rawSessions[i]);
            }
        }


        private void StartConnectSession(RemoteSession session)
        {
            Thread connectSession = new Thread(new ThreadStart(() =>
            {
                Chrome chrome = new Chrome();
                chrome.Connect(session.webSocketDebuggerUrl);

                lock (locker)
                {
                    int browserid = CountAdded + 1;
                    chrome.SetBrowserId(browserid);

                    string name = this.CoinsIndexed[this.CountAdded];
                    this.Browsers.Add(name, chrome);
                    
                    this.CountAdded++;
                    this.OnSocketConnected(this, new SocketConnectedEventArgs(this.CountAdded, this.Coins.Count * 2));

                    if (this.CountAdded == this.Coins.Count * 2)
                    {
                        this.OnComplete(this, new AttachBrowsersCompleteEventArgs(this.Browsers));
                    }
                }
            }));

            connectSession.Start();
        }


        private void StartConnectSessionAndGotoUpbit(RemoteSession session)
        {
            Thread connectSession = new Thread(new ThreadStart(() =>
            {
                Chrome chrome = new Chrome();
                chrome.Connect(session.webSocketDebuggerUrl);
                
                chrome.Eval(Commands.NavigateTo("http://upbit.com/exchange"));

                lock (locker)
                {
                    int browserid = CountAdded + 1;
                    chrome.SetBrowserId(browserid);

                    string name = this.CoinsIndexed[this.CountAdded];
                    this.Browsers.Add(name, chrome);

                    this.CountAdded++;
                    this.OnSocketConnected(this, new SocketConnectedEventArgs(this.CountAdded, this.Coins.Count * 2));

                    if (this.CountAdded == this.Coins.Count * 2)
                    {
                        this.OnComplete(this, new AttachBrowsersCompleteEventArgs(this.Browsers));
                    }
                }
            }));

            connectSession.Start();
        }

  
        
        



        private Dictionary<string, object> GetAvailableSessions(List<string> coins)
        {
            List<RemoteSession> sessions = Chrome.GetAvailableSessions("http://localhost:9222"); // connected sessions
            List<RemoteSession> upbitSessions = new List<RemoteSession>(); // upbit pages
            List<RemoteSession> rawSessions = new List<RemoteSession>(); // specified pages in config
            //string chromeStartUrl = Properties.Settings.Default.ChromeStartUrl;//
            string chromeStartUrl = "a";//
            string upbitUrl = "upbit.com/exchange";

            foreach (RemoteSession session in sessions)
            {
                string url = session.url;

                if (url.IndexOf(upbitUrl) != -1)
                    upbitSessions.Add(session);

                else if (url.IndexOf(chromeStartUrl) != -1)
                    rawSessions.Add(session);                
            }

            Dictionary<string, object> availableSessions = new Dictionary<string, object>();
            availableSessions["upbit"] = upbitSessions;
            availableSessions["raw"] = rawSessions;
            availableSessions["count"] = upbitSessions.Count + rawSessions.Count;

            return availableSessions;
        }

        

    }

    

}
