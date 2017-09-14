using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShutDown256.Extensions.Logging.Demo {
    class DemoLogger : ILogger {
        public IDisposable BeginScope<TState>(TState state) => new DummyScope();
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
            var message = formatter(state, exception);
            var names = string.Join(", ", (state as IReadOnlyList<KeyValuePair<string, object>>).Select(kvp => kvp.Key));
            Console.WriteLine($"{message} => [{names}]");
        }

        class DummyScope : IDisposable {
            public void Dispose() { }
        }
    }
}
