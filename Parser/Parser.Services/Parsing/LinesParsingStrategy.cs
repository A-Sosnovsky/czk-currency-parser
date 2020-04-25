using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser.Services.Parsing
{
    internal abstract class LinesParsingStrategy : IParsingStrategy<CurrencyValue>
    {
        private readonly string[] _lineDelimiters = {"\r\n", "\r", "\n"};
        private const char ColumnDelimiter = '|';

        protected abstract IEnumerable<CurrencyValue> Parse(string[][] columns);
        public IEnumerable<CurrencyValue> Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Enumerable.Empty<CurrencyValue>();
            }
            
            var lines = input.Split(_lineDelimiters, StringSplitOptions.RemoveEmptyEntries);
            if (!lines.Any())
            {
                return Enumerable.Empty<CurrencyValue>();
            }
            
            var columns = lines.Select(l => l.Split(ColumnDelimiter, StringSplitOptions.RemoveEmptyEntries)).ToArray();
            return !columns.Any() ? Enumerable.Empty<CurrencyValue>() : Parse(columns);
        }
    }
}