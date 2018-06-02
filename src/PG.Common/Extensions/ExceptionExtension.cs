// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;

namespace PG.Common.Extensions
{
    public static class ExceptionExtension
    {
        public static System.Exception GetLastInnerException(this System.Exception exception)
        {
            var ex = exception;
            while (true)
            {
                if (ex.InnerException == null)
                    return ex;

                ex = ex.InnerException;
            }
        }

        public static string GetLastInnerExceptionMessage(this System.Exception exception)
        {
            var ex = exception;
            while (true)
            {
                if (ex.InnerException == null)
                    return ex.Message;

                ex = ex.InnerException;
            }
        }

        public static string GetFlatExceptionMessage(this System.Exception exception, string separator)
        {
            var message = "";
            var ex = exception;
            while (true)
            {
                message += (string.IsNullOrEmpty(message) ? "" : separator) + ex.Message;

                if (ex.InnerException == null)
                    break;

                ex = ex.InnerException;
            }

            return message;
        }

        public static List<string> GetExceptionMessageList(this System.Exception exception, string separator)
        {
            var list = new List<string>();
            var ex = exception;
            while (true)
            {
                list.Add(ex.Message);

                if (ex.InnerException == null)
                    break;

                ex = ex.InnerException;
            }

            return list;
        }
    }
}
