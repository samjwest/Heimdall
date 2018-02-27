using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heimdall.Gateway.Core.Logging
{
    public class NullLogger : ILogger
    {
        private static readonly ILogger _instance = new NullLogger();

        public static ILogger Instance
        {
            get { return _instance; }
        }

        public IDisposable BeginScope<TState>(TState state) 
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }

    }
}
