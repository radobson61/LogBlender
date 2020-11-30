using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections;
using System.Collections.Generic;
using SLLogBlender;
using System;
using System.IO;

namespace SLLogBlenderTests
{
    [TestClass]
    public class LogLoaderTests
    {
        FileInfo inputfile1;
        FileInfo inputfile2;
        string testline;

        [TestInitialize]
        public void Setup()
        {
            string inputfilename = Path.GetTempFileName();
            inputfile1 = new FileInfo(inputfilename);
            inputfile2 = new FileInfo(Path.GetTempFileName());
            testline = "[2020/09/16 09:50]  iמʞαʞů (inkaku Capalini): 2";
            StreamWriter sw = inputfile1.CreateText();
            sw.WriteLine("[2020/09/16 09:44]  Rads (Radslns Hutchence): Good morning");
            sw.Close();

            sw = inputfile2.CreateText();
                sw.WriteLine("[2020/09/16 09:50]  iמʞαʞů (inkaku Capalini): cleaning out a desk is just not fun, hopefully their rearranging is beneficial for you lol");
                sw.WriteLine(testline);
            sw.Close();
        }

        [TestMethod]
        public void Create_LogEntry_From_LogLineList()
        {
            var logline = "[2020/09/15 12:13]  person: message";
            LogLine target = new LogLine(logline);
            Assert.AreEqual( "[2020/09/15 12:13]  person: message", target.getLine());
        }

        [TestMethod]
        public void Create_LogEntry_Date_From_LogLineList()
        {
            var logline = "[2020/09/15 12:13]  person: message";
            LogLine target = new LogLine(logline);
            Assert.AreEqual( "2020/09/15 12:13", target.getDateTime());
        }

        [TestMethod]
        public void Create_2_LogEntries_From_A_CollectionOfLines()
        {
            var loglines = new LogLines();
            loglines.Add("[2020/09/16 09:50]  iמʞαʞů (inkaku Capalini): cleaning out a desk is just not fun, hopefully their rearranging is beneficial for you lol");
            loglines.Add("[2020/09/16 09:44]  Rads (Radslns Hutchence): Good morning");
            LogLoader target = new LogLoader(loglines);
            Assert.AreEqual( 2, target.Lines.Count);
        }

        [TestMethod]
        public void Sort_LogEntries_BasedOn_Date()
        {
            var loglines = new LogLines();
            loglines.Add("[2020/09/16 09:50]  iמʞαʞů (inkaku Capalini): cleaning out a desk is just not fun, hopefully their rearranging is beneficial for you lol");
            loglines.Add("[2020/09/16 09:44]  Rads (Radslns Hutchence): Good morning");
            LogLoader target = new LogLoader(loglines);
            var testvalue = target.SortedLines();
            Assert.AreEqual( "2020/09/16 09:44", testvalue.Values[0].getDateTime());
        }

        [TestMethod]
        public void Sort_Two_LogEntries_For_The_Same_In_The_Order_They_Appear_In_TheLogs()
        {
            var loglines = new LogLines();
            testline = "[2020/09/16 09:50]  iמʞαʞů (inkaku Capalini): 2";
            loglines.Add("[2020/09/16 09:50]  iמʞαʞů (inkaku Capalini): cleaning out a desk is just not fun, hopefully their rearranging is beneficial for you lol");    
            loglines.Add(testline);
            loglines.Add("[2020/09/16 09:44]  Rads (Radslns Hutchence): Good morning");
            LogLoader target = new LogLoader(loglines);
            var testvalue = target.SortedLines();
            Assert.AreEqual(testline, testvalue.Values[2].getLine() );
        }

        [TestMethod]
        public void LogLoader_LoadsValues_From_TwoFile()
        {
            LogLoader target = new LogLoader(inputfile1, inputfile2);
            var testvalue = target.SortedLines();
            Assert.AreEqual(testline, testvalue.Values[2].getLine());
        }

        [TestMethod]
        public void LogLoader_WritesOut_LinesToAFile()
        {
            var outputfilename = Path.GetTempFileName();
            LogLoader target = new LogLoader(inputfile1, inputfile2);
            target.SaveBlendedLogs(outputfilename);
            var outputfile = new FileInfo(outputfilename);
            using (StreamReader sr = outputfile.OpenText())
            {
                string line = "";
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                }
                Assert.AreEqual(testline, line);
            }
        }

        [TestMethod]
        public void Not_Really_A_Test()
        {
            var inputfilename1 = @"C:\Users\ra_do\AppData\Roaming\Firestorm_x64\radslns_hutchence\inkaku Capalini.txt";
            var inputfilename2 = @"G:\My Drive\SL\Chat\radslns_hutchence\radslns_hutchence\inkaku Capalini.txt";
            var outputfilename = @"G:\My Drive\SL\Chat\radslns_hutchence\inkaku Capalini.txt";
            var target = new LogLoader(new FileInfo(inputfilename1), new FileInfo(inputfilename2));
            target.SaveBlendedLogs(outputfilename);
        }

        private class LogLines : IEnumerable<string>
        {
            private List<string> loglines;
            public LogLines() 
            {
                loglines = new List<string>();
            }

            public IEnumerator<string> GetEnumerator()
            {
                foreach (string line in loglines)
                {
                    yield return line;
                }
            }

            internal void Add(string line)
            {
                loglines.Add(line);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
