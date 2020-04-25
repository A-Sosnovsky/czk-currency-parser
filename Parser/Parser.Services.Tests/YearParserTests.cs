using System;
using System.Linq;
using NUnit.Framework;
using Parser.Services.Parsing;

namespace Parser.Services.Tests
{
    public class YearParserTests
    {
        private readonly Parser<CurrencyValue> _target = new Parser<CurrencyValue>(new YearParsingStrategy());

        [Test]
        public void ParseByYearTest()
        {
            const string input = @"Date|1 AUD|1 BGN|100 INR|1000 IDR
02.01.2018|16.540|13.033|32.603|1.306|
03.01.2018|16.642|13.055|31.647|1.791|";
            var result = _target.Parse(input).ToArray();

            Assert.AreEqual(8, result.Length);
            Assert.AreEqual(result[0], new CurrencyValue{ Date = new DateTime(2018, 01, 02), Amount = 1, Name = "AUD", Value = (decimal)16.540});
            Assert.AreEqual(result[1], new CurrencyValue{ Date = new DateTime(2018, 01, 02), Amount = 1, Name = "BGN", Value = (decimal)13.033});
            Assert.AreEqual(result[2], new CurrencyValue{ Date = new DateTime(2018, 01, 02), Amount = 100, Name = "INR", Value = (decimal)32.603});
            Assert.AreEqual(result[3], new CurrencyValue{ Date = new DateTime(2018, 01, 02), Amount = 1000, Name = "IDR", Value = (decimal)1.306});
            Assert.AreEqual(result[4], new CurrencyValue{ Date = new DateTime(2018, 01, 03), Amount = 1, Name = "AUD", Value = (decimal)16.642});
            Assert.AreEqual(result[5], new CurrencyValue{ Date = new DateTime(2018, 01, 03), Amount = 1, Name = "BGN", Value = (decimal)13.055});
            Assert.AreEqual(result[6], new CurrencyValue{ Date = new DateTime(2018, 01, 03), Amount = 100, Name = "INR", Value = (decimal)31.647});
            Assert.AreEqual(result[7], new CurrencyValue{ Date = new DateTime(2018, 01, 03), Amount = 1000, Name = "IDR", Value = (decimal)1.791});
        }
    }
}