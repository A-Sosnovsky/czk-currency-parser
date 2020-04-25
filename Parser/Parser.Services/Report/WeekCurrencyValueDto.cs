namespace Parser.Services.Report
{
    public class WeekCurrencyValueDto
    {
        public int WeekNumber { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public string CurrencyName { get; set; }
        public decimal MaxValue { get; set; }
        public decimal MinValue { get; set; }
        public decimal MedianValue { get; set; }
    }
}