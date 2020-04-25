using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Parser.Services.Parsing
{
    internal class DayParsingStrategy : LinesParsingStrategy
    {
        private const int DateIndex = 0;
        private const int AmountColumnIndex = 2;
        private const int CurrencyColumnIndex = 3;
        private const int RateColumnIndex = 4;
        private readonly Regex _dateRegex = new Regex(@"(\d{2} \w{3} \d{4})", RegexOptions.Compiled);
        private const string DateParseFormat = "dd MMM yyyy";

        protected override IEnumerable<CurrencyValue> Parse(string[][] columns)
        {
            var dateMatch = _dateRegex.Match(columns[DateIndex][DateIndex]);
            var date = DateTime.ParseExact(dateMatch.Value, DateParseFormat, CultureInfo.InvariantCulture).Date;

            for (var i = 2; i < columns.Length; i++)
            {
                yield return new CurrencyValue
                {
                    Date = date,
                    Amount = int.Parse(columns[i][AmountColumnIndex], NumberStyles.Any, CultureInfo.InvariantCulture),
                    Name = columns[i][CurrencyColumnIndex],
                    Value = decimal.Parse(columns[i][RateColumnIndex], NumberStyles.Any, CultureInfo.InvariantCulture)
                };
            }
        }
    }
}