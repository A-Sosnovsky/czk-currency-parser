using System;
using System.Linq;
using NUnit.Framework;
using Parser.Services.Parsing;

namespace Parser.Services.Tests
{
    public class DayParserTest
    {
        private readonly Parser<CurrencyValue> _target = new Parser<CurrencyValue>(new DayParsingStrategy());
        
        [Test]
        public void ParseByDayTest()
        {
            const string input = @"27 Jul 2018 #143
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|16.255
Brazil|real|1|BRL|5.895
India|rupee|100|INR|32.087
Indonesia|rupiah|1000|IDR|1.529";
            var result = _target.Parse(input).ToArray();

            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(result[0], new CurrencyValue{ Date = new DateTime(2018, 07, 27), Amount = 1, Name = "AUD", Value = (decimal)16.255});
            Assert.AreEqual(result[1], new CurrencyValue{ Date = new DateTime(2018, 07, 27), Amount = 1, Name = "BRL", Value = (decimal)5.895});
            Assert.AreEqual(result[2], new CurrencyValue{ Date = new DateTime(2018, 07, 27), Amount = 100, Name = "INR", Value = (decimal)32.087});
            Assert.AreEqual(result[3], new CurrencyValue{ Date = new DateTime(2018, 07, 27), Amount = 1000, Name = "IDR", Value = (decimal)1.529});
        }
    }
}