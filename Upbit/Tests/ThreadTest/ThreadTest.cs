using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Upbit.Tests.ThreadTest
{
    // http://www.albahari.com/threading/
    class ThreadTest
    {
        private int data = 0;
        private bool proc1Done;
        private bool proc2Done;

        public ThreadTest()
        {
            //SimpleThreadTest();           
            EventThreadTest();
            
        }



        private void SimpleThreadTest()
        {
            this.proc1Done = false;
            Thread proc1 = new Thread(new ThreadStart(Proc1));
            proc1.Start();

            this.proc2Done = false;
            Thread proc2 = new Thread(new ThreadStart(Proc2));
            proc2.Start();

            while (true)
            {
                if(this.proc1Done && this.proc2Done)
                {
                    Console.WriteLine("done");
                }
                else
                {
                    Console.WriteLine("working");
                }
                Thread.Sleep(10);
            }
        }

        private void Proc1()
        {
            Thread.Sleep(10000);
            this.proc1Done = true;            
        }

        private void Proc2()
        {
            Thread.Sleep(5000);
            this.proc2Done = true;
        }



        
        private void EventThreadTest()
        {
            Proc1 proc1 = new Proc1();

            proc1.OnStart += new Proc1.StartEventHandler(() =>
            {
                Console.WriteLine("Proc1 Started");
            });

            proc1.OnFinish += new EventHandler((object sender, EventArgs e) =>
            {
                this.proc1Done = true;
                Console.WriteLine("Proc1 Finished");
                if(this.proc1Done && this.proc2Done)
                {
                    Console.WriteLine("finish");
                }
            });
            proc1.Start();

            Proc2 proc2 = new Proc2();
            proc2.OnFinish += new Proc2.FinishEventHandler((object sender, FinishEventArgs e) =>
            {
                this.proc2Done = true;
                Console.WriteLine("Proc2 Finished");
                Console.WriteLine(e.message);
                if (this.proc1Done && this.proc2Done)
                {
                    Console.WriteLine("finish");
                }
            });
            proc2.Start();
            Console.WriteLine("started");


        }
        
    }

    class Proc1
    {
        // using generic event
        public event EventHandler OnFinish; 

        // custom event without eventargs
        public delegate void StartEventHandler();
        public event StartEventHandler OnStart;

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                OnStart(); // trigger onstart event
                Thread.Sleep(3000);
                OnFinish(this, EventArgs.Empty); // trigger genoeric event
            }));
            thread.Start();
            
        }
    }

    class Proc2
    {
        // custom event with custom eventargs (class)
        public delegate void FinishEventHandler(object sender, FinishEventArgs e);
        public event FinishEventHandler OnFinish;
        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(2000);
                OnFinish(this, new FinishEventArgs("finished1")); // trigger with custom eventargs
            }));
            thread.Start();

            // double thread from one class
            Thread thread2 = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(4000);
                OnFinish(this, new FinishEventArgs("finished2")); // trigger with custom eventargs
            }));
            thread2.Start();
        }
    }

    class FinishEventArgs : EventArgs
    {
        public string message; // this make data accessible by event listener

        public FinishEventArgs(string message)
        {
            this.message = message;
        }
    }
}
