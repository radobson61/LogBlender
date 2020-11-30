using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SLLogBlender
{
    public class LogLoader
    {
        private IEnumerable<string> loglines;
        private SortedList<string, LogLine> lines;
        private int linenumber;
        private FileInfo inputfile1;
        private FileInfo inputfile2;

        public LogLoader(IEnumerable<string> loglines)
        {
            this.loglines = loglines;
            lines = new SortedList<string, LogLine>();
            foreach(string line in loglines)
            {
                var logline = new LogLine(line);
                lines.Add(string.Concat(logline.getDateTime(),"+",linenumber++), logline );
            }
        }

        public LogLoader(FileInfo inputfile1, FileInfo inputfile2)
        {
            lines = new SortedList<string, LogLine>();
            this.inputfile1 = inputfile1;
            using (StreamReader sr = this.inputfile1.OpenText())
            {
                while(!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    var logline = new LogLine(line);
                    lines.Add(string.Concat(logline.getDateTime(), "+", linenumber++), logline);
                }
            }
            this.inputfile2 = inputfile2;
            using (StreamReader sr = this.inputfile2.OpenText())
            {
                while(!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    var logline = new LogLine(line);
                    lines.Add(string.Concat(logline.getDateTime(), "+", linenumber++), logline);
                }
            }
        }

        public SortedList<string, LogLine> Lines {
            get { return lines; }
            set { ; }
        }

        public SortedList<string, LogLine> SortedLines()
        {
            return lines;
        }

        public void SaveBlendedLogs(string outputfilename)
        {
            var outputfile = new FileInfo(outputfilename);
            using (StreamWriter sw = File.CreateText(outputfilename))
            {
                foreach (KeyValuePair<string, LogLine> line in lines)
                {
                    sw.WriteLine(line.Value.getLine());
                }

            }
        }
    }
}
