# Logging
Experiment with using InterpolatedString for [structured logging](https://nblumhardt.com/2016/06/structured-logging-concepts-in-net-series-1/) in Asp.Net Core 2.0
Standard logging extensions work as [string.Format](https://msdn.microsoft.com/cs-cz/library/b1csw23d(v=vs.110).aspx) where instead of using number you use name of n-th parameter from the array of passed objects:
```c#
logger.LogInformation("Request finished in {ElapsedTime} with message {Message} and code {StatusCode}", elapsed, msg, code);
```
Instead these extensions enable you to use c# 6 interpolated strings:
```c#
logger.LogInformation($"Request finished in {elapsed:ElapsedTime} with message {msg:Message} and code {code:StatusCode}");
```
You can even format them:
```c#
logger.LogInformation($"Request finished in {elapsed:ElapsedTime:ff} ms with message {msg:Message,20} and code {code:StatusCode:000}");
```
String (`[a-zA-Z][a-zA-Z0-9]*`) after the first colon is used as a name for the object in the log's state. If you don't want to name the object and put into the state but you still want to format it, use underscore `_` instead of the name:
```c#
logger.LogInformation($"Request finished in {elapsed:_:ff} ms with code {code}");
```
