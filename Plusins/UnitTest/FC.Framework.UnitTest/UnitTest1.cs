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

            var timestamp = date.ToUnixTimestamp();
            var resolveLocalDate = timestamp.ToLocalDateTime();
            var resolveUTCDate = timestamp.ToUtcDateTime();
             
            Assert.Equal(date.Date, resolveLocalDate.Date);
            Assert.Equal(date.Hour, resolveLocalDate.Hour);
            Assert.Equal(date.Minute, resolveLocalDate.Minute);
            Assert.Equal(date.Second, resolveLocalDate.Second);
            Assert.NotEqual(date, resolveUTCDate);
            Assert.NotEqual(resolveLocalDate, resolveUTCDate);
        }
    }
}