using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChromeRemote;
using Upbit.App;


namespace Upbit
{
    public partial class MainForm : Form
    {
        protected DateTime _timer;

        public MainForm()
        {
            InitializeComponent();

            Automation automation = new Automation();            
            automation.Start();


            
            /*
            Chrome chrome1 = new Chrome();
            Chrome chrome2 = new Chrome();
            Chrome chrome3 = new Chrome();
            Chrome chrome4 = new Chrome();
            List<RemoteSession> sessions = chrome1.GetAvailableSessions();

            
            //chrome1.websocketDebuggerUrl = sessions[0].webSocketDebuggerUrl;

            ts();
            chrome1.Connect(sessions[0].webSocketDebuggerUrl);
            te();
            ts();
            chrome2.Connect(sessions[0].webSocketDebuggerUrl);
            te();
            ts();
            chrome3.Connect(sessions[0].webSocketDebuggerUrl);
            te();
            ts();
            chrome4.Connect(sessions[0].webSocketDebuggerUrl);
            te();

            ts();
            Console.WriteLine(chrome1.Eval("aa = document.querySelector('.mainB .ty02>article>.tabB>ul a[title=\"USDT\"]');"));
            te();

            ts();
            Console.WriteLine(chrome1.Eval("aa.innerText;", true));
            te();
            */
    
        }


        



    }
}
