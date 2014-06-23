using System;
using Xunit;
using FC.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FC.Framework.CouchCache.UnitTest
{
    public class UnitTest1
    {

        [Fact]
        public void TestUnixTimestamp()
        {
            var date = DateTime.Now;
            int timestamp1=1403492447;
            var timestamp2= date.ToUnixTimestamp();
            var resolveLocalDate2 = timestamp2.ToLocalDateTime();
            var resolveUTCDate2 = timestamp2.ToUtcDateTime();
             
            Assert.Equal(date.Date, resolveLocalDate2.Date);
            Assert.Equal(date.Hour, resolveLocalDate2.Hour);
            Assert.Equal(date.Minute, resolveLocalDate2.Minute);
            Assert.Equal(date.Second, resolveLocalDate2.Second);
            Assert.NotEqual(date, resolveUTCDate2);
            Assert.NotEqual(resolveLocalDate2, resolveUTCDate2);
        }
    }
}