using System.Collections.Generic;
using System.Linq;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Parser.Services.Parsing
{
    internal class Parser<T> where T : class, ICsvMapping<CurrencyValue>, new()
    {
        private readonly CsvParserOptions _csvParserOptions = new CsvParserOptions(false, '|');
        private readonly CsvReaderOptions _csvReaderOptions = new CsvReaderOptions(new[] {"\r\n", "\r", "\n"}); 
        
        public IEnumerable<CurrencyValue> Parse(string input)
        {
            var csvParser = new CsvParser<CurrencyValue>(_csvParserOptions, new T());
            var parseResult = csvParser.ReadFromString(_csvReaderOptions, input);
            return parseResult.Where(r => r.IsValid).Select(r => r.Result);
        }
    }
}