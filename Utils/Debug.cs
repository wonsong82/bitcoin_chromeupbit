using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Debug
    {
        protected static DateTime StartTime;

        

                
        
        public static void Start()
        {
            Debug.StartTime = DateTime.Now;
        }

        public static double End(string message = "")
        {
            Console.WriteLine(message + (DateTime.Now - Debug.StartTime).TotalSeconds);
            return 0;
        }

        public static double End(bool returnValue)
        {
            return (DateTime.Now - Debug.StartTime).TotalSeconds;
        }
        

    }
}
