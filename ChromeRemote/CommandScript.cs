using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeRemote
{
    public class CommandScript
    {
        public string objectGroup = "console";
        public bool includeCommandLineAPI = true;
        public bool doNotPauseOnExceptions = false;
        public bool returnByValue = false;
        public bool awaitPromise = false;
        public string functionDeclaration;
        public string executionContextId = "document";

        public CommandScript(string func)
        {
            this.functionDeclaration = func;
        }
    }
}
