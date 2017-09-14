using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Linq;

namespace ShutDown256.Extensions.Logging {
    public static class InterpolatedStringLoggerExtensions {
        public static void Log(this ILogger logger, LogLevel level, EventId eventId, Exception ex, FormattableString message) {
            if (logger == null) return;
            var a = ArrayPool<NamedObject>.Shared.Rent(message.ArgumentCount);
            var args = message.GetArguments();
            for (var i = 0; i < args.Length; i++) a[i] = a[i]?.Reset(args[i]) ?? new NamedObject(args[i]);
            var f = string.Format(message.Format, a);
            args = a.Take(args.Length).Where(no => !no.Ignore).Select(no => no.Obj).ToArray();
            for (var i = 0; i < message.ArgumentCount; i++) a[i].Obj = null;
            ArrayPool<NamedObject>.Shared.Return(a);

            switch (level) {
                case LogLevel.Trace:
                    logger.LogTrace(ex, f, args);
                    break;
                case LogLevel.Debug:
                    logger.LogDebug(ex, f, args);
                    break;
                case LogLevel.Information:
                    logger.LogInformation(ex, f, args);
                    break;
                case LogLevel.Warning:
                    logger.LogWarning(ex, f, args);
                    break;
                case LogLevel.Error:
                    logger.LogError(ex, f, args);
                    break;
                case LogLevel.Critical:
                    logger.LogCritical(ex, f, args);
                    break;
            }
        }

        class NamedObject : IFormattable {
            public NamedObject(object obj) {
                Obj = obj;
            }

            public object Obj { get; set; }
            public bool Ignore { get; private set; } = true;

            public NamedObject Reset(object obj) {
                Obj = obj;
                Ignore = true;
                return this;
            }

            public string ToString(string format, IFormatProvider formatProvider) {
                if (format == null) {
                    return Obj?.ToString();
                } else if (format.StartsWith("_:")) {
                    return string.Format($"{{0:{format.Substring(2)}}}", Obj);
                } else if (!((format[0] >= 'a' && format[0] <= 'z') || (format[0] >= 'A' && format[0] <= 'Z') || format[0] == '_')) {
                    return string.Format($"{{0:{format}}}", Obj);
                }
                Ignore = false;
                return $"{{{format}}}";
            }
        }

        public static void LogTrace(this ILogger logger, Exception ex, FormattableString message) => logger.Log(LogLevel.Trace, 0, ex, message);
        public static void LogTrace(this ILogger logger, FormattableString message) => logger.Log(LogLevel.Trace, 0, null, message);
        public static void LogTrace(this ILogger logger, EventId eventId, FormattableString message) => logger.Log(LogLevel.Trace, eventId, null, message);
        public static void LogTrace(this ILogger logger, EventId eventId, Exception ex, FormattableString message) => logger.Log(LogLevel.Trace, eventId, ex, message);

        public static void LogDebug(this ILogger logger, Exception ex, FormattableString message) => logger.Log(LogLevel.Debug, 0, ex, message);
        public static void LogDebug(this ILogger logger, FormattableString message) => logger.Log(LogLevel.Debug, 0, null, message);
        public static void LogDebug(this ILogger logger, EventId eventId, FormattableString message) => logger.Log(LogLevel.Debug, eventId, null, message);
        public static void LogDebug(this ILogger logger, EventId eventId, Exception ex, FormattableString message) => logger.Log(LogLevel.Debug, eventId, ex, message);

        public static void LogInformation(this ILogger logger, Exception ex, FormattableString message) => logger.Log(LogLevel.Information, 0, ex, message);
        public static void LogInformation(this ILogger logger, FormattableString message) => logger.Log(LogLevel.Information, 0, null, message);
        public static void LogInformation(this ILogger logger, EventId eventId, FormattableString message) => logger.Log(LogLevel.Information, eventId, null, message);
        public static void LogInformation(this ILogger logger, EventId eventId, Exception ex, FormattableString message) => logger.Log(LogLevel.Information, eventId, ex, message);

        public static void LogWarning(this ILogger logger, Exception ex, FormattableString message) => logger.Log(LogLevel.Warning, 0, ex, message);
        public static void LogWarning(this ILogger logger, FormattableString message) => logger.Log(LogLevel.Warning, 0, null, message);
        public static void LogWarning(this ILogger logger, EventId eventId, FormattableString message) => logger.Log(LogLevel.Warning, eventId, null, message);
        public static void LogWarning(this ILogger logger, EventId eventId, Exception ex, FormattableString message) => logger.Log(LogLevel.Warning, eventId, ex, message);

        public static void LogError(this ILogger logger, Exception ex, FormattableString message) => logger.Log(LogLevel.Error, 0, ex, message);
        public static void LogError(this ILogger logger, FormattableString message) => logger.Log(LogLevel.Error, 0, null, message);
        public static void LogError(this ILogger logger, EventId eventId, FormattableString message) => logger.Log(LogLevel.Error, eventId, null, message);
        public static void LogError(this ILogger logger, EventId eventId, Exception ex, FormattableString message) => logger.Log(LogLevel.Error, eventId, ex, message);

        public static void LogCritical(this ILogger logger, Exception ex, FormattableString message) => logger.Log(LogLevel.Critical, 0, ex, message);
        public static void LogCritical(this ILogger logger, FormattableString message) => logger.Log(LogLevel.Critical, 0, null, message);
        public static void LogCritical(this ILogger logger, EventId eventId, FormattableString message) => logger.Log(LogLevel.Critical, eventId, null, message);
        public static void LogCritical(this ILogger logger, EventId eventId, Exception ex, FormattableString message) => logger.Log(LogLevel.Critical, eventId, ex, message);

    }
}
