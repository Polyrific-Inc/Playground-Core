// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;

namespace PG.Common
{
    public interface ILogger
    {
        /// <summary>
        /// Log as Debug
        /// </summary>
        /// <param name="message">Debug message</param>
        void Debug(string message);

        /// <summary>
        /// Log as Debug
        /// </summary>
        /// <param name="message">Debug message</param>
        /// <param name="ex">Exception info</param>
        void Debug(string message, Exception ex);

        /// <summary>
        /// Log as Error
        /// </summary>
        /// <param name="message">Error message</param>
        void Error(string message);

        /// <summary>
        /// Log as Error
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="ex">Exception info</param>
        void Error(string message, Exception ex);

        /// <summary>
        /// Log as Info
        /// </summary>
        /// <param name="message">Info message</param>
        void Info(string message);

        /// <summary>
        /// Log as Info
        /// </summary>
        /// <param name="message">Info message</param>
        /// <param name="ex">Exception info</param>
        void Info(string message, Exception ex);

        /// <summary>
        /// Log as Warning
        /// </summary>
        /// <param name="message">Warning message</param>
        void Warning(string message);

        /// <summary>
        /// Log as Warning
        /// </summary>
        /// <param name="message">Warning message</param>
        /// <param name="ex">Exception info</param>
        void Warning(string message, Exception ex);
    }
}