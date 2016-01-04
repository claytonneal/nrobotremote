using System;

namespace NRobotRemote.Domain
{
    public class KeywordLoadingException : Exception
    {

        public KeywordLoadingException() { }

        public KeywordLoadingException(string message) : base(message) { }

        public KeywordLoadingException(string message, Exception inner) : base(message, inner) { }

    }
}
