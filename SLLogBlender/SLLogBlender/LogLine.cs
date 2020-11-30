using System;
using System.Collections.Generic;
using System.Text;

namespace SLLogBlender
{
    public class LogLine
    {
        private string logline;

        public LogLine(string logline)
        {
            this.logline = logline;
        }

        public string getLine()
        {
            return this.logline;
        }

        public string getDateTime()
        {
            if (logline.Length > 16)
            {
                return logline.Substring(1, 16);
            }

            if (logline.Length < 16)
            {
                return logline.Substring(1, logline.Length -1);
            }

            return "";
            
        }
    }
}
