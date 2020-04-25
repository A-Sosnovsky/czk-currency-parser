using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Parser.Services.Report;
using Parser.WebApi.Reports;

namespace Parser.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyReportController : ControllerBase
    {
        private readonly ICurrencyReportService _reportService;

        public CurrencyReportController(ICurrencyReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public MonthReport Get([Required] [Range(1, 12)] int month,
            [Required] int year)
        {
            var report = _reportService.GetMonthReport(month, year);

            return new MonthReport
            {
                Date = report.Date,
                WeekPeriods = report.Weeks
                    .GroupBy(w => new {w.WeekNumber, w.StartDay, w.EndDay})
                    .OrderBy(w => w.Key.WeekNumber)
                    .Select(grouping => new MonthReportWeekPeriod
                    {
                        WeekStartDay = grouping.Key.StartDay,
                        WeekEndDay = grouping.Key.EndDay,
                        Rows = grouping.Select(w => new WeekPeportRow
                        {
                            CurrencyName = w.CurrencyName,
                            Max = w.MaxValue,
                            Min = w.MinValue,
                            Median = w.MedianValue
                        })
                    })
            };
        }
    }
}