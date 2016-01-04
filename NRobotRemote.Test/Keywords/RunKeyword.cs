using System;
using System.Diagnostics;
using System.Threading;
using NRobotRemote.Exceptions;

namespace NRobotRemote.Test.Keywords
{
    /// <summary>
    /// Keywords used by the RunKeywordFixture
    /// </summary>
    public class RunKeyword
    {

        public void NoInputNoOutput() { }

        public void NoError() { }

        public void ThrowsException() {  throw new Exception("A regular exception");}

        public void ThrowsFatalException() { throw new FatalKeywordException("A fatal exception"); }

        public void ThrowsContinuableException() { throw new ContinuableKeywordException("A continuable exception"); }

        public void WritesTraceOutput()
        {
            Trace.WriteLine("First line");
            Trace.WriteLine("Second line");
        }

        /// <summary>
        /// Keyword used by multithread fixture
        /// </summary>
        public string MultiThreadKeyword(string wait)
        {
            Trace.WriteLine("Waiting");
            Thread.Sleep(Convert.ToInt32(wait));
            return "OK";
        }

    }
}
