using TinyCsvParser.Mapping;

namespace Parser.Services.Parsing
{
    internal class DayValueMapping : CsvMapping<CurrencyValue>
    {
        public DayValueMapping()
        {
            MapProperty(3, x => x.Name);
            MapProperty(4, x => x.Value);
        }
    }
}