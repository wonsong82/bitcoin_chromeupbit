using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using WebSocket4Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace ChromeRemote
{
    public class Chrome2
    {
        public string websocketDebuggerUrl;
        protected WebSocket _socket;
        protected DateTime _timer;
        protected string[] _commands;
        protected object[] _messages;
        protected int _id = 1;
        protected int _messagesCount = 500;
        protected int _browserid;


        public Chrome2(int browserid = 1) {
            this._browserid = browserid;
            this._commands = new string[this._messagesCount];
            this._messages = new object[this._messagesCount];
        }
        public Chrome2(string websocketDebuggerUrl, int browserid = 1)
        {
            this._browserid = browserid;
            this.websocketDebuggerUrl = websocketDebuggerUrl;
            this._commands = new string[this._messagesCount];
            this._messages = new object[this._messagesCount];
        }

        public void SetBrowserId(int browserid)
        {
            this._browserid = browserid * 10000;
        }

        

        public static List<RemoteSession> GetAvailableSessions(string remoteDebuggingUrl="http://localhost:9222")
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(remoteDebuggingUrl + "/json");
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string result = sr.ReadToEnd();
            sr.Dispose();

            List<RemoteSession> sessionList = JsonConvert.DeserializeObject<List<RemoteSession>>(result);
            List<RemoteSession> filteredList = new List<RemoteSession>();
            foreach(RemoteSession session in sessionList)
            {
                if (session.type == "page")
                    filteredList.Add(session);
            }

            return filteredList;
        }


        


        public void Eval(string cmd)
        {
            Command command = new Command(cmd);
            cmd = JsonConvert.SerializeObject(command);

            string json = "{\"method\":\"Runtime.evaluate\", \"id\":100000001, \"params\":" + cmd + "}";

            _socket.Send(json);
        }

        
        public dynamic EvalAndGet(string cmd)
        {
            DateTime start = DateTime.Now;
            string originalCmd = cmd;

            // Command
            Command command = new Command(cmd);
            command.returnByValue = true;
            cmd = JsonConvert.SerializeObject(command);

            // Id
            this._id++; if (this._id >= this._messagesCount) this._id = 0;
            this._messages[this._id] = null;
            this._commands[this._id] = originalCmd;

            // Send
            string json = "{\"method\":\"Runtime.evaluate\", \"id\":" + (this._browserid + this._id) + ", \"params\":" + cmd + "}";


            bool sent = false;
            while (!sent)
            {
                try
                {
                    _socket.Send(json);
                    sent = true;
                }
                catch(Exception e){
                    Console.WriteLine(e.Message);
                    Thread.Sleep(5000);                    
                }
            }


            

            // Waiting
            while (_messages[_id] == null)
            {
                Thread.Sleep(10);

                if ((DateTime.Now - start).TotalSeconds > (double)5)
                {
                    throw new Exception(String.Format("EvalAndGet Timeout. cmd:{0}", cmd));
                }
            }
            
            return _messages[_id];
        }


        public void EvalUntil(string cmd, int timeoutInSecond = 5)
        {
            bool conditionMet = false;
            DateTime start = DateTime.Now;

            while (!conditionMet)
            {                
                bool result = (bool)EvalAndGet(cmd);

                if (result == true)
                {
                    conditionMet = true;
                    break;
                }

                Thread.Sleep(10);

                if ((DateTime.Now - start).TotalSeconds > (double)timeoutInSecond)
                {
                    throw new Exception(String.Format("EvalUntil Timeout. cmd:{0} expect:{1}", cmd, true));
                }
            }
        }

        public void EvalUntil(string cmd, bool expect, int timeoutInSecond = 5)
        {
            bool conditionMet = false;
            DateTime start = DateTime.Now;

            while (!conditionMet)
            {
                bool result = (bool)EvalAndGet(cmd);

                if (result == expect)
                {
                    conditionMet = true;
                    break;
                }

                Thread.Sleep(10);
                                
                if((DateTime.Now - start).TotalSeconds > (double)timeoutInSecond)
                {
                    throw new Exception(String.Format("EvalUntil Timeout. cmd:{0} expect:{1}", cmd, expect));
                }
            }
        }

        public void EvalUntil(string cmd, string expect, int timeoutInSecond = 5)
        {
            bool conditionMet = false;
            DateTime start = DateTime.Now;

            while (!conditionMet)
            {
                string result = (string)EvalAndGet(cmd);

                if (result == expect)
                {
                    conditionMet = true;
                    break;
                }

                Thread.Sleep(10);

                if ((DateTime.Now - start).TotalSeconds > (double)timeoutInSecond)
                {
                    throw new Exception(String.Format("EvalUntil Timeout. cmd:{0} expect:{1}", cmd, expect));
                }
            }
        }
        
        public void EvalUntil(string cmd, int expect, int timeoutInSecond = 5)
        {
            bool conditionMet = false;
            DateTime start = DateTime.Now;

            while (!conditionMet)
            {
                int result = (int)EvalAndGet(cmd);

                if (result == expect)
                {
                    conditionMet = true;
                    break;
                }

                Thread.Sleep(10);

                if ((DateTime.Now - start).TotalSeconds > (double)timeoutInSecond)
                {
                    throw new Exception(String.Format("EvalUntil Timeout. cmd:{0} expect:{1}", cmd, expect));
                }
            }
        }

        





        public void Connect(string socketUrl)
        {
            ManualResetEvent connectEvent = new ManualResetEvent(false);

            this._socket = new WebSocket(socketUrl);
            
            this._socket.Opened += delegate (System.Object o, EventArgs e) {
                connectEvent.Set();
            };

            this._socket.MessageReceived += delegate (System.Object o, MessageReceivedEventArgs e) {

                CommandResult result = JsonConvert.DeserializeObject<CommandResult>(e.Message);
                
                if(result.id != 100000001)
                {
                    int id = result.id % _browserid;

                    if (_browserid == 10000)
                    {
                        Console.WriteLine(result.id);
                    }

                    if (result.result != null)
                    {
                        JObject resultobj = (JObject)result.result;                        

                        if((string)resultobj["result"]["subtype"] == "error")
                        {
                            throw new Exception("Command returned but has an error while executing the script: cmd:" + _commands[id] + resultobj["result"]["description"]);
                        }

                        var value = resultobj["result"]["value"];

                        this._messages[id] = value;
                    }
                    else
                    {
                        throw new Exception("Command returned without a valid result value. cmd:" + _commands[id]);
                    }
                }
            };

            _socket.Open();
            connectEvent.WaitOne();
        }


        








        public string SendCommand(string cmd)
        {   
            WebSocket j = new WebSocket(this.websocketDebuggerUrl);
            ManualResetEvent waitEvent = new ManualResetEvent(false);
            ManualResetEvent closedEvent = new ManualResetEvent(false);
            string message = "";
            byte[] data;

            Exception exc = null;
            j.Opened += delegate (System.Object o, EventArgs e) {
                j.Send(cmd);
            };

            j.MessageReceived += delegate (System.Object o, MessageReceivedEventArgs e) {
                message = e.Message;
                waitEvent.Set();
            };

            j.Error += delegate (System.Object o, SuperSocket.ClientEngine.ErrorEventArgs e)
            {
                exc = e.Exception;
                waitEvent.Set();
            };

            j.Closed += delegate (System.Object o, EventArgs e)
            {
                closedEvent.Set();
            };

            j.DataReceived += delegate (System.Object o, DataReceivedEventArgs e)
            {
                data = e.Data;
                waitEvent.Set();
            };

            j.Open();

            waitEvent.WaitOne();
            if (j.State == WebSocketState.Open)
            {
                j.Close();
                closedEvent.WaitOne();
            }
            if (exc != null)
                throw exc;

            return message;
        }


        private void ts()
        {
            this._timer = DateTime.Now;
        }
        private void te(string message = "")
        {
            Console.WriteLine(message + (DateTime.Now - _timer).TotalSeconds);
        }


    }
}
