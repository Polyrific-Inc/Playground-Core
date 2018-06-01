// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;

namespace PG.Common
{
    public static class LoggerManager
    {
        private static Dictionary<string, WeakReference> _cache = new Dictionary<string, WeakReference>();

        public static string DefaultLoggerName = "default-logger";

        public static ILogger GetLogger()
        {
            return GetLogger(DefaultLoggerName);
        }

        public static ILogger GetLogger(string name)
        {
            if (_cache.TryGetValue(name, out var loggerRef))
            {
                return loggerRef.Target as ILogger;
            }

            return null;
        }

        public static void AddLogger(string name, ILogger logger)
        {
            _cache[name] = new WeakReference(logger);
        }
    }
}