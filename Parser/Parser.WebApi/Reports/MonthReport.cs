using System;
using System.Collections.Generic;

namespace Parser.WebApi.Reports
{
    public class MonthReport
    {
        public DateTime Date { get; set; }
        public IEnumerable<MonthReportWeekPeriod> WeekPeriods { get; set; }
    }
}