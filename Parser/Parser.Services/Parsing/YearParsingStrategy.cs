using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Parser.Services.Parsing
{
    internal class YearParsingStrategy: LinesParsingStrategy
    {
        private readonly Regex _currencyAmountRegex = new Regex(@"(^[0-9]+) (\w+$)", RegexOptions.Compiled);
        private const string DateParseFormat = "dd.MM.yyyy";
        private const int DateColumnIndex = 0;
        private const int CurrencyAmountRowIndex = 0;
        
        protected override IEnumerable<CurrencyValue> Parse(string[][] columns)
        {
            for (var i = 1; i < columns.Length; i++)
            {
                var date = DateTime.ParseExact(columns[i][DateColumnIndex], DateParseFormat, CultureInfo.InvariantCulture).Date;
                for (var j = 1; j < columns[i].Length; j++)
                {
                    var currency = _currencyAmountRegex.Match(columns[CurrencyAmountRowIndex][j]);
                    var value = columns[i][j];
                    yield return new CurrencyValue
                    {
                        Date = date,
                        Value = decimal.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture),
                        Amount = int.Parse(currency.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture),
                        Name = currency.Groups[2].Value,
                    };
                }
            }
        }
    }
}