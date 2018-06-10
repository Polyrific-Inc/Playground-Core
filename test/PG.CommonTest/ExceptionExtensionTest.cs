// // Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using PG.Common.Extensions;
using Xunit;

namespace PG.CommonTest
{
    public class ExceptionExtensionTest
    {
        [Fact]
        public void GetLastInnerException()
        {
            var ex = new Exception("", new NotImplementedException());
            var innerEx = ex.GetLastInnerException();

            Assert.IsType<NotImplementedException>(innerEx);
        }

        [Fact]
        public void GetLastInnerExceptionMessage()
        {
            const string ex2Message = "Exception 2";

            var ex = new Exception("", new Exception(ex2Message));
            var innerEx = ex.GetLastInnerException();

            Assert.Equal(ex2Message, innerEx.Message);
        }

        [Fact]
        public void GetFlatExceptionMessage()
        {
            const string ex1Message = "Exception 1";
            const string ex2Message = "Exception 2";
            const string separator = ";";
            var flatMessage = $"{ex1Message}{separator}{ex2Message}";

            var ex = new Exception("Exception 1", new Exception(ex2Message));
            var exMessage = ex.GetFlatExceptionMessage(separator);

            Assert.Equal(flatMessage, exMessage);
        }

        [Fact]
        public void GetExceptionMessageList()
        {
            const string ex1Message = "Exception 1";
            const string ex2Message = "Exception 2";
            
            var ex = new Exception(ex1Message, new Exception(ex2Message));
            var msgList = ex.GetExceptionMessageList();

            Assert.Equal(2, msgList.Count);
            Assert.Equal(ex1Message, msgList[0]);
            Assert.Equal(ex2Message, msgList[1]);
        }
    }
}