using System;
using ShutDown256.Extensions.Logging;

namespace ShutDown256.Extensions.Logging.Demo {
    class Program {
        static void Main(string[] args) {
            var logger = new DemoLogger();
            var str1 = "Foo";
            var str2 = "Bar";
            var i = 5;
            var d = 0.33;
            logger.LogInformation($"Simple message without any interpolation");
            logger.LogInformation($"Message '{str1}' without naming");
            logger.LogInformation($"Message '{str1:Foo}' with name Foo");
            logger.LogInformation($"Message '{str1:Foo,15}' with name Foo and padding");
            logger.LogInformation($"Message '{str1:Foo}' with name Foo and another message '{str2}' without name");
            logger.LogInformation($"Value '{i:_:00}' without naming but still formatted");
            logger.LogInformation($"Value '{i:number:00}' named number and formatted");
            logger.LogInformation($"Value '{d:FP,3:#.0}' named number and formatted");
            logger.LogInformation($"It is '{DateTime.Now:now:t}' and the message #{i:num:00} is '{str1:message}'");
            Console.ReadKey();
        }
    }
}
