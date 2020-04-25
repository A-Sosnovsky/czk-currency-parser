using System;
using System.Collections.Generic;

namespace Parser.Services.Report
{
    public class WeekReportDto
    {
        public DateTime Date { get; set; }
        public IEnumerable<WeekCurrencyValueDto> Weeks { get; set; }
    }
}