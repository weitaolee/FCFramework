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

        [Fact]
        public void TestDecimalAndDoubleFixed()
        {
            var dc1 = 12.123456;
            var dc2 = 12.1234465;


            var db1 = 12.123456;
            var db2 = 12.1234465;

            Assert.Equal(dc1.ToFixed(4), dc2.ToFixed(4));
            Assert.Equal(db1.ToFixed(4), db2.ToFixed(4));
        }
    }
}