using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeRemote
{
    public class Command
    {
        public string expression;
        public string objectGroup = "console";
        public bool includeCommandLineAPI = true;
        public bool doNotPauseOnExceptions = false;
        public bool returnByValue = false;
        public bool awaitPromise = false;

        public Command(string expression)
        {
            this.expression = expression;
        }
    }
}
