using System.Collections.Generic;

namespace Parser.WebApi.Reports
{
    public class MonthReportWeekPeriod
    {
        public int WeekStartDay { get; set; }
        public int WeekEndDay { get; set; }
        public IEnumerable<WeekPeportRow> Rows { get; set; }
    }
}